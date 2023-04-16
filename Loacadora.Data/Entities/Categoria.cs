using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loacadora.Data.Entities
{
    public class Categoria
    {
        private Guid _id; 
        private string? _nome;
        private Guid _idUsuario;
        private Guid? _idFilme;
        private Usuario? _usuario;
        private List<Filme>? _filmes;

        public Guid Id { get => _id; set => _id = value; }
        public string? Nome { get => _nome; set => _nome = value; }
        public Guid IdUsuario { get => _idUsuario; set => _idUsuario = value; }
        public Guid? IdFilme { get => _idFilme; set => _idFilme = value; }
        public Usuario? Usuario { get => _usuario; set => _usuario = value; }
        public List<Filme>? Filmes { get => _filmes; set => _filmes = value; }
    }
}
