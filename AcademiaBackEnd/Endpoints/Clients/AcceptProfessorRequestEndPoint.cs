using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Services.Clients;

namespace AcademiaBackEnd.Endpoints.Clients
{
    public class AcceptProfessorRequestEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("client/profile/acceptrequest", HandleAsync);

        private static async Task<IResult> HandleAsync(
             AcceptProfessorRequest request,
             IClientInterface _clientInterface,
             HttpContext _httpContext)
        {
            var result = await _clientInterface.ApproveRequest(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);

        }
    }
}
