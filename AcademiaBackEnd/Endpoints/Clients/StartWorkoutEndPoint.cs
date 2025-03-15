using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Clients;

namespace AcademiaBackEnd.Endpoints.Clients
{
    public class StartWorkoutEndPoint : IEndPoint

    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/workouts/start", HandleAsync)
             .WithName("Inicia um Treino.")
             .WithSummary("Inicia um Treino.")
             .WithDescription("Inicia um Treino.")
             .Produces<Response<SessaoTreino>>()
             .WithOrder(4)
             .RequireAuthorization();

        private static async Task<IResult> HandleAsync(
            StartWorkoutRequest request,
            IClientInterface _clientInterface,
            HttpContext _httpContext
            )
        {

            request.ClientId = long.Parse(_httpContext.User.FindFirst("UserId").Value);

            var result = await _clientInterface.StartWorkout(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);


        }
    }
}
