using AcademiaFrontEnd.Response;
using System.Text.Json;
using System.Text;
using AcademiaFrontEnd.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http.Headers;
using AcademiaFrontEnd.Request.client;
using AcademiaFrontEnd.Request.Professor;
using static System.Net.Mime.MediaTypeNames;

namespace AcademiaFrontEnd.Services
{
    public class ApiService : IApiInterface
    {
        private HttpClient _client;

        public ApiService(HttpClient client)
        {
            _client = client;

        }

        public async Task<Response<WorkoutModel>> GetClientWorkout(long idtreino, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);



            // Fazer a requisição GET
            var response = await _client.GetAsync($"v1/professor/GetClientWorkout/{idtreino}");

            if (response.IsSuccessStatusCode)
            {
                // Ler o conteúdo da resposta
                var responseData = await response.Content.ReadFromJsonAsync<Response<WorkoutModel>>();

                return new Response<WorkoutModel>(responseData.Data, 200, "Treino encontrado");
            }

            return new Response<WorkoutModel>(null, 400, "Erro ao localizar o treino");
        }

        public async Task<bool> EndWorkout(EndWorkoutRequest request, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("v1/finishworkout/", content);

            if (response.IsSuccessStatusCode)
            {

                return true;
            }

            return false;
        }

        public async Task<Response<ClientModel>> GetClientProfile(long id, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"v1/professor/findclient/{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<ClientModel>>();

                return new Response<ClientModel>(responseData.Data, 200, "Perfil carregado com sucesso");
            }
            else
            {
                return new Response<ClientModel>(null, 404, "Usuário não encontrado");
            }
        }

        public async Task<Response<ProfessorModel>> GetProfessorProfile(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("v1/professor/profile");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<ProfessorModel>>();

                return new Response<ProfessorModel>(responseData.Data, 200, "Perfil carregado com sucesso");
            }
            else
            {
                return new Response<ProfessorModel>(null, 404, "Usuário não encontrado");
            }
        }

        public async Task<Response<ClientModel>> GetProfile(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("v1/client/Profile");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<ClientModel>>();

                return new Response<ClientModel>(responseData.Data, 200, "Perfil carregado com sucesso");
            }
            else
            {
                return new Response<ClientModel>(null, 404, "Usuário não encontrado");
            }
        }


        public async Task<Response<string>> LoginUser(ClientLoginRequest request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("v1/Login", content);



                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<Response<string>>();

                    return new Response<string>(responseData.Data, 200, "Login efetuado com sucesso");


                }
                else 
                {
                    var responseData = await response.Content.ReadFromJsonAsync<Response<string>>();

                    return new Response<string>(null, 404, responseData.Message);

                }



            }
            catch (Exception ex)
            {
                return new Response<string>(null, 500, ex.Message);
            }
        }

        public async Task<Response<ClientModel>> RegisterUser(RegisterNewClientRequest request)
        {
            try
            {

                var jsonContent = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("v1/RegisterNewClient", content);

                if (response.IsSuccessStatusCode)

                {
                    return new Response<ClientModel>(null, 200, "Usuário criado com sucesso");
                }
                var responseData = await response.Content.ReadFromJsonAsync<Response<ClientModel>>();

                return new Response<ClientModel>(null, 400, responseData.Message);
            }
            catch (Exception ex)
            {
                return new Response<ClientModel>(null, 500, ex.Message);
            }
        }

        public async Task<Response<WorkoutModel>> RemoveWorkout(long idtreino, long idusuario, string token)
        {
            try
            {
                // Adiciona o token no cabeçalho da requisição
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Cria o objeto para enviar no corpo da requisição
                var requestBody = new
                {
                    WorkoutId = idtreino,
                    userclientId = idusuario
                };

                // Serializa o objeto para JSON
                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(requestBody),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                // Faz a requisição para o endpoint
                var response = await _client.PostAsync("v1/professor/removeworkout", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<Response<WorkoutModel>>();

                    return new Response<WorkoutModel>(responseData.Data, 200, "Treino removido com sucesso");
                }
                else
                {
                    return new Response<WorkoutModel>(null, 404, "Treino não encontrado");
                }
            }
            catch (Exception ex)
            {
                return new Response<WorkoutModel>(null, 500, ex.Message);
            }

        }


        public async Task<Response<WorkoutSessionModel>> StartWorkout(StartWorkoutRequest request, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("v1/workouts/start", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<WorkoutSessionModel>>();
                return new Response<WorkoutSessionModel>(responseData.Data, 200, "Treino iniciado");
            }

            return new Response<WorkoutSessionModel>(null, 400, "Erro ao Começar o treino");



        }

        public async Task<Response<WorkoutModel>> UpdateClientWorkout(UpdateClientWorkoutRequest request, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("v1/professor/updateworkout", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<WorkoutModel>>();
                return new Response<WorkoutModel>(responseData.Data, 200, "Treino Atualizado");
            }

            return new Response<WorkoutModel>(null, 400, "Erro ao atualizar o treino");

        }

        public async Task<Response<List<ExerciseModel>>> GetAllExercise(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("v1/professor/exercises");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<List<ExerciseModel>>>();
                return new Response<List<ExerciseModel>>(responseData.Data, 200, "Exercicios localizados");
            }

            return new Response<List<ExerciseModel>>(null, 400, "Erro ao encontrar os exercicios");
        }

        public async Task<Response<WorkoutModel>> CreateNewWorkout(CreateNewWorkoutRequest content, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var jsonContent = JsonSerializer.Serialize(content);
            var request = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("v1/professor/newWorkout", request);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<WorkoutModel>>();
                return new Response<WorkoutModel>(responseData.Data, 200, "Treino Criado");
            }

            return new Response<WorkoutModel>(null, 400, "Erro ao criar o treino");
        }

        public async Task<Response<bool>> UpdateImageProfile(UpdateImageProfileRequest request, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            using var content = new MultipartFormDataContent();
            using var fileStream = request.image.OpenReadStream();

            content.Add(new StreamContent(fileStream), "image", request.image.FileName);


            var response = await _client.PostAsync("v1/client/updateimage", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<bool>>();

                return new Response<bool>(true, 200, "Imagem atualizada com sucesso");
            }


            return new Response<bool>(true, 400, "Erro ao enviar imagem");

        }

        public async Task<Response<List<ClientModel>>> GetAllClients(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("v1/professor/clients");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<List<ClientModel>>>();

                return new Response<List<ClientModel>>(responseData.Data, 200, "Usuarios carregados");
            }


            return new Response<List<ClientModel>>(null, 400, "Erro ao buscar usuarios");


        }

        public async Task<Response<SolicitacaoModel>> AddNewClient(AddNewClientRequest request, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("v1/professor/AddNewClient", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<SolicitacaoModel>>();

                return new Response<SolicitacaoModel>(responseData.Data, 200, "Soliciatação enviada com sucesso.");
            }


            return new Response<SolicitacaoModel>(null, 400, "Erro ao Enviar solicitação.");

        }

        public async Task<Response<bool>> ApproveRequest(AcceptProfessorRequest request, string jwtToken)
        {
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            var response = await _client.PostAsync("v1/client/profile/acceptrequest", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<bool>>();

                return new Response<bool>(true, 200, "Relacionamento criado com sucesso.");
            }


            return new Response<bool>(false, 400, "Erro ao criar relacionamento.");

        }
    }
}
