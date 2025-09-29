using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HelpDeskUnipAssis.Data;

namespace HelpDeskUnipAssis.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Senha { get; set; } = string.Empty;

        public string Mensagem { get; set; } = "";

        public IActionResult OnGet()
        {
            // 👉 Se não existe nenhum usuário no banco, redireciona para a tela de cadastro
            if (!_context.Usuarios.Any())
            {
                return RedirectToPage("/Usuarios/Create");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == Email && u.Senha == Senha);

            if (usuario == null)
            {
                Mensagem = "Usuário ou senha incorretos!";
                return Page();
            }

            return RedirectToPage("/Tickets/Index");
        }
    }
}