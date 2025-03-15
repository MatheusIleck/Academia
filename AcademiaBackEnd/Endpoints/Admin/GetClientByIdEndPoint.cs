using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Admin;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Admin;

namespace AcademiaBackEnd.Endpoints.Admin
{
    public class GetClientByIdEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/getClient/{id}", HandleAsync)
            .WithName("Admin: Busca um cliente.")
            .WithSummary("Busca um cliente pelo Id.")
            .WithDescription("Busca um cliente pelo Id.")
            .Produces<Response<Usuario>>();
        private static async Task<IResult> HandleAsync(long id, IAdminInterface _adminInterface)
        {
            var request = new GetClientByIdRequest
            {
                id = id
            };

            var result = await _adminInterface.GetClientById(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
