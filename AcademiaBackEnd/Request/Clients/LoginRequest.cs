using System.ComponentModel.DataAnnotations;

namespace AcademiaBackEnd.Request.Clients
{
    public class LoginRequest
    {
        [Required]
        public string email { get; set; } = null!;

        [Required]
        public string senha { get; set; } = null!;
    }
}
