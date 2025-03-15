namespace AcademiaBackEnd.Dto
{
    public class ExerciseDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }

        public int sets { get; set; }

        public int reps { get; set; }
        public string carga { get; set; }

        public string Video { get; set; }

    }
}
