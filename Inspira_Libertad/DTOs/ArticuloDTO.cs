using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inspira_Libertad.Validaciones;

namespace Inspira_Libertad.DTOs
{
    public class ArticuloDTO
    {
        public int ArticuloId { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio..")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Precio es obligatorio..")]
        public float Precio { get; set; }
        [Required(ErrorMessage = "El campo Video es obligatorio..")]
        public IFormFile File { get; set; }
        [Required(ErrorMessage = "El campo Url es obligatorio..")]
        public string Url { get; set; }
        [Required(ErrorMessage = "El campo Texto es obligatorio..")]
        public string Texto { get; set; }
        [Required(ErrorMessage = "El campo Habilitado es obligatorio..")]
        public byte Habilitado { get; set; } = 0;
    }
}
