using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DB;
using ShoppingCart.Dtos;

namespace ShoppingCart.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly DbCtx context;

    public ProductController(DbCtx context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await context.Products.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        context.Entry(product).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Products.Any(e => e.Id == id))
            {
                return NotFound();
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Post(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // 重新載入該物件以取得完整資料
        await context.Entry(product).ReloadAsync();

        return product;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return NoContent();
    }
}