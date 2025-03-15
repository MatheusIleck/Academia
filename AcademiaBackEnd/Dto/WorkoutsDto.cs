
namespace AcademiaBackEnd.Dto
{
    public class WorkoutsDto
    {

        public long Id { get; set; }
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public List<ExerciseDto> Exercicios { get; set; }

    }
}
