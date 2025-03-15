using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using Microsoft.AspNetCore.Identity.Data;

namespace AcademiaBackEnd.Services.Clients
{
    public interface IClientInterface
    {
        Task<Response<ClientDto>> EditProfileAsync(EditProfileRequest request);

        Task<Response<ClientDto>> GetProfileAsync(GetProfileRequest request);

        Task<Response<List<WorkoutsDto>>> GetAllWorkout();

        Task<Response<string>> Login(Request.Clients.LoginRequest request);

        Task<Response<WorkoutSessionDto>> StartWorkout(StartWorkoutRequest request);

        Task<bool> EndWorkout(EndWorkoutRequest request);

        Task<Response<List<WorkoutSessionDto>>> GetWorkoutSessions(GetWorkoutsSessionsRequest request);


        Task<Response<bool>> ApproveRequest(AcceptProfessorRequest request );



    }
}
