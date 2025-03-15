using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Professor;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaBackEnd.Endpoints.Professor
{
    public class ListAllClientsEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/professor/clients", HandleAsync)
             .WithName("View Clients")
             .WithSummary("View Clients")
             .Produces<Response<List<ClientDto>>>();

        private static async Task<IResult> HandleAsync(
            IProfessorInterface _professorInterface,
            HttpContext _httpContext)
        {
            var request = new GetAllClientsRequest
            {
                ClientId = long.Parse(_httpContext.User.FindFirst("UserId").Value)
            };

            var result = await _professorInterface.GetClientsAsync(request);

            return result.IsSuccess
                  ? TypedResults.Ok(result)
                  : TypedResults.BadRequest(result);
        }
    }
}
