﻿using System;
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
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly PrecookContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(PrecookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorModel>>> GetAuthors()
        {
            try
            {
                if (_context.Authors == null)
                {
                    return NotFound();
                }
                var authors = _context.Authors.ToList();
                IEnumerable<AuthorModel> result = _mapper.Map<IEnumerable<AuthorModel>>(authors);
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorModel>> GetAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);
            AuthorModel result = _mapper.Map<AuthorModel>(author);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if(_context.Authors == null)
            {
                return NotFound("Authors is null");
            }
             Author author = _mapper.Map<Author>(model);            
            _context.Entry(author).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Author>> PostAuthor([FromBody] AuthorModel author)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'PrecookContext.Authors'  is null.");
            }
            author.Id = null;
            Author new_author = _mapper.Map<AuthorModel, Author>(author);
            _context.Authors.Add(new_author);
            await _context.SaveChangesAsync();
            Author result = _context.Authors.OrderByDescending(x => x.Id).FirstOrDefault();
            return CreatedAtAction("GetAuthor", new { id = result.Id }, result);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
