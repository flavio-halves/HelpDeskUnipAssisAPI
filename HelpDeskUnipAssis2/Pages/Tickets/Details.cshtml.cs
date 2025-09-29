using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HelpDeskUnipAssis.Data;
using HelpDeskUnipAssis.Models;

namespace HelpDeskUnipAssis2.Pages.Tickets
{
    public class DetailsModel : PageModel
    {
        private readonly HelpDeskUnipAssis.Data.ApplicationDbContext _context;

        public DetailsModel(HelpDeskUnipAssis.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Ticket Ticket { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            else
            {
                Ticket = ticket;
            }
            return Page();
        }
    }
}
