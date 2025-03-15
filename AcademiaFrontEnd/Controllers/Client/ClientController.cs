using AcademiaFrontEnd.Models;
using AcademiaFrontEnd.Request;
using AcademiaFrontEnd.Request.client;
using AcademiaFrontEnd.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaFrontEnd.Controllers.Client
{
    public class ClientController : Controller
    {
        private readonly ApiService _userService;

        public ClientController(ApiService clientService)
        {
            _userService = clientService;
        }
        public async Task<IActionResult> ProfileAsync()
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");
            var result = await _userService.GetProfile(jwtToken);

            if (result.IsSuccess)
            {
                return View(result);

            }
            return RedirectToAction("Login","home");

        }
        public async Task<IActionResult> StartWorkout(long id)
        {
            var request = new StartWorkoutRequest
            {
                WorkoutId = id
            };
            var jwtToken = HttpContext.Session.GetString("AuthToken");

            var result = await _userService.StartWorkout(request, jwtToken);

            if (result.IsSuccess)
            {
                return View("Start", result);

            }
            else
            {
                TempData["Message"] = "Você já está em um treino.";
                return View("Start", result);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EndWorkout(long id)
        {
            var request = new EndWorkoutRequest
            {
                WorkoutId = id
            };
            var jwtToken = HttpContext.Session.GetString("AuthToken");

            var result = await _userService.EndWorkout(request, jwtToken);

            if (result == true)
            {
                return RedirectToAction("Profile");

            }
            else
                return BadRequest();

        }


        public async Task<IActionResult> ApproveRequest(AcceptProfessorRequest request)
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");

            var result = await _userService.ApproveRequest(request, jwtToken);

            if (result.IsSuccess)
            {
                return RedirectToAction("profile");

            }
            else
            {
                TempData["Message"] = "Erro ao aceitar solicitação.";
                return RedirectToAction("profile", result);
            }

        }

    }
}
