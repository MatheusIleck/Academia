
using AcademiaBackEnd.Endpoints;
using AcademiaBackEnd.Extensions;

namespace AcademiaBackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.AddArchitectures();
            builder.AddDataContexts();
            builder.AddCrossOrigin();
            builder.AddDocumentation();
            builder.AddServices();
            builder.AddAuthentication();


            var app = builder.Build();


            if (app.Environment.IsDevelopment())
                app.ConfigureDevEnvironment();

            app.UseAuthentication(); // O JWT é suficiente para autenticação
            app.UseAuthorization();

            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.MapEndPoints();

            app.Run();
        }
    }
}
