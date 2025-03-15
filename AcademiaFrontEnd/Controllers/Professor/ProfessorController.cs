using AcademiaFrontEnd.Models;
using AcademiaFrontEnd.Request.Professor;
using AcademiaFrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AcademiaFrontEnd.Controllers.Professor
{
    public class ProfessorController : Controller
    {
        private readonly ApiService _professorService;

        public ProfessorController(ApiService clientService)
        {
            _professorService = clientService;
        }
        public async Task<IActionResult> ProfileAsync()
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");
            var result = await _professorService.GetProfessorProfile(jwtToken);

            if (result.IsSuccess)
            {
                return View(result);

            }
            return RedirectToAction("Login", "home");

        }

        public async Task<IActionResult> GetProfileClient(long id)
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");
            var result = await _professorService.GetClientProfile(id, jwtToken);

            if (result.IsSuccess)
            {
                return View("ClientProfile", result);

            }
            return RedirectToAction("Login", "home");

        }
        [HttpGet]
        public async Task<IActionResult> RemoveWorkout(long idtreino, long idusuario)
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");
            var result = await _professorService.RemoveWorkout(idtreino, idusuario, jwtToken);

            if (result.IsSuccess)
            {
                return RedirectToAction("GetProfileClient", new { id = idusuario });
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> EditClientWorkout(long idtreino, long idusuario)
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");

            var result = await _professorService.GetClientWorkout(idtreino, jwtToken);
            ViewBag.Idusuario = idusuario;

            if (result.IsSuccess)
            {
                return View("EditClientWorkout", result.Data);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> EditClientWorkout(WorkoutModel treino, long idusuario)
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");

            var request = new UpdateClientWorkoutRequest
            {
                workoutDto = treino
            };

            var result = await _professorService.UpdateClientWorkout(request, jwtToken);

            if (result.IsSuccess)
            {
                return RedirectToAction("GetProfileClient", new { id = idusuario });
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> CreateNewWorkout(long idusuario)
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");



            var result = await _professorService.GetAllExercise(jwtToken);

            if (result.IsSuccess)
            {
                ViewBag.idusuario = idusuario;
                ViewBag.Exercicios = result.Data;
                return View();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewWorkout(WorkoutModel treino, long idusuario)
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");

            var request = new CreateNewWorkoutRequest
            {
                idusuario = idusuario,
                Nome = treino.Nome,
                Descricao = treino.Descricao,
                Exercicios = treino.Exercicios.Select(e => new ExerciseModel
                {
                    Nome = e.Nome,
                    Id = e.Id,
                    Sets = e.Sets,
                    Reps = e.Reps,
                    Carga = e.Carga
                }).ToList()
            };

            var result = await _professorService.CreateNewWorkout(request, jwtToken);

            if (result.IsSuccess)
            {
                return RedirectToAction("GetProfileClient", new { id = idusuario });

            }

            return BadRequest();
        }


        [HttpGet]
        public async Task<IActionResult> AddNewClient()
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");
            
            var result = await _professorService.GetAllClients(jwtToken);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }
            else
                return View(result);
        }
       

        [HttpPost]
        public async Task<IActionResult> AddNewClient(AddNewClientRequest request)
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");

          

            var result = await _professorService.AddNewClient(request, jwtToken);

            if (!result.IsSuccess)
            {
                return BadRequest();
            }
            else
                return RedirectToAction("profile");
        }
       
    }
}
