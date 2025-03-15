using AcademiaFrontEnd.Services;

namespace AcademiaFrontEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache(); // Mem�ria local para sess�o
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Expira��o da sess�o ap�s 30 minutos
                options.Cookie.HttpOnly = true;                 // Protege contra acesso via JavaScript
                options.Cookie.IsEssential = true;              // Necess�rio para conformidade com pol�ticas de cookies
            });

            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7166"); 
            });

            builder.Services.AddScoped<IApiInterface, ApiService>();


            var app = builder.Build();


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
