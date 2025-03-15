using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Clients;

namespace AcademiaBackEnd.Endpoints.Clients
{
    public class GetWorkoutSessionEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
      => app.MapGet("/client/Profile/workoutsessions", HandleAsync)
             .WithName("Acessa o Historico.")
             .WithSummary("Retorna o historico do cliente.")
             .WithDescription("Retorna o historico do cliente.")
             .Produces<Response<List<WorkoutSessionDto>>>()
            .RequireAuthorization();

        private static async Task<IResult> HandleAsync(
            IClientInterface _clientInterface,
            HttpContext _httpContext
            )
        {
            var request = new GetWorkoutsSessionsRequest
            {
                ClientId = long.Parse(_httpContext.User.FindFirst("UserId").Value),
            };
            var result = await _clientInterface.GetWorkoutSessions(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);  
        }
    }
}
