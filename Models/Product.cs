using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AngularProject.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string? Title { get; set; }
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }


        [Required]
        public int Quantity { get; set; }

        [ForeignKey("Category")]
        //foriegn
        public int CategoryId { get; set; }
        [JsonIgnore]

        public virtual Category? Category { get; set; }
        [Required]
        public string? ImageUrl { get; set; }

        [Required]
        public bool IsPromoted { get; set; }

        [Required]
        public int  PromotionPercentage { get; set; }


        //To be handled
        public double OverAllRating { get; set; }



    }
}
