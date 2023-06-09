using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DB;
using ShoppingCart.Dtos;

namespace ShoppingCart.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly DbCtx context;

    public CustomerController(DbCtx context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
    {
        var customers = await context.Customers.ToListAsync();

        if (customers == null || customers.Count <= 0)
        {
            return NotFound();
        }

        return customers;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> Get(int id)
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        return customer;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Customer customer)
    {
        if (id != customer.Id)
        {
            return BadRequest();
        }

        try
        {
            context.Customers.Update(customer);
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
    public async Task<Customer> Post(Customer customer)
    {
        var added = await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();
        // 重新載入該物件方法
        // await context.Entry(customer).ReloadAsync();
        return added.Entity;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        context.Customers.Remove(customer);
        await context.SaveChangesAsync();

        return Ok();
    }
}