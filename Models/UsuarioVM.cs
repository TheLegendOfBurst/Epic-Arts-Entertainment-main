namespace Epic_Arts_Entertainment.Models
{
    public class UsuarioVM
    {
        public int IdUsuario { get; set; }

        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telefone { get; set; } = null!;

        public string Senha { get; set; } = null!;

        public int TipoUsuario { get; set; }
    }
}
