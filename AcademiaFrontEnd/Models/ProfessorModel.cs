﻿namespace AcademiaFrontEnd.Models
{
    public class ProfessorModel
    {

        public string imgperfil { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public double? Peso { get; set; }
        public double? Altura { get; set; }

        public TimeSpan Horastotais { get; set; }
        public int idade { get; set; }
        public double? Imc { get; set; }

        public List<ClientModel> alunos { get; set; }
    }
}
