using System;
using System.ComponentModel.DataAnnotations;
using RevStack.Identity.Mvc;

namespace RevStack.Admin
{
    public class CreateProfileBaseModel<TKey> : ProfileBaseModel<TKey>,ICreateProfileModel<TKey>
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
