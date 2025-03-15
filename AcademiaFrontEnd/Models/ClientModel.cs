namespace AcademiaFrontEnd.Models
{
    public class ClientModel
    {
        public long id { get; set; }
        public string imgperfil { get; set; }

        public string Nome { get; set; }
        public string Email { get; set; }
        public double? Peso { get; set; }
        public double? Altura { get; set; }
        public double? Imc { get; set; }
        public int idade { get; set; }
        public int ofensiva { get; set; }

        public TimeSpan Horastotais { get; set; }
        public List<WorkoutModel> Workouts { get; set; }
        public List<WorkoutSessionModel> WorkoutsSessions { get; set; }
        public List<SolicitacaoModel> solicitacoes { get; set; }


    }

}
