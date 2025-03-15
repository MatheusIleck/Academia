namespace AcademiaFrontEnd.Models
{
    public class WorkoutSessionModel
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Inicio { get; set; }
        public string Fim { get; set; }
        public string? Duracao { get; set; }
        public WorkoutModel Treino { get; set; }
    }
}
