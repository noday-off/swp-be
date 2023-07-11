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
    public class OrdersController : ControllerBase
    {
        private readonly PrecookContext _context;
        private readonly IMapper _mapper;

        public OrdersController(PrecookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrders()
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var orders = await _context.Orders.ToListAsync();
            return Ok(_mapper.Map<List<OrderModel>>(orders));
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrder(int id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            OrderModel result = _mapper.Map<OrderModel>(order);
            var items = _mapper.Map<List<OrderItemModel>>(_context.OrderItems.Where(oi => oi.OrderId == result.Id).Include(oi => oi.Package).ToList());
            result.Items = items;
            return Ok(result);
        }

        [HttpGet("GetByUser/{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderByUser(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var orders =  _context.Orders.Select(x => x.UserId == id);

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }
        [HttpGet("GetCartByUser/{id}")]
        public async Task<ActionResult<Order>> GetCartByUser(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order =  _context.Orders.FirstOrDefault(x => x.UserId == id && x.Status == "On-cart");

            if (order == null)
            {
                OrderCreateModel default_cart_order = new OrderCreateModel
                {
                    UserId = id,
                    OrderDate = DateTime.Now,
                    TotalPrice = 0,
                    Status = "On-cart",
                    PaymentMethod = "COD",
                };
                Order default_order = _mapper.Map<Order>(default_cart_order);
                _context.Orders.Add(default_order);
                await _context.SaveChangesAsync();
                order = _context.Orders.FirstOrDefault(x => x.UserId == id && x.Status == "On-cart");
            }
            OrderModel result = _mapper.Map<OrderModel>(order);
            result.Items = _mapper.Map<List<OrderItemModel>>(_context.OrderItems.Where(oi => oi.OrderId == result.Id).ToList());
            return Ok(order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderCreateModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("Id doesn't match!");
            }
            if(_context.Orders == null)
            {
                return Problem("The Orders is null!");
            }
            Order order = _mapper.Map<Order>(model);
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return BadRequest("Order not found!");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("addCart/{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderItemModel model)
        {
            if (id != model.OrderId)
            {
                return BadRequest("Order Id doesn't match!");
            }
            if(!_context.Orders.Any(o => o.Id == id))
            {
                return BadRequest("Order not found!");
            }
            model.Id = null;
            OrderItem orderItem = _mapper.Map<OrderItem>(model);
            _context.OrderItems.Add(orderItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return BadRequest("Order not found!");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderCreateModel model)
        {
            if (_context.Orders == null)
            {
                return Problem("Orders Dbset is null.");
            }
            if(!_context.Users.Any(u => u.Id == model.UserId))
            {
                return BadRequest("User not found!");
            }
            Order order = _mapper.Map<Order>(model);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            Order result = _context.Orders.OrderByDescending(o => o.Id).FirstOrDefault();
            //return Ok();
            return CreatedAtAction("GetOrder", new { id = result.Id }, result);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            if(_context.OrderItems.Any(oi => oi.OrderId == id))
            {
                return BadRequest("Fail to delete order. The order contains item(s).");
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
