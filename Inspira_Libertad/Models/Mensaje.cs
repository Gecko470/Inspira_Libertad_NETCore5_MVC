using System.ComponentModel.DataAnnotations;
using System;

namespace Inspira_Libertad.Models
{
    public class Mensaje
    {
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public DateTime Fecha { get; set; } = DateTime.Now;


        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(100, ErrorMessage = "Este campo tiene una longitud máxima de 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(40, ErrorMessage = "Este campo tiene una longitud máxima de 40 caracteres")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Introduzca un Email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(300, ErrorMessage = "Este campo tiene una longitud máxima de 300 caracteres")]
        public string Texto { get; set; }
    }
}
