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
    public class IngredientsController : ControllerBase
    {
        private readonly PrecookContext _context;
        private readonly IMapper _mapper;

        public IngredientsController(PrecookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientModel>>> GetIngredients()
        {
          if (_context.Ingredients == null)
          {
              return NotFound();
          }
            var result = _mapper.Map<IEnumerable<IngredientModel>>(_context.Ingredients.ToList());
            return Ok(result);
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientModel>> GetIngredient(int id)
        {
          if (_context.Ingredients == null)
          {
              return NotFound();
          }
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<IngredientModel>(ingredient);
            return Ok(result);
        }

        // PUT: api/Ingredients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredient(int id, IngredientModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if(_context.Ingredients == null)
            {
                return NotFound("Ingredients is null");
            }
            Ingredient ingredient = _mapper.Map<Ingredient>(model);
            _context.Entry(ingredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
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

        // POST: api/Ingredients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ingredient>> PostIngredient(IngredientModel model)
        {
          if (_context.Ingredients == null)
          {
              return Problem("Entity set 'PrecookContext.Ingredients'  is null.");
          }
            model.Id = null;
            Ingredient ingredient = _mapper.Map<Ingredient>(model);
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngredient", new { id = ingredient.Id }, ingredient);
        }

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            if (_context.Ingredients == null)
            {
                return NotFound();
            }
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngredientExists(int id)
        {
            return (_context.Ingredients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
