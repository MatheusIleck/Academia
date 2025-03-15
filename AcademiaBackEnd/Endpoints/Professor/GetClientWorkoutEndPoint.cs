using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Professor;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaBackEnd.Endpoints.Professor
{
    public class GetClientWorkoutEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/professor/GetClientWorkout/{idtreino}", HandleAsync)
                .WithName("Busca um treino de um cliente")
                .WithSummary("Busca um treino de um cliente")
                .WithDescription("Busca um treino de um cliente")
                .Produces<Response<WorkoutsDto>>()
                .RequireAuthorization();
       


        private static async Task<IResult> HandleAsync(
        IProfessorInterface _professorInterface,
        long idtreino)
        {


            var result = await _professorInterface.GetClientWorkout(idtreino);

            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }

            return TypedResults.BadRequest(new Response<WorkoutsDto>(null, 400, "Erro ao buscar treino."));
        }


    }
}
