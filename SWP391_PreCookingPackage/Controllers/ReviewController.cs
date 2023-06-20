using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_PreCookingPackage.Models;
using SWP391_PreCookingPackage.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWP391_PreCookingPackage.Models;
using SWP391_PreCookingPackage.ModelsDTO;

namespace SWP391_PreCookingPackage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly PrecookContext _context;
        private readonly IMapper _mapper;

        public ReviewController(PrecookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/REview
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReview()
        {
            try
            {
                if (_context.Reviews == null)
                {
                    return NotFound();
                }
                var reviews = _context.Reviews.ToList();    
                IEnumerable<Review> result = _mapper.Map<IEnumerable<Review>>(reviews);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        // GET: api/Review/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorModel>> GetReview(int id)
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }
            var reviews = await _context.Reviews.FindAsync(id);
            AuthorModel result = _mapper.Map<AuthorModel>(reviews);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }
        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview([FromBody] Review review)
        {
            if (_context.Reviews == null)
            {
                return Problem("Entity set 'PrecookContext.Reviews'  is null.");
            }
            Review new_review = _mapper.Map<Review>(review);
            //Author new_author = new Author()
            //{
            //    Id = author.Id,
            //    Fullname = author.Fullname,
            //    Email = author.Email,
            //    Contact = author.Contact,
            //    Recipes = null
            //};
            _context.Reviews.Add(new_review);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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
        private bool ReviewExists(int id)
        {
            return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
