using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_PreCookingPackage.Models;
using SWP391_PreCookingPackage.ModelsDTO;

namespace SWP391_PreCookingPackage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesIngredientsController : ControllerBase
    {
        private readonly PrecookContext _context;
        private readonly IMapper _mapper;

        public RecipesIngredientsController(PrecookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/RecipesIngredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipesIngredientModel>>> GetRecipesIngredients()
        {
          if (_context.RecipesIngredients == null)
          {
              return NotFound();
          }
            var result = _mapper.Map<IEnumerable<RecipesIngredient>>(_context.RecipesIngredients);
            return Ok(result);
        }

        // GET: api/RecipesIngredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipesIngredientModel>> GetRecipesIngredient(int id)
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
            var result = _mapper.Map<RecipesIngredientModel>(recipesIngredient);
            return Ok(result);
        }

        // PUT: api/RecipesIngredients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipesIngredient(int id, RecipesIngredientModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            RecipesIngredient recipesIngredient = _mapper.Map<RecipesIngredient>(model);
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
        public async Task<ActionResult<RecipesIngredient>> PostRecipesIngredient(RecipesIngredientModel model)
        {
          if (_context.RecipesIngredients == null)
          {
              return Problem("Entity set 'PrecookContext.RecipesIngredients'  is null.");
          }
            model.Id = null;
            RecipesIngredient recipesIngredient = _mapper.Map<RecipesIngredient>(model);
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
