using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Clients;

namespace AcademiaBackEnd.Endpoints.Clients
{
    public class GetAllWorkoutsEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
       => app.MapGet("/client/workouts", HandleAsync)
            .WithName("Busca todos os treinos.")
            .WithSummary("Busca todos os treinos.")
            .WithDescription("Busca todos os treinos.")
            .Produces<Response<List<WorkoutsDto>>>()
            .RequireAuthorization();

        private static async Task<IResult> HandleAsync(
            IClientInterface _clientInterface
            )
        {
            var result = await _clientInterface.GetAllWorkout();

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
