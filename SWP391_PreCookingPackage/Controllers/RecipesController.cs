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
    public class RecipesController : ControllerBase
    {
        private readonly PrecookContext _context;
        private readonly IMapper _mapper;

        public RecipesController(PrecookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeModel>>> GetRecipes()
        {
            if (_context.Recipes == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<IEnumerable<RecipeModel>>(_context.Recipes.ToList());
            //foreach(var item in result)
            //{
            //    //include ignredients as an array
            //    item.Ingredients = _context.RecipesIngredients
            //        .Include(ri => ri.Ingredient)
            //        .Where(ri => ri.RecipeId == item.Id)
            //        .Select(ri => ri.Ingredient.Name)
            //        .ToList();
            //    //include packages
            //    item.Packages = _mapper.Map<List<PackageModel>>(_context.Packages.Where(p => p.RecipeId == item.Id));
            //}
            foreach (var item in result)
            {
                //include ingredients
                item.Ingredients = _context.RecipesIngredients
                        .Include(ri => ri.Ingredient)
                        .Where(ri => ri.RecipeId == item.Id)
                        .Select(ri => ri.Ingredient.Name)
                        .ToList();
                //include categories
                item.Categories = _mapper.Map<List<CategoryModel>>(_context.Categories.Where(c => c.RecipeId == item.Id).ToList());
            }
            return Ok(result);
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeModel>> GetRecipe(int id)
        {
          if (_context.Recipes == null)
          {
              return NotFound();
          }
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<RecipeModel>(recipe);
            //include ingredients
            result.Ingredients = _context.RecipesIngredients
                    .Include(ri => ri.Ingredient)
                    .Where(ri => ri.RecipeId == result.Id)
                    .Select(ri => ri.Ingredient.Name)
                    .ToList();
            //include categories
            result.Categories = _mapper.Map<List<CategoryModel>>(_context.Categories.Where(c => c.RecipeId == result.Id).ToList());
            //include packages
            result.Packages = _mapper.Map<List<PackageModel>>(_context.Packages.Where(p => p.RecipeId == result.Id));
            return Ok(result);
        }

        [HttpGet("getByAuthor/{id}")]
        public async Task<ActionResult<IEnumerable<RecipeModel>>> GetRecipesByAuthor(int id)
        {
            if (_context.Recipes == null)
            {
                return NotFound();
            }
            var recipes = _context.Recipes.Where(r => r.AuthorId == id);
            var result = _mapper.Map<IEnumerable<RecipeModel>>(recipes);
            return Ok(result);
        }

        // PUT: api/Recipes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, RecipeCreateModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (_context.Recipes == null)
            {
                return NotFound("Recipes is null");
            }
            if(model.AuthorId != null && !_context.Authors.Any(a => a.Id == model.AuthorId))
            {
                return BadRequest("Author not found");
            }
            Recipe recipe = _mapper.Map<Recipe>(model);
            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
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

        // POST: api/Recipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(RecipeCreateModel model)
        {
          if (_context.Recipes == null)
          {
              return Problem("Entity set 'PrecookContext.Recipes'  is null.");
          }
            if (model.AuthorId != null && !_context.Authors.Any(a => a.Id == model.AuthorId))
            {
                return BadRequest("Author not found");
            }
            model.Id = null;
            Recipe recipe = _mapper.Map<Recipe>(model);
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            Recipe result = _context.Recipes.OrderByDescending(x => x.Id).FirstOrDefault();
            return CreatedAtAction("GetRecipe", new { id = result.Id }, result);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            if (_context.Recipes == null)
            {
                return Problem("The Recipes is null!");
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return BadRequest("Recipe not found");
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(int id)
        {
            return (_context.Recipes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
