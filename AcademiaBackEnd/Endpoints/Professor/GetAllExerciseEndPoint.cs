using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Services.Professor;

namespace AcademiaBackEnd.Endpoints.Professor
{
    public class GetAllExerciseEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/professor/exercises", HandleAsync);

        private static async Task<IResult> HandleAsync(
            IProfessorInterface _professorInterface
            )
        {

            var result = await _professorInterface.GetAllExercise();

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
