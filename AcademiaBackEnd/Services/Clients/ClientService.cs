using AcademiaBackEnd.Data.Models;
using AcademiaBackEnd.Dto;
using AcademiaBackEnd.Request.Clients;
using AcademiaBackEnd.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AcademiaBackEnd.Request.Professor;

namespace AcademiaBackEnd.Services.Clients
{
    public class Clientservice : IClientInterface
    {
        private readonly db_AcademiaContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Clientservice(db_AcademiaContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<ClientDto>> EditProfileAsync(EditProfileRequest request)
        {
            try
            {

                var client = await _db.Usuarios.FirstOrDefaultAsync(x => x.Id == request.ClientId);

                if (client == null)
                {
                    return new Response<ClientDto>(null, 404, "Cliente não encontrado.");
                }
                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    client.Nome = request.Name;
                }
                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    client.Email = request.Email;
                }
                if (request.Height != 0)
                {
                    client.Altura = request.Height;
                }
                if (request.Weight != 0)
                {
                    client.Peso = request.Weight;
                }

                _db.Update(client);
                await _db.SaveChangesAsync();

                ClientDto clientDto = new ClientDto
                {

                    Nome = client.Nome,
                    Email = client.Email,
                    Altura = client.Altura,
                    Peso = client.Peso,
                };

                return new Response<ClientDto>(clientDto, 200, "Os dados foram atualizados com sucesso!");
            }
            catch (Exception ex)
            {
                return new Response<ClientDto>(null, 500, ex.Message);

            }



        }

        public async Task<Response<List<WorkoutsDto>>> GetAllWorkout()
        {
            try
            {

                var workoutDtos = await _db.Treinos
                .AsNoTracking()
                   .Select(w => new WorkoutsDto
                   {
                       Id = w.Id,
                       Nome = w.Nome,
                       Exercicios = w.TreinoExercicios
                           .Select(we => new ExerciseDto
                           {
                               Id = we.IdExercicioNavigation.Id,
                               Nome = we.IdExercicioNavigation.Nome,
                               sets = we.Sets,
                               reps = we.Reps,
                               carga = we.Carga,


                           }).ToList()
                   })
                   .ToListAsync();



                if (workoutDtos == null)
                {
                    return new Response<List<WorkoutsDto>>(null, 404, "Você não possui treinos no momento.");
                }

                return new Response<List<WorkoutsDto>>(workoutDtos, 200, "Treinos encontrados.");

            }
            catch (Exception ex)
            {
                return new Response<List<WorkoutsDto>>(null, 500, ex.Message);
            }
        }

        public async Task<Response<ClientDto>> GetProfileAsync(GetProfileRequest request)
        {
            DateOnly hoje = DateOnly.FromDateTime(DateTime.Now);
            try
            {
                ClientDto clientDto = await _db.Usuarios
                .Include(s => s.SessaoTreinos)
                .Include(sc => sc.SolicitacoClientes)
                .Include(c => c.Treinos)
                .ThenInclude(w => w.TreinoExercicios)
                .Where(x => x.Id == request.ClientId)
                .Select(c => new ClientDto
                {
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



                    }).ToList(),
                    solicitacoes = c.SolicitacoClientes.Where(s => s.Status == "Pendente").Select(sd => new SolicitacaoDto
                    {
                        Id = sd.Id,
                        ProfessorId = sd.ProfessorId,
                        NomeProfessor = sd.Professor.Nome
                    }).ToList()
                }).FirstOrDefaultAsync();

                var workoutssession = await _db.SessaoTreinos.Where(x => x.ClienteId == request.ClientId).ToListAsync();

                TimeSpan totalDuration = TimeSpan.Zero;



                clientDto.Horastotais = TimeSpan.FromHours(totalDuration.TotalHours);

                var teste = ContarDiasConsecutivos(request.ClientId);
                clientDto.ofensiva = teste;

                if (clientDto == null)
                {
                    return new Response<ClientDto>(null, 404, "Erro ao acessar o perfil.");
                }



                return new Response<ClientDto>(clientDto, 200, "Perfil carregado com sucesso.");

            }
            catch (Exception ex)
            {
                return new Response<ClientDto>(null, 500, ex.Message);
            }
        }

        public async Task<Response<string>> Login(LoginRequest request)
        {
            try
            {
                var client = await _db.Usuarios.FirstOrDefaultAsync(x => x.Email == request.email.ToLower());

                if (client == null)
                {
                    return new Response<string>(null, 404, "Desculpe, mas não encontramos sua conta. ");

                }

                if (client.Senha == request.senha)
                {
                    string token = GenerateToken(client);


                    return new Response<string>(token, 200);
                }
                else
                {
                    return new Response<string>(null, 400, "Senha incorreta.");

                }


            }
            catch (Exception ex)
            {
                return new Response<string>(null, 500, ex.Message);
            }
        }

        public async Task<Response<WorkoutSessionDto>> StartWorkout(StartWorkoutRequest request)
        {
            try
            {
                // Checa pra ver se existe alguma sessão de treino ativa
                var workoutActive = await GetActiveWorkoutSessionAsync(request.ClientId);
                if (workoutActive != null)
                {
                    return new Response<WorkoutSessionDto>(workoutActive, 200, "Você possui um treino ativo.");
                }

                // Cria uma nova sessão de treino
                var workoutSession = new SessaoTreino
                {
                    TreinoId = request.WorkoutId,
                    HoraInicio = request.startDate,
                    ClienteId = request.ClientId,
                    StatusTreino = "Em andamento"
                };

                _db.SessaoTreinos.Add(workoutSession);
                await _db.SaveChangesAsync();

                // Recupera e cria uma sessão de treino a partir do Id do usuario
                var newWorkoutSession = await GetActiveWorkoutSessionAsync(request.ClientId);
                return new Response<WorkoutSessionDto>(newWorkoutSession, 200, "Treino iniciado!");
            }
            catch (Exception ex)
            {
                return new Response<WorkoutSessionDto>(null, 500, ex.Message);
            }
        }

        public async Task<bool> EndWorkout(EndWorkoutRequest request)
        {
            try
            {
                var finishedWorkout = await _db.SessaoTreinos
                                              .Where(x => x.StatusTreino == "Em Andamento" && x.ClienteId == request.ClientId)
                                              .FirstOrDefaultAsync();

                if (finishedWorkout == null)
                {
                    throw new InvalidOperationException("Você não possui nenhum treino iniciado.");
                }

                finishedWorkout.HoraFim = request.finishDate;
                finishedWorkout.StatusTreino = "Finalizado";



                _db.Update(finishedWorkout);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                return false;

            }
        }


        public async Task<Response<List<WorkoutSessionDto>>> GetWorkoutSessions(GetWorkoutsSessionsRequest request)
        {
            try
            {
                var workoutSessions = await _db.SessaoTreinos
                 .Where(x => x.ClienteId == request.ClientId)
                 .Select(ws => new WorkoutSessionDto
                 {
                     Id = ws.Id,
                     Inicio = ws.HoraInicio.ToString("H:m:ss"),
                     Fim = ws.HoraFim.HasValue ? ws.HoraFim.Value.ToString("H:m:ss") : null,

                 })
                 .ToListAsync();

                if (workoutSessions == null)
                {
                    return new Response<List<WorkoutSessionDto>>(null, 404, "Nenhum historico encontrado.");
                }




                return new Response<List<WorkoutSessionDto>>(workoutSessions, 200, "Historico encontrado.");

            }
            catch (Exception ex)
            {
                return new Response<List<WorkoutSessionDto>>(null, 500, ex.Message);
            }
        }


        // Metodo para verificar se existe uma sessão ativa
        private async Task<WorkoutSessionDto> GetActiveWorkoutSessionAsync(long clientId)
        {
            var activesession = await _db.SessaoTreinos
      .Where(ws => ws.HoraFim == null && ws.ClienteId == clientId)
      .FirstOrDefaultAsync();

            // Verifica se existe uma sessão ativa
            if (activesession == null)
            {
                return null;
            }

            var workout = await _db.Treinos
      .Where(x => x.Id == activesession.TreinoId)
      .Include(t => t.TreinoExercicios) // Inclui os exercícios associados
          .ThenInclude(we => we.IdExercicioNavigation) // Inclui o exercício em si
      .FirstOrDefaultAsync();

            if (workout == null)
            {
                throw new Exception("O treino associado não foi encontrado.");
            }

            // Mapeia os exercícios do treino para a DTO
            var exerciciosDto = workout.TreinoExercicios
                .Select(we => new ExerciseDto
                {
                    Id = we.Id,
                    reps = we.Reps,
                    sets = we.Sets,
                    carga = we.Carga,
                    Video = we.IdExercicioNavigation.Video
                })
                .ToList();

            // Cria o DTO da sessão ativa
            WorkoutSessionDto activesessiondto = new WorkoutSessionDto
            {
                Id = activesession.Id,
                Nome = workout.Nome,
                Inicio = activesession.HoraInicio.ToString("HH:mm:ss"),
                Fim = activesession.HoraFim?.ToString("yyyy-MM-dd HH:mm:ss"), // Formata ou deixa nulo
                Treino = new WorkoutsDto
                {
                    Id = workout.Id,
                    Descricao = workout.Descricao,
                    Exercicios = exerciciosDto // Atribui a lista de exercícios
                }
            };

            return activesessiondto;
        }

        private int ContarDiasConsecutivos(long clienteId)
        {
            // Obter todas as sessões de treino do cliente, ordenadas pela data de início
            var sessoes = _db.SessaoTreinos
                                  .Where(s => s.ClienteId == clienteId && s.StatusTreino == "Finalizado")
                                  .OrderBy(s => s.HoraInicio)
                                  .ToList();

            if (!sessoes.Any())
                return 0; // Caso não haja nenhuma sessão de treino, retorna 0

            int diasConsecutivos = 1;
            DateOnly dataAnterior = DateOnly.FromDateTime(sessoes[0].HoraInicio);

            for (int i = 1; i < sessoes.Count; i++)
            {
                DateOnly dataAtual = DateOnly.FromDateTime(sessoes[i].HoraInicio);
                // Verifica se a data atual é o dia seguinte à anterior
                if (dataAtual == dataAnterior.AddDays(1))
                {
                    diasConsecutivos++;
                }
                else
                {
                    // Se houver uma falha no dia consecutivo, reinicia o contador
                    break;
                }
                dataAnterior = dataAtual;
            }

            return diasConsecutivos;
        }

        //token
        public static string GenerateToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var chaveSecreta = Environment.GetEnvironmentVariable("chaveSecreta");



            var key = Encoding.ASCII.GetBytes(chaveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "sua_empresa",
                Audience = "sua_aplicacao",
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Nome.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("UserId", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<Response<bool>> ApproveRequest(AcceptProfessorRequest request)
        {
            try
            {
                var solicitacao = await _db.Solicitacoes.FirstOrDefaultAsync(x => x.Id == request.idSolicitacao && x.ProfessorId == request.idProfessor);

                solicitacao.Status = "Aceito";

                _db.Update(solicitacao);

                var novoRelacionamento = new Relacionamento
                {
                    AlunoId = solicitacao.ClienteId,
                    ProfessorId = solicitacao.ProfessorId,
                    DataConfirmacao = DateTime.Now
                };

                _db.Relacionamentos.Add(novoRelacionamento);
                await _db.SaveChangesAsync();

                return new Response<bool>(true, 200, "Relacionamento criado com sucesso!");


            }
            catch (Exception ex)
            {
                return new Response<bool>(false, 500, ex.Message);

            }
        }
    }
}
