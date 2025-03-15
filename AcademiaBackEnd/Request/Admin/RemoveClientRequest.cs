using System.ComponentModel.DataAnnotations;

namespace AcademiaBackEnd.Request.Clients
{
    public class RemoveClientRequest
    {
        [Required]
        public long Id { get; set; }
    }
}
