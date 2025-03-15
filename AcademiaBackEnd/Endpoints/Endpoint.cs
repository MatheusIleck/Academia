using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Endpoints.Admin;
using AcademiaBackEnd.Endpoints.Clients;
using AcademiaBackEnd.Endpoints.Professor;
using AcademiaBackEnd.Extensions;
using AcademiaBackEnd.Request.Clients;
using Microsoft.AspNetCore.Authorization;

namespace AcademiaBackEnd.Endpoints
{
    public static class Endpoint
    {
        public static void MapEndPoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("");


            endpoints.MapGroup("v1")
             .WithTags("Health Check")
             .MapGet("/", () => new { message = "OK" });


            endpoints.MapGroup("v1")
           .WithTags("Login")
           .MapEndpoint<LoginEndPoint>();

            endpoints.MapGroup("v1")
                .WithTags("Admin")
                .DisableAntiforgery()

                .MapEndpoint<RegisterNewUserEndPoint>()
                .MapEndpoint<RemoveClientEndPoint>()
                .MapEndpoint<GetClientByIdEndPoint>()
                .MapEndpoint<GetAllClientsEndPoint>()
                .MapEndpoint<UpdateImageProfileEndPoint>();

            endpoints.MapGroup("v1")
               .WithTags("Professor")
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Professor" })
                .MapEndpoint<GetClientProfileEndPoint>()
                .MapEndpoint<ListAllClientsEndPoint>()
                .MapEndpoint<CreateNewWorkoutEndPoint>()
                .MapEndpoint<GetProfessorProfileEndPoint>()
                .MapEndpoint<RemoveWorkoutEndpoint>()
                .MapEndpoint<GetClientWorkoutEndPoint>()
                .MapEndpoint<GetAllExerciseEndPoint>()
                .MapEndpoint<UpdateClientWorkoutEndPoint>()
                .MapEndpoint<AddNewClientEndPoint>();




            endpoints.MapGroup("v1")
                .WithTags("Client")
                .RequireAuthorization(new AuthorizeAttribute { Roles = "Client" })
                .DisableAntiforgery()
                .MapEndpoint<GetProfileEndPoint>()
                .MapEndpoint<EditprofileEndpoint>()
                .MapEndpoint<GetAllWorkoutsEndPoint>()
                .MapEndpoint<StartWorkoutEndPoint>()
                .MapEndpoint<EndWorkoutEndPoint>()
                .MapEndpoint<GetWorkoutSessionEndPoint>()
                .MapEndpoint<AcceptProfessorRequestEndPoint>();




        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
          where TEndpoint : IEndPoint
        {
            TEndpoint.Map(app);
            return app; ;
        }
    }
}
