using Microsoft.EntityFrameworkCore;
using Work_Wave.Data;
using Work_Wave.Models;
using Work_Wave.Services.Interfaces;

namespace Work_Wave.Services
{
    public class TicketService : ITTicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            try
            {
                List<Ticket> tickets = await _context.Tickets
                    .Include(t => t.Comments)
                    .Include(t => t.Technician)
                    .Include(t => t.Support)
                    .Include(t => t.Priority)
                    .ToListAsync();

                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                Ticket ticket = (await GetAllTicketsAsync()).FirstOrDefault(t => t.Id == ticketId);

                return ticket;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddTicketCommentAsync(Comment comment)
        {
            try
            {
                await _context.AddAsync(comment);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByPriorityAsync(string priorityName)
        {
            int priorityId = (await LookupTicketPriorityIdAsync(priorityName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Tickets
                    .Include(t => t.Comments)
                    .Include(t => t.Technician)
                    .Include(t => t.Support)
                    .Include(t => t.Priority)
                    .Where(t => t.PriorityId == priorityId)
                    .ToListAsync();

                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int?> LookupTicketPriorityIdAsync(string priorityName)
        {
            try
            {
                Priority priority = await _context.Priorities.FirstOrDefaultAsync(p => p.Name == priorityName);

                return priority?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
