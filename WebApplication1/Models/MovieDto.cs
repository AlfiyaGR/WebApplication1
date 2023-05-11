using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class MovieDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Write a title")]
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string? GenreName { get; set; }

        [Required]
        [Display(Name = "Цена")]
        [Range(typeof(decimal), "5,0", "1000,6", ErrorMessage = "Наименьшая цена - 5 рублей, в качестве разделителя дробной и целой части используется запятая")]
        public decimal Price { get; set; }

        public List<MovieAuthorDto>? Authors { get; set; }

        //public List<string>? AuthorNames { get; set; }

        //public List<string>? AuthorEmail { get; set; }
    }
}
