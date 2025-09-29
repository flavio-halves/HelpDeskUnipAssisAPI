using HelpDeskUnipAssis.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

// Conex�o com o SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

// Aqui a gente for�a a tela inicial ser o Login
app.MapGet("/", context =>
{
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

app.Run();