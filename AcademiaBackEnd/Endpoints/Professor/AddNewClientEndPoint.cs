using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Services.Professor;

namespace AcademiaBackEnd.Endpoints.Professor
{
    public class AddNewClientEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/professor/AddNewClient", HandleAsync);
        private static async Task<IResult> HandleAsync(
              AddNewClientRequest request,
              IProfessorInterface _professorInterface,
            HttpContext _httpContext
              )
        {

            request.professorId = long.Parse(_httpContext.User.FindFirst("UserId").Value);
            var result = await _professorInterface.AddNewClientAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }

}
