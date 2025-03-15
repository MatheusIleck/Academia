using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Response;

namespace AcademiaBackEnd.Services.Professor
{
    public interface IProfessorInterface
    {

        public Task<Response<ProfessorDto>> GetProfessorProfileAsync(GetProfessorProfileRequest request);

        public Task<Response<List<ClientDto>>> GetClientsAsync(GetAllClientsRequest request);



        public Task<Response<ClientDto>> GetClientProfileAsync(GetClientProfileRequest request);
        public Task<Response<Treino>> RemoveWorkout(RemoveWorkoutRequest request);

        public Task<Response<WorkoutsDto>> GetClientWorkout(long idtreino);

        public Task<Response<WorkoutsDto>> UpdateClientWorkout(UpdateClientWorkoutRequest request);

        public Task<Response<WorkoutsDto>>CreateNewWorkout(CreateNewWorkoutRequest request);

        public Task<Response<List<ExerciseDto>>> GetAllExercise();

        public Task<Response<Solicitaco>>AddNewClientAsync(AddNewClientRequest request);


    }
}
