using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace PetroOperaciones.Models
{

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Clave Actual")]
        public string OldPassword { get; set; }


        [StringLength(100, ErrorMessage = "El {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 6)]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]

        [RegularExpression(@"^.*(?=.{6,18})(?=.*\d)(?=.*[A-Za-z])(?=.*[@%&#]{0,}).*$", ErrorMessage = "Debe contener por lo menos un número  y una letra")]
        [Display(Name = "Nueva Clave")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Clave")]
        [Compare("NewPassword", ErrorMessage = "La nueva clave y la confirmación no son iguales")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
  
        [Display(Name = "Nombre de Usuario")]
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }

        [Display(Name = "No cerrar sesión")]
        public bool RememberMe { get; set; }


    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
