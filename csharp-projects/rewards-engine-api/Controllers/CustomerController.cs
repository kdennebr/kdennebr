using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using loyalty_application.Data;
using loyalty_application.DTOs;
using loyalty_application.Models;

namespace loyalty_application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly RewardsDbContext _context;
    public CustomerController(RewardsDbContext context)
    {
        _context = context;
    }


    //Gets all customers
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _context.Customers.ToListAsync();
        
        return Ok(customers);
    }

    //Gets a customer by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        
        if (customer == null)
        {
            return NotFound("Customer not found");
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CreateCustomerRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.FirstName) || 
            string.IsNullOrWhiteSpace(request.LastName) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("First name, last name, and email are required.");
        }

        bool emailExists = await _context.Customers.AnyAsync(c => c.Email == request.Email);
        if (emailExists)        {
            return Conflict("Email already has an account.");

        }

        var customer = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
    }

    //Updates an existing customer
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerRequest request)
    {
        var customer = await _context.Customers.FindAsync(id);
        
        if (customer == null)
        {
            return NotFound("Customer not found");
        }
        if(string.IsNullOrWhiteSpace(request.FirstName) || 
            string.IsNullOrWhiteSpace(request.LastName) ||
            string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("First name, last name, and email are required.");
        }
        bool emailExists = await _context.Customers.AnyAsync(c => c.Email == request.Email && c.CustomerId != id);
        if (emailExists)        {
            return Conflict("Email already has an account.");
        }
        
        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.Email = request.Email;

        await _context.SaveChangesAsync();

        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound("Customer not found");
        }

        customer.IsActive = false;
        
        await _context.SaveChangesAsync();

        return Ok(customer);
    }

    [HttpGet("{id}/points")]
    public async Task<IActionResult> GetCustomerPoints(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound("Customer not found");
        }

        int totalPoints = await _context.PointLedgerEntries
            .Where(p => p.CustomerId == id)
            .SumAsync(p => p.PointsChanged);

        return Ok(new { CustomerId = id, TotalPoints = totalPoints });
    }

    [HttpGet("{id}/ledger")]
    public async Task<IActionResult> GetCustomerLedger(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound("Customer not found");
        }

        var ledgerEntries = await _context.PointLedgerEntries
            .Where(p => p.CustomerId == id)
            .ToListAsync();

        return Ok(ledgerEntries);
    }

    // Reactivates a customer
[HttpPatch("{id}/reactivate")]
public async Task<IActionResult> ReactivateCustomer(int id)
{
    var customer = await _context.Customers.FindAsync(id);

    if (customer == null)
    {
        return NotFound("Customer not found");
    }

    if (customer.IsActive)
    {
        return BadRequest("Customer is already active");
    }

    customer.IsActive = true;

    await _context.SaveChangesAsync();

    return Ok("Customer reactivated successfully");
}
}