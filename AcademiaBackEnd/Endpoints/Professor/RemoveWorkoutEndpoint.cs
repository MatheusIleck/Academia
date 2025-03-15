using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Services.Professor;

namespace AcademiaBackEnd.Endpoints.Professor
{
    public class RemoveWorkoutEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
         => app.MapPost("/professor/removeworkout", HandleAsync);

        private static async Task<IResult> HandleAsync(
            RemoveWorkoutRequest request,
            IProfessorInterface _professorInterface,
            HttpContext _httpContext
            )
        {

            request.ClientId = long.Parse(_httpContext.User.FindFirst("UserId").Value);
          

            var result = await _professorInterface.RemoveWorkout(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
