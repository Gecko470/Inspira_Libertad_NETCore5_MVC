using Inspira_Libertad.Models;
using System.Collections.Generic;
using System;

namespace Inspira_Libertad.DTOs
{
    public class OrdenPendienteDTO
    {
        public int OrderId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public float ImporteTotal { get; set; }
        public List<Curso> Cursos { get; set; }
    }
}
