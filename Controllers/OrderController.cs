using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DB;
using ShoppingCart.Dtos;

namespace ShoppingCart.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly DbCtx context;

    public OrderController(DbCtx context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await context.Orders.Where(o => o.Status == OrderStatus.Cart).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrders(int id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return order;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Order order)
    {
        var findOrder = await context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        if (findOrder == null)
        {
            findOrder = new Order
            {
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                Status = OrderStatus.Cart
            };
            context.Orders.Add(findOrder);
        }
        else
        {
            findOrder.Quantity += order.Quantity;
            context.Entry(findOrder).State = EntityState.Modified;
        }

        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        context.Orders.Remove(order);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Route("BuyAll")]
    public async Task<IActionResult> BuyAll()
    {
        var carts = await context.Orders.Where(o => o.Status == OrderStatus.Cart).ToListAsync();
        if (carts.Count == 0)
        {
            return BadRequest("Data Not Found");
        }

        foreach (var order in carts)
        {
            order.Status = OrderStatus.Completed;
            context.Entry(order).State = EntityState.Modified;

            var product = await context.Products.FindAsync(order.ProductId);
            if (product != null)
            {
                product.Quantity -= order.Quantity;
                context.Entry(product).State = EntityState.Modified;
            }
        }

        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    [Route("Complete")]
    public async Task<ActionResult<IEnumerable<Order>>> GetComplete()
    {
        return await context.Orders.Where(o => o.Status == OrderStatus.Completed).ToListAsync();
    }
}