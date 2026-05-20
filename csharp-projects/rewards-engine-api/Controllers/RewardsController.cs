using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using loyalty_application.Data;
using loyalty_application.Models;
using loyalty_application.DTOs;

namespace loyalty_application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RewardsController : ControllerBase
    {
        private readonly RewardsDbContext _context;

        public RewardsController(RewardsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRewards()
        {
            var rewards = await _context.Rewards.Where(reward => reward.IsActive).ToListAsync();
            return Ok(rewards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRewardById(int id)
        {
            var reward = await _context.Rewards.FindAsync(id);
            if (reward == null)
            {
                return NotFound();
            }
            return Ok(reward);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReward(CreateRewardRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Reward name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return BadRequest("Reward description is required.");
            }

            if (request.PointsRequired <= 0)
            {
                return BadRequest("Points required must be greater than zero.");
            }

            var reward = new Reward
            {
                Name = request.Name,
                Description = request.Description,
                PointsRequired = request.PointsRequired,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Rewards.Add(reward);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRewardById), new { id = reward.RewardId }, reward);
        }
    

        //Redeems a reward for a customer
     [HttpPost("{rewardId}/redeem")]
        public async Task<IActionResult> RedeemReward(int rewardId, RedeemRewardRequest request)
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId);
            if (customer == null)            {
                return NotFound("Customer not found.");
            }

            if (!customer.IsActive)
            {
                return BadRequest("Inactive customers cannot redeem rewards.");
            }

            var reward = await _context.Rewards.FindAsync(rewardId);
            if (reward == null || !reward.IsActive)            
            {
                return NotFound("Reward not found or is inactive.");
            }

            int currentBalance = await _context.PointLedgerEntries
                .Where(entry => entry.CustomerId == request.CustomerId)
                .SumAsync(entry => entry.PointsChanged);
            
            if (currentBalance < reward.PointsRequired)
            {
                return BadRequest("Insufficient points to redeem this reward.");
            }

            var redemption = new Redemption
            {
                CustomerId = request.CustomerId,
                RewardId = rewardId,
                PointsSpent = reward.PointsRequired,
                RedemptionDate = DateTime.UtcNow
            };

            _context.Redemptions.Add(redemption);
            await _context.SaveChangesAsync();

            var ledgerEntry = new PointLedgerEntry
            {
                CustomerId = request.CustomerId,
                PointsChanged = -reward.PointsRequired,
                RedemptionId = redemption.RedemptionId,
                Reason = $"Redeemed reward: {reward.Name}",
                CreatedAt = DateTime.UtcNow
            };

            _context.PointLedgerEntries.Add(ledgerEntry);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Reward redeemed successfully.",
                CustomerId = request.CustomerId,
                RewardId = rewardId,
                PointsSpent = reward.PointsRequired,
                RemainingPoints = currentBalance - reward.PointsRequired
            });
           
        }
    }
}