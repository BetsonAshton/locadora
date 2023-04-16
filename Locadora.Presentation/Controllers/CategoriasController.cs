using Loacadora.Data.Entities;
using Loacadora.Data.Repositories;
using Locadora.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Locadora.Presentation.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(CategoriasCadastroViewModel model)
        {
            if(ModelState.IsValid)
            {
                try 
                {
                    var data = User.Identity.Name;
                    var identityViewModel = JsonConvert.DeserializeObject<IdentityViewModel>(data);

                    var categoria = new Categoria();
                    categoria.Id = Guid.NewGuid();
                    categoria.Nome = model.Nome;
                    categoria.IdUsuario = identityViewModel.Id;

                    var categoriaRepository = new CategoriaRepository();
                    categoriaRepository.Add(categoria);

                    TempData["MensagemSucesso"] = $"Categoria {categoria.Nome}, cadastrada com sucesso.";

                    ModelState.Clear();
                }
                catch(Exception e) 
                {
                    TempData["MensagemErro"] = $"Falha ao cadastrar categoria: { e.Message}";

                }

            }
            else 
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento de formulário.";
            
            }

            return View();
        }

        public IActionResult Consulta()
        {
            var model = new List<CategoriasConsultaViewModel>();
            try 
            {
                var categoriaRepository = new CategoriaRepository();
                foreach(var item in categoriaRepository.GetByUsuario(UsuarioAutenticado.Id))
                    {
                        var categoriaConsultaViewModel = new CategoriasConsultaViewModel();
                    categoriaConsultaViewModel.Id = item.Id;
                    categoriaConsultaViewModel.Nome = item.Nome;

                    model.Add(categoriaConsultaViewModel);

                    }
            
            }
            catch (Exception e) 
            {
                TempData["MensagemErro"] = "Falha ao consultar categorias: " + e.Message;
            
            }

            return View(model);
        }

      

        public IActionResult Exclusao(Guid id)
        {
            try
            {
                var categoriaRepository = new CategoriaRepository();
                var categoria = categoriaRepository.GetById(id);

                if(categoria != null && categoria.IdUsuario == UsuarioAutenticado.Id) 
                { 
                        categoriaRepository.Delete(categoria);

                    TempData["MensagemSucesso"] = "Categoria excluída com sucesso.";
                }

            }
            catch (Exception e) 
            {
                TempData["MensagemErro"] = "Falha ao excluir categoria: " + e.Message;
            
            }
            return RedirectToAction("Consulta");
        }

        public IActionResult Edicao(Guid id)
        {

            var model = new CategoriaEdicaoViewModel();

            try 
            {
                var categoriaRepository = new CategoriaRepository();
                var categoria = categoriaRepository.GetById(id);

                if(categoria != null && categoria.IdUsuario == UsuarioAutenticado.Id)
                {
                    model.Id = categoria.Id;
                    model.Nome = categoria.Nome;

                }
            
            }
            catch (Exception e) 
            {
                TempData["MensagemErro"] = "Falha ao obter categoria: " + e.Message;

            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edicao(CategoriaEdicaoViewModel model)
        {
            if(ModelState.IsValid)
            {
                try 
                {
                    var categoriaRepository = new CategoriaRepository();
                    var categoria = categoriaRepository.GetById(model.Id);

                    if(categoria != null && categoria.IdUsuario==UsuarioAutenticado.Id)
                    {
                        categoria.Nome = model.Nome;
                        categoriaRepository.Update(categoria);

                        TempData["MensagemSucesso"] = "Categoria atualizada com sucesso!";

                        return RedirectToAction("consulta");
                    }
                
                }
                catch(Exception e) 
                {
                    TempData["MensagemErro"] = "Falha ao atualizar categoria: " + e.Message;
                }
            
            }

            return View(model);

        }

        private IdentityViewModel UsuarioAutenticado
        {
            get
            {
                var data = User.Identity.Name;
                return JsonConvert.DeserializeObject<IdentityViewModel>(data);
            }

        }
    }
}
