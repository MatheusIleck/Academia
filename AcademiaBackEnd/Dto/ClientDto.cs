using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AcademiaBackEnd.Data.Models;

namespace AcademiaBackEnd.Dto
{
    public class ClientDto
    {
        public long id {  get; set; }
        public string imgperfil {  get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public double? Peso { get; set; }
        public double? Altura { get; set; }

        public TimeSpan Horastotais { get; set; }
        public int idade { get; set; }
        public double? Imc { get; set; }

        public int ofensiva { get; set; }

        public List<WorkoutsDto> Workouts { get; set; }
        public List<WorkoutSessionDto> WorkoutsSessions { get; set; }

        public List<SolicitacaoDto> solicitacoes  { get; set; }


    }
}
