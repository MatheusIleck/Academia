using System.Text.Json.Serialization;

namespace AcademiaBackEnd.Request.Clients
{
    public class EndWorkoutRequest : Request
    {
        public DateTime finishDate { get; set; } = DateTime.Now;


    }
}
