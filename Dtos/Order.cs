using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Dtos;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public OrderStatus Status { get; set; }
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}

public enum OrderStatus
{
    Cart,
    Completed
}
