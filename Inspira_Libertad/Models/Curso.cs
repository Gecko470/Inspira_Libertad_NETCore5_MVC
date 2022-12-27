using System.Collections.Generic;

namespace Inspira_Libertad.Models
{
    public class Curso
    {
        public int CursoId { get; set; }
        public string Nombre { get; set; }
        public float Precio { get; set; }
        public string Url { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
