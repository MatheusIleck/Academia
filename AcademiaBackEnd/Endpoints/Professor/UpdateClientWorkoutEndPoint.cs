using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Services.Professor;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaBackEnd.Endpoints.Professor
{
    public class UpdateClientWorkoutEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
         => app.MapPut("/professor/updateworkout", HandleAsync);

        private static async Task<IResult> HandleAsync(
           IProfessorInterface _professorInterface,
           [FromBody] UpdateClientWorkoutRequest request)
        {

            var result = await _professorInterface.UpdateClientWorkout(request);

            return result.IsSuccess
                 ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }

}
