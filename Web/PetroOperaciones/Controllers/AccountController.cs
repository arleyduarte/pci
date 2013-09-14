using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PetroOperaciones.Models;
using Pdfizer;
using System.IO;
using iTextSharp.text;

namespace PetroOperaciones.Controllers
{
    public class AccountController : Controller 
    {
        private ValidadorUsuario vu = new ValidadorUsuario();
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {


            if (ModelState.IsValid)
            {

                if (vu.validarUsuario(model.UserName, model.Password))
                {

                    Usuario usuario = vu.getUsuarioByLogin(model.UserName);

                    string roles = usuario.Rol;
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                      1,
                      usuario.NombreUsuario,  //user id
                      DateTime.Now,
                      DateTime.Now.AddMinutes(120),  // expiry
                      false,  //do not remember
                      roles,
                      "/");
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);

                    if (roles.Equals(TipoRol.CLIENTE))
                    {
                        return RedirectToAction("GetListDate", "PCIClientesDO");
                       
                    }
                    else
                    {
                        return RedirectToAction("GetUltimosSeguimiento", "Seguimiento");
                    }


           
                }

                else
                {
                    ModelState.AddModelError("", "El nombre de usuario o la clave no son validos");
                }
            }

            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("LogOn", "Account");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try


                {

                    

                    if (vu.validarUsuario(User.Identity.Name, model.OldPassword))
                    {
                        Usuario usuarioN = vu.getUsuarioByLogin(User.Identity.Name);
                        EfDbContext db = new EfDbContext();

                        Usuario usuario = db.Usuarios.Find(usuarioN.UsuarioID);
                        usuario.Clave = model.NewPassword;

                        db.SaveChanges();

                        changePasswordSucceeded = true;
                    }
                    else
                    {
                        changePasswordSucceeded = false;
                    }


                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("LogOn");
                }
                else
                {
                    ModelState.AddModelError("", "La clave actual no es valida.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        private string GetRoles(string username)
        {
            // Lookup code omitted for clarity
            // This code would typically look up the role list from a database 
            // table. If the user was being authenticated against Active 
            // Directory, the Security groups and/or distribution lists that 
            // the user belongs to may be used instead

            // This GetRoles method returns a pipe delimited string containing 
            // roles rather than returning an array, because the string format 
            // is convenient for storing in the authentication ticket / 
            // cookie, as user data
            return "Admin,Employee";
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
