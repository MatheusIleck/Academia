using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Admin;

namespace AcademiaBackEnd.Endpoints.Admin
{
    public class RemoveClientEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/ClientRemove/{id}", HandleAsync) 
             .WithName("Client: Remove um cliente.")
             .WithSummary("Remove um cliente.")
             .WithDescription("Remove um cliente.")
             .WithOrder(2)
             .Produces<Response<Usuario>>();
            

        private static async Task<IResult> HandleAsync(
            long id,
            IAdminInterface _adminInterface
           )
        {
            var request = new RemoveClientRequest { Id = id };

            var result = await _adminInterface.DeleteClient(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
