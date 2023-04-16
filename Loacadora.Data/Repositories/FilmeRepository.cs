using Dapper;
using Loacadora.Data.Configurations;
using Loacadora.Data.Entities;
using Loacadora.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loacadora.Data.Repositories
{
    public class FilmeRepository: IRepository<Filme>
    {
        public void Add(Filme entity)
        {
            var query = @"
             INSERT INTO FILME( ID, NOME, DATA, TIPO, IDUSUARIO, IDCATEGORIA)
             VALUES(@Id, @Nome, @DataCadastro, @Tipo, @IdUsuario, @IdCategoria)
            ";

            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Update(Filme entity)
        {
            // Verificando se o valor da coluna IDCATEGORIA em entity é válido
            if (entity.Categoria == null || entity.Categoria.Id == Guid.Empty)
            {
                throw new ArgumentException("A categoria do filme não foi informada ou é inválida.");
            }

            var query = @"
            UPDATE FILME SET
            NOME = @Nome, DATA = @DataCadastro,TIPO = @Tipo, IDCATEGORIA = @IdCategoria 
            WHERE ID = @Id
            ";

            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                connection.Execute(query, new
                {
                    entity.Nome,
                    entity.DataCadastro,
                    entity.Tipo,
                    IdCategoria = entity.Categoria.Id,
                    entity.Id
                });
            }
        }


        public void Delete(Filme entity)
        {
            var query = @"
                DELETE FROM FILME 
                WHERE ID = @Id
            ";

            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public List<Filme> GetAll()
        {
            var query = @"
                SELECT * FROM FILME f
                INNER JOIN CATEGORIA ca
                ON ca.ID = f.IDCATEGORIA
                ORDER BY f.NOME
            ";

            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                return connection.Query(query,
                    (Filme f, Categoria ca) => { f.Categoria = ca; return f; },
                    splitOn: "IdCategoria")
                    .ToList();
            }
        }

        public List<Filme> GetByUsuario(Guid idUsuario)
        {
            var query = @"
                SELECT * FROM FILME f
                INNER JOIN CATEGORIA ca
                ON ca.ID = f.IDCATEGORIA
                WHERE f.IDUSUARIO = @idUsuario
                ORDER BY f.NOME
            ";

            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                return connection.Query(query,
                    (Filme f, Categoria ca) => { f.Categoria = ca; return f; },
                    new { idUsuario },
                    splitOn: "IdCategoria")
                    .ToList();
            }
        }

        public List<Filme> GetByUsuarioAndNome(Guid idUsuario, string nome)
        {
            var query = @"
            SELECT * 
            FROM FILME f
            INNER JOIN CATEGORIA ca ON ca.ID = f.IDCATEGORIA
            WHERE f.IDUSUARIO = @IdUsuario 
            AND f.NOME LIKE '%' + @Nome + '%'
            ORDER BY f.NOME";

            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                connection.Open();
                return connection.Query<Filme, Categoria, Filme>(query,
                    (filme, categoria) =>
                    {
                        filme.Categoria = categoria;
                        return filme;
                    },
                    new { IdUsuario = idUsuario, Nome = nome },
                    splitOn: "ID,ID")
                .Distinct()
                .ToList();
            }
        }




        public Filme? GetById(Guid id)
        {
            var query = @"
                SELECT * FROM FILME f
                INNER JOIN CATEGORIA ca
                ON ca.ID = f.IDCATEGORIA
                WHERE f.ID = @Id
            ";

            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                return connection.Query(query,
                   (Filme f, Categoria ca) => { f.Categoria = ca; return f; },
                   new { id },
                   splitOn: "IdCategoria")
                   .FirstOrDefault();
            }
        }
    }
}
