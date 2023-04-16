using Loacadora.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loacadora.Data.Entities
{
    public class Filme
    {
        private Guid _id;
        private string? _nome;
        private DateTime? _dataCadastro;
        private TipoFilme? _tipo;
        private Guid? _idUsuario;
        private Guid _idCategoria;
        private Usuario? _usuario;
        private Categoria? _categoria;
        private List<Categoria>? _categorias;

        public Guid Id { get => _id; set => _id = value; }
        public string? Nome { get => _nome; set => _nome = value; }
        public DateTime? DataCadastro { get => _dataCadastro; set => _dataCadastro = value; }
        public TipoFilme? Tipo { get => _tipo; set => _tipo = value; }
        public Guid? IdUsuario { get => _idUsuario; set => _idUsuario = value; }
        public Guid IdCategoria { get => _idCategoria; set => _idCategoria = value; }
        public Usuario? Usuario { get => _usuario; set => _usuario = value; }
        public Categoria? Categoria { get => _categoria; set => _categoria = value; }
        public List<Categoria>? Categorias { get => _categorias; set => _categorias = value; }
    }
}
