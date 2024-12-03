using Microsoft.EntityFrameworkCore;
using Epic_Arts_Entertainment.Repositorios;
using Epic_Arts_Entertainment.ORM;

var builder = WebApplication.CreateBuilder(args);

// Registrar o DbContext se necessário
builder.Services.AddDbContext<BdEpicArtsEntertainmentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar o repositório (UsuarioRepositorio)
builder.Services.AddScoped<UsuarioRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso

// Registrar o repositório (ServicoRepositorio)
builder.Services.AddScoped<ServicoRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso

// Registrar o repositório (AgendamentoRepositorio)
builder.Services.AddScoped<AgendamentoRepositorio>();  // Adicionando AgendamentoRepositorio

// Registrar outros serviços, como controllers com views
builder.Services.AddControllersWithViews();

// Adiciona suporte para sessões
builder.Services.AddDistributedMemoryCache();  // Usando memória para cache de sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Defina o tempo de expiração da sessão
    options.Cookie.HttpOnly = true;  // Protege o cookie contra acesso JavaScript
    options.Cookie.IsEssential = true;  // Permite que a sessão seja essencial para a aplicação
});

var app = builder.Build();

// Configure o pipeline de requisições
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Habilita o uso de sessões
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();