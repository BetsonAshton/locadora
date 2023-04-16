using Loacadora.Data.Entities;
using Loacadora.Data.Enums;
using Loacadora.Data.Repositories;
using Locadora.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Locadora.Presentation.Controllers
{
    public class FilmeController : Controller
    {
        [Authorize]
        public IActionResult Cadastro()
        {
            var model = new FilmeCadastroViewModel();
            model.Categorias = ObterCategorias();

            return View(model);
        }

        [HttpPost]
        public IActionResult Cadastro(FilmeCadastroViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var filme = new Filme();

                    filme.Id = Guid.NewGuid();
                    filme.Nome = model.Nome;
                    filme.DataCadastro = model.DataCadastro;
                    filme.Tipo = model.Tipo == 1 ? TipoFilme.Disponivel : TipoFilme.Indisponivel;
                    filme.IdCategoria = (Guid)model.IdCategoria;
                    filme.IdUsuario = UsuarioAutenticado.Id;

                    var filmeRepository = new FilmeRepository();
                    filmeRepository.Add(filme);

                    TempData["MensagemSucesso"] = "Filme cadastrado com sucesso";

                    model = new FilmeCadastroViewModel(); 
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = "Falha ao cadastrar filme:" + e.Message;
                }

            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";

            }
            model.Categorias = ObterCategorias();
            return View(model);
        }

        public IActionResult Consulta()
        {
            var model = new FilmeConsultaViewModel();

            try
            {

                ObterFilmes(model);

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Falha ao consultar filmes: " + e.Message;
            }
            return View(model);
        }



        [HttpPost]
        public IActionResult Consulta(FilmeConsultaViewModel model)
        {
            if(ModelState.IsValid) 
            {
                try
                {
                    ObterFilmes(model);
                }
                catch (Exception e) 
                {
                    TempData["MensagemErro"] = "Falha ao consultar filmes: " + e.Message;
                }
            
            }
            
           
            return View(model);
        }

        public IActionResult Exclusao(Guid id)
        {
            try
            {
                var filmeRepository = new FilmeRepository();
                var filme = filmeRepository.GetById(id);

                if (filme != null && filme.IdUsuario == UsuarioAutenticado.Id)
                {
                    filmeRepository.Delete(filme);

                    TempData["MensagemSucesso"] = "Filme excluido com sucesso.";
                }

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Falha ao excluir filme:" + e.Message;

            }
            return RedirectToAction("Consulta");
        }

        public IActionResult Edicao(Guid id)
        {
            var model = new FilmeEdicaoViewModel();

            try
            {
                var filmeRepository = new FilmeRepository();
                var filme = filmeRepository.GetById(id);

                if(filme != null && filme.IdUsuario == UsuarioAutenticado.Id)
                {
                    model.Id = filme.Id;
                    model.Nome = filme.Nome;
                    model.DataCadastro = filme.DataCadastro;
                    model.Tipo = (int?)filme.Tipo;
                    model.IdCategoria = filme.IdCategoria;
                    model.Categorias = ObterCategorias();
                    
                }

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Falha ao exibir os dados dos filmes: " + e.Message;

            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edicao(FilmeEdicaoViewModel model)
        {
            if(ModelState.IsValid) 
            {

                try 
                {
                    var filmeRepository = new FilmeRepository();
                    var filme = filmeRepository.GetById((Guid)model.Id);

                    if(filme != null && filme.IdUsuario ==UsuarioAutenticado.Id)
                    {
                        filme.Nome = model.Nome;
                        filme.DataCadastro = model.DataCadastro;
                        filme.Tipo = (TipoFilme?)model.Tipo;
                        model.IdCategoria = model.IdCategoria;
                       
                        filmeRepository.Update(filme);

                        TempData["MensagemSucesso"] = "Filme atualizado com sucesso.";

                        return RedirectToAction("Consulta");

                    }
                }
                catch (Exception e) 
                {
                    TempData["MensagemErro"] = "Falha ao atualizar FILME: " + e.Message;

                }
            }

            model.Categorias = ObterCategorias();
            return View(model);

        }

        private List<SelectListItem> ObterCategorias()
        {
            
            var categoriaRepository = new CategoriaRepository();
            var categorias = categoriaRepository.GetByUsuario(UsuarioAutenticado.Id);

            var lista = new List<SelectListItem>();
            foreach (var item in categorias)
            {
                var selectListItem = new SelectListItem();
                selectListItem.Value = item.Id.ToString();
                
                selectListItem.Text = item.Nome;
                

                lista.Add(selectListItem);
            }

            return lista;
        }

        
        private IdentityViewModel UsuarioAutenticado
        {
            get
            {
                var data = User.Identity.Name;
                return JsonConvert.DeserializeObject<IdentityViewModel>(data);
            }

        }

        private void ObterFilmes(FilmeConsultaViewModel model, string nome = "")
        {
            var filmeRepository = new FilmeRepository();
            var filmes = filmeRepository.GetByUsuarioAndNome(UsuarioAutenticado.Id, nome);

            model.Resultado = new List<FilmeConsultaResultadoViewModel>();
            foreach (var item in filmes)
            {
                var resultado = new FilmeConsultaResultadoViewModel();
                resultado.Id = item.Id;
                resultado.Nome = item.Nome;
                resultado.DataCadastro = item.DataCadastro.ToString();

                resultado.Tipo = item.Tipo.ToString();
                resultado.Categoria = item.Categoria.Nome;

                model.Resultado.Add(resultado);
            }
        }



    }
}
