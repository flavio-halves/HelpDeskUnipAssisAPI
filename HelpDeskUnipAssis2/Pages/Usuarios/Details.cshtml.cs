using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HelpDeskUnipAssis.Data;
using HelpDeskUnipAssis.Models;

namespace HelpDeskUnipAssis2.Pages.Usuarios
{
    public class DetailsModel : PageModel
    {
        private readonly HelpDeskUnipAssis.Data.ApplicationDbContext _context;

        public DetailsModel(HelpDeskUnipAssis.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                Usuario = usuario;
            }
            return Page();
        }
    }
}
