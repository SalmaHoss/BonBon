using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AngularProject.Models
{
    public class Category
    {
        public Category()
        {
            ProductList = new List<Product>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual List<Product> ProductList { get; set; }
    }
}
