using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace NextwoIdentity.Models.ViewModels
{
    public class CreateRoleViewModel
    {



        [Required]
        public string ?RoleName  { get; set; }



    }
}
