using Microsoft.EntityFrameworkCore;
using Epic_Arts_Entertainment.Repositorios;
using Epic_Arts_Entertainment.ORM;

var builder = WebApplication.CreateBuilder(args);

// Registrar o DbContext se necess�rio
builder.Services.AddDbContext<BdEpicArtsEntertainmentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar o reposit�rio (UsuarioRepositorio)
builder.Services.AddScoped<UsuarioRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso

// Registrar o reposit�rio (ServicoRepositorio)
builder.Services.AddScoped<ServicoRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso

// Registrar o reposit�rio (AgendamentoRepositorio)
builder.Services.AddScoped<AgendamentoRepositorio>();  // Adicionando AgendamentoRepositorio

// Registrar outros servi�os, como controllers com views
builder.Services.AddControllersWithViews();

// Adiciona suporte para sess�es
builder.Services.AddDistributedMemoryCache();  // Usando mem�ria para cache de sess�o
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Defina o tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true;  // Protege o cookie contra acesso JavaScript
    options.Cookie.IsEssential = true;  // Permite que a sess�o seja essencial para a aplica��o
});

var app = builder.Build();

// Configure o pipeline de requisi��es
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Habilita o uso de sess�es
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();