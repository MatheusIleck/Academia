namespace AcademiaBackEnd.Dto
{
    public class AddExerciseDto
    {
        public long Id { get; set; } 
        public string Nome { get; set; }
        public string Carga { get; set; } 
        public int Reps { get; set; } 
        public int Sets { get; set; } 
    }
}
