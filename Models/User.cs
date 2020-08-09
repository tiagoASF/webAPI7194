using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Este campo deverá conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deverá conter entre 3 e 60 caracteres")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Ente campo deverá conter entre 8 e 20 caracteres")]
        [MinLength(8, ErrorMessage = "Este campo devberá conter entre 8 e 20 caracteres")]
        public string Password { get; set; }

        public string Role { get; set; }



    }
}