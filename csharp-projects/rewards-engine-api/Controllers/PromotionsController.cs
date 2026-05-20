using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using loyalty_application.Data;
using loyalty_application.DTOs;
using loyalty_application.Models;

namespace loyalty_application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromotionsController : ControllerBase
{
    private readonly RewardsDbContext _context;
    public PromotionsController(RewardsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetPromotions()
    {
        var promotions = await _context.Promotions.Where(p => p.IsActive).ToListAsync();
        return Ok(promotions);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActivePromotions()
    {
        var now = DateTime.UtcNow;

        var promotions = await _context.Promotions.Where(p => p.IsActive).ToListAsync();
        
        return Ok(promotions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPromotionById(int id)
    {
        var promotion = await _context.Promotions.FindAsync(id);
        if (promotion == null)        
        {
            return NotFound();  
        }
        return Ok(promotion);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePromotion(CreatePromotionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Promotion name is required");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return BadRequest("Promotion description is required");
        }

        if (string.IsNullOrWhiteSpace(request.PromotionType))
        {
            return BadRequest("Promotion type is required");
        }

        if (request.EndDate <= request.StartDate)
        {
            return BadRequest("End date must be after start date");
        }

        if (request.PromotionType == "Multiplier" && 
            (request.Multiplier == null || request.Multiplier <= 1))
        {
            return BadRequest("Multiplier promotions require a multiplier greater than 1");
        }

        if ((request.PromotionType == "FixedBonus" || request.PromotionType == "MinimumSpendBonus") &&
            (request.BonusPoints == null || request.BonusPoints <= 0))
        {
            return BadRequest("Bonus point promotions require bonus points greater than 0");
        }

        if (request.PromotionType == "MinimumSpendBonus" &&
            (request.MinimumSpend == null || request.MinimumSpend <= 0))
        {
            return BadRequest("Minimum spend promotions require a minimum spend greater than 0");
        }

        var promotion = new Promotion
        {
            Name = request.Name,
            Description = request.Description,
            PromotionType = request.PromotionType,
            Category = request.Category,
            Multiplier = request.Multiplier,
            BonusPoints = request.BonusPoints,
            MinimumSpend = request.MinimumSpend,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = true
        };

        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPromotionById),
            new { id = promotion.PromotionId },
            promotion
        );
    }

    [HttpPatch("{id}/disable")]
    public async Task<IActionResult> DisablePromotion(int id)
    {
        var promotion = await _context.Promotions.FindAsync(id);
        
        if (promotion == null)
        {
            return NotFound();
        }

        promotion.IsActive = false;
        await _context.SaveChangesAsync();

        return Ok(promotion);
    }
}
    