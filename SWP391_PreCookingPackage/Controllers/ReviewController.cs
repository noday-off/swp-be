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
    }
}
