using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Request.Professor;
using AcademiaBackEnd.Response;
using Microsoft.EntityFrameworkCore;

namespace AcademiaBackEnd.Services.Professor
{
    public class ProfessorService(db_AcademiaContext _db) : IProfessorInterface
    {


        public async Task<Response<List<ClientDto>>> GetClientsAsync(GetAllClientsRequest request)
        {
            try
            {
                var Clients = await _db.Usuarios
        .Select(dt => new ClientDto
        {
            id = dt.Id,
            Nome = dt.Nome,
            Email = dt.Email,
            Peso = dt.Peso,
            Altura = dt.Altura,
            imgperfil = dt.Img ?? "default.png"
        })
        .ToListAsync();

                // Verificar se a imagem existe no servidor
                foreach (var client in Clients)
                {
                    string caminhoImagem = "wwwroot" + client.imgperfil;

                    if (!File.Exists(caminhoImagem))
                    {
                        client.imgperfil = "/imgs/default.png"; // Se não existir, define uma imagem padrão
                    }
                }


                if (Clients == null)
                {
                    return new Response<List<ClientDto>>(null, 404, "Você não possui alunos.");
                }

                return new Response<List<ClientDto>>(Clients, 200, "Alunos encontrados");

            }
            catch (Exception ex)
            {
                return new Response<List<ClientDto>>(null, 500, ex.Message);
            }

        }


        public async Task<Response<ClientDto>> GetClientProfileAsync(GetClientProfileRequest request)
        {
            try
            {
                DateOnly hoje = DateOnly.FromDateTime(DateTime.Now);
                ClientDto clientDto = await _db.Usuarios
                .Include(s => s.SessaoTreinos)
                .Include(c => c.Treinos)
                .ThenInclude(w => w.TreinoExercicios)
                .Where(x => x.Id == request.userId)
                .Select(c => new ClientDto
                {
                    id = c.Id,
                    imgperfil = c.Img,
                    Nome = c.Nome,
                    Email = c.Email,
                    Peso = c.Peso,
                    idade = hoje.Year - c.DataNascimento.Year,
                    Altura = c.Altura,
                    Imc = c.Peso.HasValue && c.Altura.HasValue
                           ? c.Peso.Value / (c.Altura.Value * c.Altura.Value)
                           : (double?)null,
                    Workouts = c.Treinos.Select(w => new WorkoutsDto
                    {
                        Id = w.Id,
                        Nome = w.Nome,
                        Descricao = w.Descricao,
                        Exercicios = w.TreinoExercicios.Select(e => new ExerciseDto
                        {
                            Id = e.Id,
                            sets = e.Sets,
                            reps = e.Reps,
                            carga = e.Carga
                        }).ToList()
                    }).ToList(),
                    WorkoutsSessions = c.SessaoTreinos
                        .Where(ws => ws.HoraInicio.Month == DateTime.Now.Month && ws.HoraInicio.Year == DateTime.Now.Year) // Filtro pelo mês e ano atual.
                    .Select(ws => new WorkoutSessionDto
                    {
                        Id = ws.Id,
                        Inicio = ws.HoraInicio.ToString("H:m:ss"),
                        Fim = ws.HoraFim.HasValue ? ws.HoraFim.Value.ToString("H:m:ss") : null,



                    }).ToList(),
                }).FirstOrDefaultAsync();





                if (clientDto == null)
                {
                    return new Response<ClientDto>(null, 404, "Aluno não encontrado.");

                }

                return new Response<ClientDto>(clientDto, 200, "Aluno encontrado");

            }
            catch (Exception ex)
            {
                return new Response<ClientDto>(null, 500, ex.Message);
            }
        }

        public async Task<Response<ProfessorDto>> GetProfessorProfileAsync(GetProfessorProfileRequest request)
        {
            try
            {
                var hoje = DateTime.Today;

                var professorDto = await _db.Usuarios
                    .Where(x => x.Id == request.ClientId)
                    .Select(c => new ProfessorDto
                    {
                        imgperfil = c.Img,
                        Nome = c.Nome,
                        Email = c.Email,
                        Peso = c.Peso,
                        Altura = c.Altura,
                        idade = hoje.Year - c.DataNascimento.Year,
                        Imc = c.Peso.HasValue && c.Altura.HasValue && c.Altura.Value != 0
                            ? c.Peso.Value / (c.Altura.Value * c.Altura.Value)
                            : (double?)null,

                        alunos = _db.Relacionamentos
                            .Where(r => r.ProfessorId == c.Id)
                            .Select(r => new ClientDto
                            {
                                id = r.Aluno.Id,
                                imgperfil = r.Aluno.Img,
                                Nome = r.Aluno.Nome,
                            }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (professorDto == null)
                {
                    return new Response<ProfessorDto>(null, 400, "Falha ao carregar o perfil");
                }

                // Verificar se as imagens dos alunos existem
                foreach (var client in professorDto.alunos)
                {
                    string caminhoImagem = "wwwroot" + client.imgperfil;
                    if (!File.Exists(caminhoImagem))
                    {
                        client.imgperfil = "/imgs/default.png"; // Define imagem padrão se não existir
                    }
                }

                return new Response<ProfessorDto>(professorDto, 200, "Perfil carregado com sucesso");
            }
            catch (Exception ex)
            {
                return new Response<ProfessorDto>(null, 500, "Erro ao carregar perfil: " + ex.Message);
            }
        }


        public async Task<Response<Treino>> RemoveWorkout(RemoveWorkoutRequest request)
        {
            var Workout = await _db.Treinos.FirstOrDefaultAsync(x => x.Id == request.workoutId && x.IdUsuario == request.userclientId);

            if (Workout == null)
            {
                return new Response<Treino>(null, 500, "Treino não encontrado");
            }

            _db.Update(Workout);
            await _db.SaveChangesAsync();

            return new Response<Treino>(Workout, 200, "Treino removido com sucesso!");

        }

        public async Task<Response<WorkoutsDto>> GetClientWorkout(long idtreino)
        {

            var Workout = await _db.Treinos.Where(x => x.Id == idtreino)
     .Include(wk => wk.TreinoExercicios)
     .Select(w => new WorkoutsDto
     {
         Id = w.Id,
         Nome = w.Nome,
         Descricao = w.Descricao,
         Exercicios = w.TreinoExercicios.Select(we => new ExerciseDto
         {
             Id = we.Id,
             Nome = we.IdExercicioNavigation.Nome,
             sets = we.Sets,
             reps = we.Reps,
             carga = we.Carga,
             Video = we.IdExercicioNavigation.Video
         }).ToList()
     })
     .FirstOrDefaultAsync();


            if (Workout == null)
            {
                return new Response<WorkoutsDto>(null, 500, "Treino não encontrado");
            }

            return new Response<WorkoutsDto>(Workout, 200, "Treino encontrado");
        }

        public async Task<Response<WorkoutsDto>> UpdateClientWorkout(UpdateClientWorkoutRequest request)
        {
            try
            {
                // Busca a entidade Workout no banco de dados
                var workout = await _db.Treinos
                    .Where(x => x.Id == request.workoutDto.Id)
                    .Include(w => w.TreinoExercicios) // Inclui os exercícios relacionados
                    .FirstOrDefaultAsync();

                if (workout == null)
                {
                    return new Response<WorkoutsDto>(null, 404, "Treino não encontrado.");
                }

                // Atualiza as propriedades da entidade Workout com os dados do DTO
                workout.Nome = request.workoutDto.Nome;
                workout.Descricao = request.workoutDto.Descricao;

                // Atualiza os exercícios do workout
                foreach (var exerciseDto in request.workoutDto.Exercicios)
                {
                    var workoutExercise = workout.TreinoExercicios
                        .FirstOrDefault(we => we.Id == exerciseDto.Id);

                    if (workoutExercise != null)
                    {
                        workoutExercise.Sets = exerciseDto.sets;
                        workoutExercise.Reps = exerciseDto.reps;
                        workoutExercise.Carga = exerciseDto.carga;
                    }
                }

                // Salva as alterações no banco de dados
                _db.Treinos.Update(workout);
                await _db.SaveChangesAsync();

                // Mapeia a entidade Workout atualizada para o DTO
                var updatedWorkoutDto = new WorkoutsDto
                {
                    Id = workout.Id,
                    Nome = workout.Nome,
                    Descricao = workout.Descricao,
                    Exercicios = workout.TreinoExercicios.Select(we => new ExerciseDto
                    {
                        Id = we.Id,
                        Nome = we.IdTreinoNavigation.Nome,
                        sets = we.Sets,
                        reps = we.Reps,
                        carga = we.Carga,
                    }).ToList()
                };

                return new Response<WorkoutsDto>(updatedWorkoutDto, 200, "Treino atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return new Response<WorkoutsDto>(null, 500, ex.Message);
            }
        }

        public async Task<Response<WorkoutsDto>> CreateNewWorkout(CreateNewWorkoutRequest request)
        {
            try
            {
                // Cria o novo treino
                var newWorkout = new Treino
                {
                    Nome = request.Nome,
                    Descricao = request.Descricao,
                    IdUsuario = request.idusuario
                };

                // Adiciona o treino ao banco de dados
                await _db.Treinos.AddAsync(newWorkout);
                await _db.SaveChangesAsync(); // Salva para obter o ID do treino

                // Adiciona os exercícios ao treino
                foreach (var exerciseRequest in request.Exercicios)
                {
                    // Cria a associação entre o treino e o exercício
                    var workoutExercise = new TreinoExercicio
                    {
                        IdTreino = newWorkout.Id, // ID do treino
                        IdExercicio = exerciseRequest.Id, // ID do exercício
                        Carga = exerciseRequest.Carga, // Carga do exercício
                        Reps = exerciseRequest.Reps, // Repetições
                        Sets = exerciseRequest.Sets // Séries
                    };
                    await _db.TreinoExercicios.AddAsync(workoutExercise);

                    newWorkout.TreinoExercicios.Add(workoutExercise);
                }

                // Salva os exercícios no banco de dados
                await _db.SaveChangesAsync();

                // Carrega os exercícios relacionados usando Eager Loading
                var workoutExercises = await _db.TreinoExercicios
                    .Include(we => we.IdExercicioNavigation) // Carrega a entidade relacionada Exercise
                    .Where(we => we.IdTreino == newWorkout.Id)
                    .ToListAsync();

                // Cria o DTO de resposta
                var workoutDto = new WorkoutsDto
                {
                    Nome = newWorkout.Nome,
                    Descricao = newWorkout.Descricao,
                    Exercicios = workoutExercises
                        .Select(we => new ExerciseDto
                        {
                            Id = we.IdExercicio,
                            Nome = we.IdExercicioNavigation.Nome, // Acessa o nome do exercício
                            carga = we.Carga,
                            reps = we.Reps,
                            sets = we.Sets,
                        }).ToList()
                };

                return new Response<WorkoutsDto>(workoutDto, 200, "Treino criado.");
            }
            catch (Exception ex)
            {
                return new Response<WorkoutsDto>(null, 500, ex.Message);
            }
        }

        public async Task<Response<List<ExerciseDto>>> GetAllExercise()
        {
            try
            {
                var exercicios = await _db.Exercicios.Select(ed => new ExerciseDto
                {
                    Id = ed.Id,
                    Nome = ed.Nome,

                }).ToListAsync();
                if (exercicios == null)
                {
                    return new Response<List<ExerciseDto>>(null, 400, "Exercicios não foram encontrados");

                }

                return new Response<List<ExerciseDto>>(exercicios, 200, "Exercicios encontrados");
            }
            catch (Exception ex)
            {
                {
                    return new Response<List<ExerciseDto>>(null, 500, ex.Message);
                }
            }
        }

        public async Task<Response<Solicitaco>> AddNewClientAsync(AddNewClientRequest request)
        {
            try
            {
                var novaSolicitacao = new Solicitaco
                {
                    ClienteId = request.idAluno,
                    ProfessorId = request.professorId,
                    Status = "Pendente"
                };

                if (novaSolicitacao == null)
                {
                    return new Response<Solicitaco>(null, 400, "Não foi possivel criar uma nova solicitação");

                }

                _db.Solicitacoes.Add(novaSolicitacao);
                await _db.SaveChangesAsync();

                return new Response<Solicitaco>(novaSolicitacao, 200, "Criado com sucesso");

            }
            catch (Exception ex)
            {
                return new Response<Solicitaco>(null, 500, ex.Message);
            }
        }
    }
}
