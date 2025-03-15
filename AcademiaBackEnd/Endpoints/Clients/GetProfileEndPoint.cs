using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Clients;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AcademiaBackEnd.Endpoints.Clients
{
    public class GetProfileEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/client/profile", HandleAsync)
             .WithName("Perfil")
             .WithSummary("Acessa o perfil do usuario.")
             .WithDescription("Acessa o perfil do usuario.")
             .WithOrder(3)
             .Produces<Response<ClientDto>>()
             .RequireAuthorization();

        private static async Task<IResult> HandleAsync(
            IClientInterface _clientInterface,
            HttpContext _httpContext
            )
        {
            var request = new GetProfileRequest
            {
                ClientId = long.Parse(_httpContext.User.FindFirst("UserId").Value)
            };

            var result = await _clientInterface.GetProfileAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
