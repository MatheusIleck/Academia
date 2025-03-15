using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Extensions;

using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using AcademiaBackEnd.Services.Admin;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaBackEnd.Endpoints.Admin
{
    public class RegisterNewUserEndPoint : IEndPoint
    {

        public static void Map(IEndpointRouteBuilder app)
         => app.MapPost("/RegisterNewClient", HandleAsync)
             .WithName("Client: Registra um cliente.")
             .WithSummary("Registra um novo cliente.")
             .WithDescription("Registra um novo cliente.")
             .WithOrder(1)
             .Produces<Response<Usuario>>();


        private static async Task<IResult> HandleAsync(
            [FromBody] RegisterNewClientRequest request, IAdminInterface _adminInterface)
        {
            

                var result = await _adminInterface.RegisterNewClient(request);

                return result.IsSuccess
                    ? TypedResults.Ok(result)
                    : TypedResults.BadRequest(result);

        }

    }
}