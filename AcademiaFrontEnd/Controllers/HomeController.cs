using AcademiaFrontEnd.Models;
using AcademiaFrontEnd.Request.client;
using AcademiaFrontEnd.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace AcademiaFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _userService;

        public HomeController(ApiService clientService)
        {
            _userService = clientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var jwtToken = HttpContext.Session.GetString("AuthToken");

            if (jwtToken == null)
            {
                return View();
            }
            else
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var role = token.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                if (role == "Professor")
                {
                    return RedirectToAction("profile", "professor");
                }
                else
                {
                    return RedirectToAction("profile", "client");
                }
            }


        }


        [HttpPost]
        public async Task<IActionResult> Login(ClientLoginRequest request)
        {
            var result = await _userService.LoginUser(request);

            if (result == null || !result.IsSuccess)
            {
                if (result.Message.Contains("senha", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("senha", result.Message); // Erro de senha incorreta
                }
                else
                {
                    ModelState.AddModelError("email", result.Message); // Erro de conta não encontrada
                }

                return View(request); // Retorna a View mantendo os dados inseridos
            }

            // Armazena o token na sessão
            string token = result.Data;
            HttpContext.Session.SetString("AuthToken", token);

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                return role == "Professor"
                    ? RedirectToAction("profile", "professor")
                    : RedirectToAction("profile", "client");
            }

            return RedirectToAction("AccessDenied", "Home");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewClientRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userService.RegisterUser(model);

            if (result == null || !result.IsSuccess)
            {
                if (result.Message.Contains("Email:"))
                {
                    // Realiza o split usando ":" como delimitador
                    var errorMessageParts = result.Message.Split(':');

                    // Adiciona o erro no ModelState, pegando apenas a parte da mensagem após o ":"
                    ModelState.AddModelError("Email", errorMessageParts.Length > 1 ? errorMessageParts[1] : "Erro no e-mail.");
                }
                else
                    ModelState.AddModelError("", "Erro ao cadastrar usuário. Tente novamente.");


                return View(model);
            }

            return RedirectToAction("Login");
        }



        [HttpGet]
        public IActionResult Logout()
        {
            // Limpa todos os dados da sessão
            HttpContext.Session.Clear();

            // Remove o cookie de sessão se necessário
            Response.Cookies.Delete(".AspNetCore.Session");

            return RedirectToAction("Login");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return RedirectToAction("Profile");

            }
            var request = new UpdateImageProfileRequest
            {
                image = image
            };
            var jwtToken = HttpContext.Session.GetString("AuthToken");

            var result = await _userService.UpdateImageProfile(request, jwtToken);


            if (result.IsSuccess)
            {
                return RedirectToAction("Profile", "Professor");

            }
            else
                return BadRequest();



        }
    }
}
