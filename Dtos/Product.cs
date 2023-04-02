using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Dtos;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required, Column(TypeName = "VARCHAR"), MaxLength(500)]
    public string? Name { get; set; }
    [Required]
    public int Price { get; set; }
    [Required]
    public int Quantity { get; set; }
}
