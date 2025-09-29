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
    public class IndexModel : PageModel
    {
        private readonly HelpDeskUnipAssis.Data.ApplicationDbContext _context;

        public IndexModel(HelpDeskUnipAssis.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Usuario> Usuario { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Usuario = await _context.Usuarios.ToListAsync();
        }
    }
}
