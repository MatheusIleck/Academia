﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AcademiaBackEnd.Data.Models;

[Table("solicitacoes")]
public partial class Solicitaco
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("cliente_id")]
    public long ClienteId { get; set; }

    [Column("professor_id")]
    public long ProfessorId { get; set; }

    [Required]
    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; }

    [ForeignKey("ClienteId")]
    [InverseProperty("SolicitacoClientes")]
    public virtual Usuario Cliente { get; set; }

    [ForeignKey("ProfessorId")]
    [InverseProperty("SolicitacoProfessors")]
    public virtual Usuario Professor { get; set; }
}