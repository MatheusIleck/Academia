﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AcademiaBackEnd.Data.Models;

[Table("treino")]
public partial class Treino
{
    [Key]
    public long Id { get; set; }

    [Required]
    [Column("nome")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; }

    [Required]
    [Column("descricao")]
    [StringLength(255)]
    [Unicode(false)]
    public string Descricao { get; set; }

    [Column("id_usuario")]
    public long? IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Treinos")]
    public virtual Usuario IdUsuarioNavigation { get; set; }

    [InverseProperty("Treino")]
    public virtual ICollection<SessaoTreino> SessaoTreinos { get; set; } = new List<SessaoTreino>();

    [InverseProperty("IdTreinoNavigation")]
    public virtual ICollection<TreinoExercicio> TreinoExercicios { get; set; } = new List<TreinoExercicio>();
}