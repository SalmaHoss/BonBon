using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AngularProject.Models
{
    public enum OrderState { Pending, Accepted , Canceled}
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }

        
        public OrderState State { get; set; }
        [ForeignKey(nameof(UserId))]
       
        public User User { get; set; }

        //public string Email { get; set; }
        public virtual List<OrderProduct> OrderProducts { get; set; }

    }
}
