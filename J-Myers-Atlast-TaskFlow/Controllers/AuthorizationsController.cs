using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using J_Myers_Atlast_TaskFlow.Models;
using J_Myers_Atlast_TaskFlow.Data;

// Sprint 6: Require authentication for all actions in this controller by default
[Authorize]
public class AuthorizationsController : Controller
{
    private readonly TaskFlowContext _context;

    public AuthorizationsController(TaskFlowContext context)
    {
        _context = context;
    }

    // GET: Authorizations (Accessible to all authenticated clinic roles)
    public async Task<IActionResult> Index()
    {
        return View(await _context.AuthorizationTasks.ToListAsync());
    }

    // GET: Authorizations/Details/5 (Accessible to all authenticated clinic roles)
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var authorizationtask = await _context.AuthorizationTasks
            .FirstOrDefaultAsync(m => m.TaskId == id);
        if (authorizationtask == null)
        {
            return NotFound();
        }

        return View(authorizationtask);
    }

    // GET: Authorizations/Create (Restricted to Prior Auth Specialists and Management)
    [Authorize(Roles = "Specialist,Management")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Authorizations/Create
    [HttpPost]
    [Authorize(Roles = "Specialist,Management")]
    public async Task<IActionResult> Create(AuthorizationTask authorizationtask)
    {
        var defaultProject = await _context.Projects.FirstOrDefaultAsync();
        if (defaultProject == null)
        {
            defaultProject = new Project
            {
                Name = "Default Prior Auth Project",
                Description = "Default project for tracking insurance authorizations",
                CreatedDate = DateTime.Now
            };
            _context.Projects.Add(defaultProject);
            await _context.SaveChangesAsync();
        }

        // Tag the task with the authenticated user's identity for ownership tracking
        var currentUser = User.Identity?.Name ?? "staff";
        var currentDisplayName = User.FindFirst("DisplayName")?.Value ?? currentUser;

        authorizationtask.Title = string.IsNullOrWhiteSpace(authorizationtask.PatientCaseNumber)
            ? $"Prior Auth (Owner: {currentUser})"
            : $"Auth Case: {authorizationtask.PatientCaseNumber} (Owner: {currentUser})";

        authorizationtask.Description = $"Created by {currentDisplayName}";
        authorizationtask.Priority = 1;
        authorizationtask.IsCompleted = false;
        authorizationtask.ProjectId = defaultProject.ProjectId;

        // Ensure Notes satisfies the database NOT NULL constraint
        if (string.IsNullOrWhiteSpace(authorizationtask.Notes))
        {
            authorizationtask.Notes = "Initial intake - no notes entered";
        }

        ModelState.Clear();

        _context.Add(authorizationtask);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Authorizations/Edit/5 (Restricted to Specialists, Denial Specialists, and Management)
    [Authorize(Roles = "Specialist,DenialSpecialist,Management")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var authorizationtask = await _context.AuthorizationTasks.FindAsync(id);
        if (authorizationtask == null)
        {
            return NotFound();
        }

        // Sprint 6 Data Ownership Check: Specialists cannot edit tasks created by other staff/specialists
        var currentDisplayName = User.FindFirst("DisplayName")?.Value ?? User.Identity?.Name ?? "";
        if (User.IsInRole("Specialist") &&
            authorizationtask.Description != null &&
            authorizationtask.Description.StartsWith("Created by ") &&
            !authorizationtask.Description.Contains(currentDisplayName))
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        return View(authorizationtask);
    }

    // POST: Authorizations/Edit/5
    [HttpPost]
    [Authorize(Roles = "Specialist,DenialSpecialist,Management")]
    public async Task<IActionResult> Edit(int id, AuthorizationTask authorizationtask)
    {
        if (id != authorizationtask.TaskId)
        {
            return NotFound();
        }

        // Verify ownership on POST to prevent parameter tampering (IDOR protection)
        var existingTask = await _context.AuthorizationTasks.AsNoTracking().FirstOrDefaultAsync(t => t.TaskId == id);
        var currentDisplayName = User.FindFirst("DisplayName")?.Value ?? User.Identity?.Name ?? "";
        if (existingTask != null && User.IsInRole("Specialist") &&
            existingTask.Description != null &&
            existingTask.Description.StartsWith("Created by ") &&
            !existingTask.Description.Contains(currentDisplayName))
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        if (string.IsNullOrWhiteSpace(authorizationtask.Title))
        {
            authorizationtask.Title = existingTask?.Title ?? $"Auth Case: {authorizationtask.PatientCaseNumber}";
        }
        if (string.IsNullOrWhiteSpace(authorizationtask.Description))
        {
            authorizationtask.Description = existingTask?.Description ?? $"Updated by {currentDisplayName}";
        }
        if (string.IsNullOrWhiteSpace(authorizationtask.Notes))
        {
            authorizationtask.Notes = existingTask?.Notes ?? "No notes provided";
        }
        if (authorizationtask.Priority <= 0)
        {
            authorizationtask.Priority = 1;
        }

        ModelState.Clear();

        try
        {
            _context.Update(authorizationtask);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorizationTaskExists(authorizationtask.TaskId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: Authorizations/Delete/5 (Restricted strictly to Management)
    [Authorize(Roles = "Management")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var authorizationtask = await _context.AuthorizationTasks
            .FirstOrDefaultAsync(m => m.TaskId == id);
        if (authorizationtask == null)
        {
            return NotFound();
        }

        return View(authorizationtask);
    }

    // POST: Authorizations/Delete/5
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Management")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var authorizationtask = await _context.AuthorizationTasks.FindAsync(id);
        if (authorizationtask != null)
        {
            _context.AuthorizationTasks.Remove(authorizationtask);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AuthorizationTaskExists(int id)
    {
        return _context.AuthorizationTasks.Any(e => e.TaskId == id);
    }
}