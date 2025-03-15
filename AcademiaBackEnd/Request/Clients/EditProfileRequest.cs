using System.ComponentModel.DataAnnotations;

namespace AcademiaBackEnd.Request.Clients
{
    public class EditProfileRequest : Request
    {
  
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public double Weight { get; set; }

        public double Height { get; set; }
    }
}
