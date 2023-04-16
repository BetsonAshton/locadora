using System.ComponentModel.DataAnnotations;

namespace Locadora.Presentation.Models
{
    public class FilmeConsultaViewModel
    {
        [Required(ErrorMessage = "Por favor, informe o nome do filme.")]
        public string? Nome { get; set; }

        public List<FilmeConsultaResultadoViewModel>? Resultado { get; set; }
    }
}
