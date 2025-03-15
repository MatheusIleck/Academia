using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Services.Professor;

namespace AcademiaBackEnd.Endpoints.Professor
{
    public class GetClientProfileEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/professor/findclient/{id}", HandleAsync);

        private static async Task<IResult> HandleAsync(
            long id,
            IProfessorInterface _professorInterface
            )
        {

            var request = new GetClientProfileRequest
            {
                userId = id,

            };

            var result = await _professorInterface.GetClientProfileAsync( request );

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
