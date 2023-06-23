using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_PreCookingPackage.Models;

namespace SWP391_PreCookingPackage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesIngredientsController : ControllerBase
    {
        private readonly PrecookContext _context;

        public RecipesIngredientsController(PrecookContext context)
        {
            _context = context;
        }

        // GET: api/RecipesIngredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipesIngredient>>> GetRecipesIngredients()
        {
          if (_context.RecipesIngredients == null)
          {
              return NotFound();
          }
            return await _context.RecipesIngredients.ToListAsync();
        }

        // GET: api/RecipesIngredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipesIngredient>> GetRecipesIngredient(int id)
        {
          if (_context.RecipesIngredients == null)
          {
              return NotFound();
          }
            var recipesIngredient = await _context.RecipesIngredients.FindAsync(id);

            if (recipesIngredient == null)
            {
                return NotFound();
            }

            return recipesIngredient;
        }

        // PUT: api/RecipesIngredients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipesIngredient(int id, RecipesIngredient recipesIngredient)
        {
            if (id != recipesIngredient.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipesIngredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipesIngredientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RecipesIngredients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RecipesIngredient>> PostRecipesIngredient(RecipesIngredient recipesIngredient)
        {
          if (_context.RecipesIngredients == null)
          {
              return Problem("Entity set 'PrecookContext.RecipesIngredients'  is null.");
          }
            _context.RecipesIngredients.Add(recipesIngredient);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RecipesIngredientExists(recipesIngredient.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRecipesIngredient", new { id = recipesIngredient.Id }, recipesIngredient);
        }

        // DELETE: api/RecipesIngredients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipesIngredient(int id)
        {
            if (_context.RecipesIngredients == null)
            {
                return NotFound();
            }
            var recipesIngredient = await _context.RecipesIngredients.FindAsync(id);
            if (recipesIngredient == null)
            {
                return NotFound();
            }

            _context.RecipesIngredients.Remove(recipesIngredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipesIngredientExists(int id)
        {
            return (_context.RecipesIngredients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
