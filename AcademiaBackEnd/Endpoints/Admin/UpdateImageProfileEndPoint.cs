using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Services.Admin;
using AcademiaBackEnd.Services.Clients;

namespace AcademiaBackEnd.Endpoints.Admin
{
    public class UpdateImageProfileEndPoint : IEndPoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/client/updateimage", HandleAsync);


        private static async Task<IResult> HandleAsync(
        HttpContext _httpContext,
        IAdminInterface _admininterface,
        IFormFile image)
        {
            var clientId = long.Parse(_httpContext.User.FindFirst("UserId").Value);

            var request = new UpdateImageProfileRequest
            {
                image = image
            };

            var result = await _admininterface.UpdateImageProfile(request, clientId);

            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        }


    }
}
