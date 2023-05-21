using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inspira_Libertad.DTOs
{
    public class CursoDTO
    {
        public int CursoId { get; set; }
        public string Nombre { get; set; }
        public float Precio { get; set; }
        public string Url { get; set; }
        public IFormFile File { get; set; }
    }
}
