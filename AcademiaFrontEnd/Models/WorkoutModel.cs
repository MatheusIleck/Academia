namespace AcademiaFrontEnd.Models
{
    public class WorkoutModel
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public List<ExerciseModel> Exercicios { get; set; }
    }
}
