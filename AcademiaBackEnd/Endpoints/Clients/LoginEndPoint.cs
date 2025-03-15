using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Clients;

namespace AcademiaBackEnd.Endpoints.Clients
{
    public class LoginEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
       => app.MapPost("/Login", HandleAsync)
            .WithName("Login")
             .WithSummary("Efetua o Login de um cliente.")
             .WithDescription("Efetua o Login de um cliente.")
             .WithOrder(1)
             .Produces<Response<string>>();

        private static async Task<IResult> HandleAsync(
            LoginRequest request, 
            IClientInterface _clientInterface)
        {
            var result = await _clientInterface.Login(request);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
