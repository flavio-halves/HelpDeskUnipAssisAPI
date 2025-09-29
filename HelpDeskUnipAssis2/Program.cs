using HelpDeskUnipAssis.Data;         // DbContext
using HelpDeskUnipAssis.Models;       // Ticket, Usuario (se estiverem nesse namespace)
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==== Web (Razor Pages) ====
builder.Services.AddRazorPages();

// ==== EF Core + SQL Server ====
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==== Swagger (API) ====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==== CORS (aberto em dev; feche em produção) ====
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// ===== Middlewares =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("dev");

// Se você usa Auth, mantenha:
app.UseAuthorization();

// ===== Web (Razor Pages) =====
app.MapRazorPages();

// Força a tela inicial ser o Login (mantido do seu código)
app.MapGet("/", context =>
{
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

// ===================== API =====================
// ------- Usuarios -------
app.MapGet("/api/usuarios", async (ApplicationDbContext db) =>
    await db.Usuarios.AsNoTracking().ToListAsync());

app.MapGet("/api/usuarios/{id:int}", async (int id, ApplicationDbContext db) =>
    await db.Usuarios.FindAsync(id) is { } u ? Results.Ok(u) : Results.NotFound());

app.MapPost("/api/usuarios", async (Usuario usuario, ApplicationDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(usuario.Nome) ||
        string.IsNullOrWhiteSpace(usuario.Email) ||
        string.IsNullOrWhiteSpace(usuario.Senha))
        return Results.BadRequest("Nome, Email e Senha são obrigatórios.");

    db.Usuarios.Add(usuario);
    await db.SaveChangesAsync();
    return Results.Created($"/api/usuarios/{usuario.Id}", usuario);
});

app.MapPut("/api/usuarios/{id:int}", async (int id, Usuario input, ApplicationDbContext db) =>
{
    var usuario = await db.Usuarios.FindAsync(id);
    if (usuario is null) return Results.NotFound();

    usuario.Nome = input.Nome;
    usuario.Email = input.Email;
    if (!string.IsNullOrWhiteSpace(input.Senha)) usuario.Senha = input.Senha;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/usuarios/{id:int}", async (int id, ApplicationDbContext db) =>
{
    var usuario = await db.Usuarios.FindAsync(id);
    if (usuario is null) return Results.NotFound();

    db.Usuarios.Remove(usuario);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ------- Tickets -------
app.MapGet("/api/tickets", async (ApplicationDbContext db) =>
    await db.Tickets.AsNoTracking()
        .Include(t => t.Usuario)
        .ToListAsync());

app.MapGet("/api/tickets/{id:int}", async (int id, ApplicationDbContext db) =>
{
    var ticket = await db.Tickets
        .Include(t => t.Usuario)
        .FirstOrDefaultAsync(t => t.Id == id);

    return ticket is null ? Results.NotFound() : Results.Ok(ticket);
});

app.MapPost("/api/tickets", async (Ticket ticket, ApplicationDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(ticket.Titulo) || string.IsNullOrWhiteSpace(ticket.Descricao))
        return Results.BadRequest("Título e Descrição são obrigatórios.");

    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();
    return Results.Created($"/api/tickets/{ticket.Id}", ticket);
});

app.MapPut("/api/tickets/{id:int}", async (int id, Ticket input, ApplicationDbContext db) =>
{
    var ticket = await db.Tickets.FindAsync(id);
    if (ticket is null) return Results.NotFound();

    ticket.Titulo = input.Titulo;
    ticket.Descricao = input.Descricao;
    ticket.UsuarioId = input.UsuarioId;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/tickets/{id:int}", async (int id, ApplicationDbContext db) =>
{
    var ticket = await db.Tickets.FindAsync(id);
    if (ticket is null) return Results.NotFound();

    db.Tickets.Remove(ticket);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ==============================================
app.Run();




//Web(sua UI): https://localhost:7154/  redireciona para /Login

//Swagger(API): https://localhost:7154/swagger

//Endpoints: https://localhost:7154/api/usuarios, https://localhost:7154/api/tickets, etc.

