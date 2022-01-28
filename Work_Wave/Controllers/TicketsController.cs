#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Work_Wave.Data;
using Work_Wave.Models;

namespace Work_Wave.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<WaveUser> _userManager;

        public TicketsController(ApplicationDbContext context, UserManager<WaveUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tickets.Include(t => t.Priority).Include(t => t.Support).Include(t => t.Technician);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tickets - MY TICKETS
        public async Task<IActionResult> MyTickets()
        {
            var applicationDbContext = _context.Tickets.Include(t => t.Priority).Include(t => t.Support).Include(t => t.Technician);

            WaveUser user = await _userManager.GetUserAsync(User);

            List<Ticket> tickets = new List<Ticket>();

            foreach (var t in applicationDbContext)
            {
                if (t.SupportId == user.Id)
                {
                    tickets.Add(t);
                }
                else if (t.TechnicianId == user.Id)
                {
                    tickets.Add(t);
                }
            }
            return View(tickets);
        }

        // GET: Tickets - ADMIN VIEW
        public async Task<IActionResult> Admin()
        {
            var applicationDbContext = _context.Tickets.Include(t => t.Priority).Include(t => t.Support).Include(t => t.Technician);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Priority)
                .Include(t => t.Support)
                .Include(t => t.Technician)
                .FirstOrDefaultAsync(m => m.Id == id);

            WaveUser user = await _userManager.GetUserAsync(User);

            if (ticket == null)
            {
                return NotFound();
            }

                return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name");
            ViewData["TechnicianId"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CFirstName,CLastName,CPhone,CAddress,CCity,CState,CZip,Created,Schedule,PriorityId,TechnicianId,SupportId")] Ticket ticket)
        {
            WaveUser user = await _userManager.GetUserAsync(User);

            ticket.SupportId = user.Id;

            ticket.Created = DateTimeOffset.Now;

            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", ticket.PriorityId);
            ViewData["TechnicianId"] = new SelectList(_context.Users, "Id", "FullName", ticket.TechnicianId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", ticket.PriorityId);
            ViewData["SupportId"] = new SelectList(_context.Users, "Id", "FullName", ticket.SupportId);
            ViewData["TechnicianId"] = new SelectList(_context.Users, "Id", "FullName", ticket.TechnicianId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CFirstName,CLastName,CPhone,CAddress,CCity,CState,CZip,Schedule,IsArchived,PriorityId,TechnicianId,SupportId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", ticket.PriorityId);
            ViewData["SupportId"] = new SelectList(_context.Users, "Id", "FullName", ticket.SupportId);
            ViewData["TechnicianId"] = new SelectList(_context.Users, "Id", "FullName", ticket.TechnicianId);
            return View(ticket);
        }

        // Ticket Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicketComment([Bind("Id, TicketId, Note")] Comment comment)
        {

                comment.UserId = _userManager.GetUserId(User);

                comment.Created = DateTimeOffset.Now;

                await _context.AddAsync(comment);
                await _context.SaveChangesAsync();
    

            return RedirectToAction("Details", new { id = comment.Id });
        }
        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Priority)
                .Include(t => t.Support)
                .Include(t => t.Technician)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            ticket.IsArchived = true;

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Retore/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Priority)
                .Include(t => t.Support)
                .Include(t => t.Technician)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            ticket.IsArchived = false;

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
