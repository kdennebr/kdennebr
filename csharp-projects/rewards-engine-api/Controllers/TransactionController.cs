using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using loyalty_application.Data;
using loyalty_application.DTOs;
using loyalty_application.Models;

namespace loyalty_application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly RewardsDbContext _context;

    public TransactionsController(RewardsDbContext context)
    {
        _context = context;
    }

    // Gets all purchase transactions
    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var transactions = await _context.PurchaseTransactions.ToListAsync();

        return Ok(transactions);
    }

    // Gets one transaction by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransactionById(int id)
    {
        var transaction = await _context.PurchaseTransactions.FindAsync(id);

        if (transaction == null)
        {
            return NotFound("Transaction not found");
        }

        return Ok(transaction);
    }

    // Creates a new purchase transaction and awards points
    [HttpPost]
    public async Task<IActionResult> CreateTransaction(CreateTransactionRequest request)
    {
        // Validates Customer ID
        if (request.CustomerId <= 0)
        {
            return BadRequest("Customer ID is required");
        }

        // Validates External Transaction ID
        if (string.IsNullOrWhiteSpace(request.ExternalTransactionId))
        {
            return BadRequest("External transaction ID is required");
        }

        // Validates Subtotal
        if (request.Subtotal <= 0)
        {
            return BadRequest("Subtotal must be greater than zero");
        }

        // Validates Category
        if (string.IsNullOrWhiteSpace(request.Category))
        {
            return BadRequest("Category is required");
        }

        // Finds Customer
        var customer = await _context.Customers.FindAsync(request.CustomerId);

        if (customer == null)
        {
            return NotFound("Customer not found");
        }

        // Prevents Inactive Customers From Earning Points
        if (!customer.IsActive)
        {
            return BadRequest("Inactive customers cannot earn points");
        }

        // Prevents Duplicate Transactions From Being Processed
        bool duplicateTransaction = await _context.PurchaseTransactions
            .AnyAsync(t => t.ExternalTransactionId == request.ExternalTransactionId);

        if (duplicateTransaction)
        {
            return Conflict("A transaction with this external transaction ID already exists");
        }

        // Calculates Points
        // Current Rule: 1 Point Per Whole Dollar Spent
        int basePoints = (int)Math.Floor(request.Subtotal);
        int bonusPoints = 0;
        int totalPoints = basePoints + bonusPoints;

        // Creates Purchase Transaction
        var transaction = new PurchaseTransaction
        {
            CustomerId = request.CustomerId,
            ExternalTransactionId = request.ExternalTransactionId,
            TransactionDate = DateTime.UtcNow,
            Subtotal = request.Subtotal,
            Category = request.Category,
            BasePointsEarned = basePoints,
            BonusPointsEarned = bonusPoints,
            TotalPointsEarned = totalPoints
        };

        // Saves Transaction First So It Gets An ID
        _context.PurchaseTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        // Creates Ledger Entry For Points Earned
        var ledgerEntry = new PointLedgerEntry
        {
            CustomerId = request.CustomerId,
            PurchaseTransactionId = transaction.PurchaseTransactionId,
            PointsChanged = totalPoints,
            Reason = $"Points earned from purchase {request.ExternalTransactionId}",
            CreatedAt = DateTime.UtcNow
        };

        // Saves Points Ledger Entry
        _context.PointLedgerEntries.Add(ledgerEntry);
        await _context.SaveChangesAsync();

        // Returns Created Transaction
        return CreatedAtAction(
            nameof(GetTransactionById),
            new { id = transaction.PurchaseTransactionId },
            transaction
        );
    }
}