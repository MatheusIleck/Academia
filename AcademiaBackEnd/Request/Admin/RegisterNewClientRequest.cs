using System.ComponentModel.DataAnnotations;

namespace AcademiaBackEnd.Request.Clients
{
    public class RegisterNewClientRequest 

    {
        [Required]
        public long Id { get; set; }

       

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public double Height { get; set; }

    }
}
