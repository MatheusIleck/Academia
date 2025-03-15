using AcademiaFrontEnd.Models;
using AcademiaFrontEnd.Request.client;
using AcademiaFrontEnd.Request.Professor;
using AcademiaFrontEnd.Response;
using Microsoft.Extensions.Primitives;

namespace AcademiaFrontEnd.Services
{
    public interface IApiInterface
    {

        //client
        Task<Response<string>> LoginUser(ClientLoginRequest request);
        Task<Response<ClientModel>> GetProfile(string token);
        Task<Response<ClientModel>> RegisterUser(RegisterNewClientRequest request);
        Task<Response<WorkoutSessionModel>> StartWorkout(StartWorkoutRequest request, string token);
        Task<bool> EndWorkout(EndWorkoutRequest request, string token);
        Task<Response<bool>> UpdateImageProfile(UpdateImageProfileRequest request, string jwtToken);
        Task<Response<bool>> ApproveRequest(AcceptProfessorRequest request, string jwtToken);



        //professor
        Task<Response<ProfessorModel>> GetProfessorProfile(string token);
        Task<Response<ClientModel>> GetClientProfile(long id, string token);

        Task<Response<WorkoutModel>> RemoveWorkout(long idtreino,long idusuario, string token);
        Task<Response<WorkoutModel>> GetClientWorkout(long idtreino, string token);
        Task<Response<WorkoutModel>> UpdateClientWorkout(UpdateClientWorkoutRequest request,string token);
        Task<Response<WorkoutModel>> CreateNewWorkout(CreateNewWorkoutRequest treino,string token);

        Task<Response<List<ExerciseModel>>>GetAllExercise(string token);
        Task<Response<List<ClientModel>>> GetAllClients(string token);

        Task<Response<SolicitacaoModel>>AddNewClient(AddNewClientRequest request, string token);





    }
}

