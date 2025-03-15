using AcademiaBackEnd.Dto;

namespace AcademiaBackEnd.Request.Professor
{
    public class CreateNewWorkoutRequest
    {
        public long idusuario {  get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public List<AddExerciseDto> Exercicios { get; set; }
    }
}
