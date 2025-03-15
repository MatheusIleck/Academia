using System.ComponentModel.DataAnnotations;

namespace AcademiaFrontEnd.Request.client
{
     public class RegisterNewClientRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [RegularExpression(@"^[a-zA-Z]+( [a-zA-Z]+)*$", ErrorMessage = "O nome só pode conter letras e um único espaço entre as palavras.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        [MaxLength(12, ErrorMessage = "A senha deve ter no máximo 12 caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}[\]:;<>,.?~\\/-]).{6,12}$",
            ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, um número e um caractere especial.")]
        public string Password { get; set; }


        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(RegisterNewClientRequest), nameof(ValidateBirthDate))]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "O peso é obrigatório.")]
        [Range(30, 300, ErrorMessage = "O peso deve estar entre 30kg e 300kg.")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "A altura é obrigatória.")]
        [Range(1.0, 2.5, ErrorMessage = "A altura deve estar entre 1,00m e 2,50m.")]
        public decimal Height { get; set; }

        // Validação personalizada para Data de Nascimento
        public static ValidationResult ValidateBirthDate(DateOnly date, ValidationContext context)
        {
            DateTime dateTime = DateTime.Now;
            if (date.Year >= dateTime.Year - 5)
            {
                return new ValidationResult("A data de nascimento invalida.");
            }
            return ValidationResult.Success;
        }
    }

}
