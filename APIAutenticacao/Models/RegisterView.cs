using System.ComponentModel.DataAnnotations;

namespace ApiAutenticacao.Models {
    public class RegisterView {
        [Required(ErrorMessage = "O campo {0} � obrigatorio")]
        [EmailAddress(ErrorMessage ="O campo {0} est� em um formato inv�lido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigatorio")]
        [StringLength(100,  ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres ", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="As duas senhas n�o conferem")]
        public string ConfirmPassword { get; set; }
    }
}