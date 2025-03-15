using System.Text.Json.Serialization;

namespace AcademiaBackEnd.Request.Clients
{
    public class StartWorkoutRequest : Request
    {
        public DateTime startDate { get; set; } = DateTime.Now;
        public long WorkoutId { get; set; }
    }
}
