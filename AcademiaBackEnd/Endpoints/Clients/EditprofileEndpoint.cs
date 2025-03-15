using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Clients;
using Microsoft.AspNetCore.Http;

namespace AcademiaBackEnd.Endpoints.Clients
{
    public class EditprofileEndpoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
      => app.MapPut("client/profile/edit", HandleAsync)
             .WithName("Edita o Perfil do cliente.")
             .WithSummary("Edita as informações de perfil.")
             .WithDescription("Edita as informações de perfil.")
             .WithOrder(3)
             .Produces<Response<ClientDto>>()
             .RequireAuthorization();

        private static async Task<IResult> HandleAsync(
            EditProfileRequest request,
            IClientInterface _clientInterface,
            HttpContext _httpContext)
        {
            request.ClientId = long.Parse(_httpContext.User.FindFirst("UserId").Value);
            var result = await _clientInterface.EditProfileAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);

        }
    }
}
