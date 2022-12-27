using System.ComponentModel.DataAnnotations.Schema;

namespace Inspira_Libertad.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int CursoId { get; set; }
        public Curso Curso { get; set; }

    }
}
