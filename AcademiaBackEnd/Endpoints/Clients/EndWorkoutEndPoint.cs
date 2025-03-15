using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Clients;

namespace AcademiaBackEnd.Endpoints.Clients
{
    public class EndWorkoutEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/finishworkout/", HandleAsync)
                .WithName("Finaliza um treino ativo.")
                .WithSummary("Finaliza um treino ativo.")
                .WithDescription("Finaliza um treino ativo.")
                .Produces<Response<Usuario>>()
                .WithOrder(6)
                .RequireAuthorization();

        private static async Task<IResult> HandleAsync(
            EndWorkoutRequest request,
            IClientInterface _clientInterface,
            HttpContext _httpContext
            )
        {
            request.ClientId = long.Parse(_httpContext.User.FindFirst("UserId").Value);

            var result = await  _clientInterface.EndWorkout(request);

            return result
                    ? TypedResults.Ok(result)
                    : TypedResults.BadRequest(result);
        }
    }
}
