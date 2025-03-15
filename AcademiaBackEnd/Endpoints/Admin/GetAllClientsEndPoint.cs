using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Admin;

namespace AcademiaBackEnd.Endpoints.Admin
{
    public class GetAllClientsEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/Clients", HandleAsync)
            .WithName("Admin: Busca todos os clientes.")
            .WithSummary("Busca todos os clientes.")
            .WithDescription("Busca todos os clientes.")
            .Produces<Response<Usuario>>();

        private static async Task<IResult> HandleAsync(IAdminInterface _adminInterface)
        {
            var result = await _adminInterface.GetAllClients();

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
