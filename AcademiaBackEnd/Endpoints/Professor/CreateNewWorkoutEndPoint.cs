using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Services.Professor;

namespace AcademiaBackEnd.Endpoints.Professor
{
    public class CreateNewWorkoutEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/professor/newWorkout", HandleAsync);

        private static async Task<IResult> HandleAsync(
            CreateNewWorkoutRequest treino, 
            IProfessorInterface _professorInterface)
        {

            var result = await _professorInterface.CreateNewWorkout(treino);

            return result.IsSuccess
                ?  TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
