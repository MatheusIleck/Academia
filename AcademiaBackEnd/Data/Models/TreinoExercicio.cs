﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AcademiaBackEnd.Data.Models;

[Table("treino_exercicio")]
public partial class TreinoExercicio
{
    [Key]
    public long Id { get; set; }

    [Column("id_treino")]
    public long IdTreino { get; set; }

    [Column("id_exercicio")]
    public long IdExercicio { get; set; }

    [Column("sets")]
    public int Sets { get; set; }

    [Column("reps")]
    public int Reps { get; set; }

    [Required]
    [Column("carga")]
    [StringLength(80)]
    [Unicode(false)]
    public string Carga { get; set; }

    [ForeignKey("IdExercicio")]
    [InverseProperty("TreinoExercicios")]
    public virtual Exercicio IdExercicioNavigation { get; set; }

    [ForeignKey("IdTreino")]
    [InverseProperty("TreinoExercicios")]
    public virtual Treino IdTreinoNavigation { get; set; }
}