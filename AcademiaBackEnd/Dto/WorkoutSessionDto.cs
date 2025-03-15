namespace AcademiaBackEnd.Dto
{
    public class WorkoutSessionDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Inicio { get; set; }
        public string Fim { get; set; }
        public string? Duracao { get; set; }
        public WorkoutsDto Treino { get; set; }


    }
}
