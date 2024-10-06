using Microsoft.EntityFrameworkCore;
using Sessions_app.Data;
using Sessions_app.Services;
using AutoMapper; // Importante para AutoMapper
using Sessions_app.Mapeamento; // Importante para o perfil de mapeamento

var builder = WebApplication.CreateBuilder(args);

// Adicione os servi�os ao cont�iner
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.Name = "_ApplicationSession";
    options.Cookie.IsEssential = true;
});

// Configura��o do DbContext para usar MySQL
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))); // Altere a vers�o conforme necess�rio
});

// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(PacienteProfile)); // Registra o perfil do AutoMapper

// Registrar Reposit�rio e Servi�o
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<PacienteService>();

var app = builder.Build();

// Configure o pipeline de solicita��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Paciente/Index");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Paciente}/{action=Index}/{id?}");

app.Run();
