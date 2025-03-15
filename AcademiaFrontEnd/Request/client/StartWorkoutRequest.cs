using AcademiaFrontEnd.Models;

namespace AcademiaFrontEnd.Request.client
{
    public class StartWorkoutRequest
    {
        public long IdUsuario { get; set; }
        public string Inicio { get; set; }
        public long WorkoutId { get; set; }
    }
}
