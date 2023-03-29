using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextwoIdentity.Models.ViewModels
{
    public class Category
    {
        [Key]
        public int CategoreyId { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [DataType(DataType.Text, ErrorMessage = "Invalid Name ")]
        public string ?CategoreyName { get; set; }

        
    }
}
