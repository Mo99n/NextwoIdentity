using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextwoIdentity.Models.ViewModels
{
    public class Prodects
    {
        [Key]
        public int? ProdectId { get; set; }

        public string? ProdectName { get; set; }

        public string? Description { get; set; }


        [ForeignKey("Category")]
        public int? CategoreyId { get; set; }

        public Category? Category { get; set; }



    }
}
