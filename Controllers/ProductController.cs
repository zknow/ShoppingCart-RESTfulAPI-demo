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
        if (product == null)
        {
            return BadRequest();
        }

        try
        {
            product.Id = id;
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Products.Any(e => e.Id == id))
            {
                return NotFound();
            }
        }

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Post(Product product)
    {
        if (product == null)
        {
            return BadRequest();
        }
        var added = await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        // 重新載入該物件方法
        // await context.Entry(product).ReloadAsync();
        return added.Entity;
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

        return Ok();
    }
}