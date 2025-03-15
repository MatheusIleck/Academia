using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Clients;
using AcademiaBackEnd.Services.Professor;

namespace AcademiaBackEnd.Endpoints.Professor
{
    public class GetProfessorProfileEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
       => app.MapGet("/professor/profile", HandleAsync)
             .WithName("View Professor Profile")
             .WithSummary("View Profile")
             .WithDescription("Fetch the current user's profile details.")
             .WithOrder(3)
             .Produces<Response<ProfessorDto>>()
             .RequireAuthorization();

        private static async Task<IResult> HandleAsync(
            IProfessorInterface _professorInterface,
            HttpContext _httpContext
            )
        {
            var request = new GetProfessorProfileRequest
            {
                ClientId = long.Parse(_httpContext.User.FindFirst("UserId").Value)
            };

            var result = await _professorInterface.GetProfessorProfileAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
