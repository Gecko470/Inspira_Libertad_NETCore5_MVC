using System.ComponentModel.DataAnnotations;

namespace Inspira_Libertad.Models
{
    public class Frase
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Texto es obligatorio..")]
        public string Texto { get; set; }
    }
}
