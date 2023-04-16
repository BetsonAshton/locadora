using Loacadora.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Locadora.Presentation.Models
{
    public class FilmeCadastroViewModel
    {
        [MinLength(5, ErrorMessage = "Por favor, informe no mínimo {1}caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1}caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome do filme.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de lançamento.")]
        public DateTime? DataCadastro { get; set; }

        [Required(ErrorMessage = "Por favor, informe a disponibilidade do filme.")]
        public int? Tipo { get; set; }

        [Required(ErrorMessage = "Por favor, informe a categoria do filme.")]
        public Guid? IdCategoria { get; set; }

        public List<SelectListItem>? Categorias { get; set; }

        //private Object? DataDeLançamento { get; set; }


    }
}
