
using Loacadora.Data.Repositories;
using Locadora.Presentation.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Loacadora.Data.Entities;
using System.Security.Cryptography;
using Loacadora.Data.Helpers;
using Newtonsoft.Json;

namespace Locadora.Presentation.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()

        { 
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
           if(ModelState.IsValid)
            {
                try
                {
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmailAndSenha(model.Email, MD5Helper.Encrypt(model.Senha));
                    
                    if (usuario != null)
                    {
                        #region Autenticando Usuário
                        var identityViewModel = new IdentityViewModel();
                        identityViewModel.Id = (Guid)usuario.Id;
                        identityViewModel.Nome = usuario.Nome;
                        identityViewModel.Email = usuario.Email;
                        identityViewModel.DataHoraAcesso = DateTime.Now;

                        var claimsIdentity = new ClaimsIdentity(new[]
                       {
                            new Claim(ClaimTypes.Name, JsonConvert.SerializeObject(identityViewModel))
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

                        //gravando o cookie de autenticação
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);


                        //redirecionando o usuário para /Home/Index
                        return RedirectToAction("Index", "Home");
                        #endregion


                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Acesso negado, usuário não encontrado.";
                    }

                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = $"Falha ao autenticar: {e.Message}";
                }

            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento dp formulário";
            }
            return View();

        }



        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioRepository = new UsuarioRepository();

                    if(usuarioRepository.GetByEmail(model.Email)!= null) 
                    {
                        TempData["MensagemAlerta"] = "O email informado já está cadastrado no sistema, tente outro.";


                    }
                    else 
                    {
                        var usuario = new Usuario();

                        usuario.Id = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = MD5Helper.Encrypt(model.Senha);

                        usuarioRepository.Add(usuario);

                        TempData["MensagemSucesso"] = "Parabéns, sua conta foi cadastrada com sucesso.";
                            
                        ModelState.Clear();
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Falha ao cadastrar usuário:{e.Message}";
                }

            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }
            return View();
        }

        public IActionResult Logout()
        {
            //destruir o cookie de autenticação (identificação do usuário)
            HttpContext.SignOutAsync
      (CookieAuthenticationDefaults.AuthenticationScheme);

            //redirecionar de volta para a página de login
            return RedirectToAction("Login", "Account");
        }



    }
}
