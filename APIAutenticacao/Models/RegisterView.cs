using System.ComponentModel.DataAnnotations;

namespace ApiAutenticacao.Models {
    public class RegisterView {
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [EmailAddress(ErrorMessage ="O campo {0} está em um formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(100,  ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres ", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="As duas senhas não conferem")]
        public string ConfirmPassword { get; set; }
    }
}