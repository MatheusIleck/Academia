using System.ComponentModel.DataAnnotations;

namespace AcademiaFrontEnd.Request.client
{
    public class ClientLoginRequest
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]

        public string email { get; set; }

        [Required(ErrorMessage = "Senha Incorreta!")]

        public string senha { get; set; }
    }
}
