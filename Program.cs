using Microsoft.EntityFrameworkCore;
using Epic_Arts_Entertainment.Repositorios;
using Epic_Arts_Entertainment.ORM;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Registrar o DbContext se necess�rio
builder.Services.AddDbContext<BdEpicArtsEntertainmentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar o reposit�rio (UsuarioRepositorio)
builder.Services.AddScoped<UsuarioRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso
// Registrar o reposit�rio (ServicoRepositorio)
builder.Services.AddScoped<ServicoRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso
// Registrar o reposit�rio (AgendamentoRepositorio)
builder.Services.AddScoped<AgendamentoRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso
// Registrar o reposit�rio (RelatorioRepositorio)
builder.Services.AddScoped<RelatorioRepositorio>();  // Ou AddTransient ou AddSingleton dependendo do caso

// Registrar o IHttpContextAccessor para acessar o HttpContext dentro de reposit�rios
builder.Services.AddHttpContextAccessor();

// Adicionar suporte a sess�es
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Defina o tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true; // Garante que o cookie de sess�o n�o seja acess�vel via JavaScript
    options.Cookie.IsEssential = true; // O cookie � essencial para a opera��o do site
});

// Adicionar suporte a autentica��o com cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";  // Caminho para a p�gina de login
        options.LogoutPath = "/Usuario/Logout";  // Caminho para a p�gina de logout
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Tempo de expira��o do cookie
    });

// Registrar outros servi�os, como controllers com views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar o pipeline de requisi��o HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Adicionar o middleware de sess�o
app.UseSession();

// Colocar a ordem correta dos middlewares de autentica��o e autoriza��o
app.UseRouting();

// Middleware de autentica��o: permite identificar quem � o usu�rio
app.UseAuthentication();  // Coloque antes de UseAuthorization()

// Middleware de autoriza��o: verifica se o usu�rio tem permiss�o para acessar a rota
app.UseAuthorization();   // Deve ser chamado depois de UseAuthentication()

// Configurar as rotas dos controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Rodar a aplica��o
app.Run();