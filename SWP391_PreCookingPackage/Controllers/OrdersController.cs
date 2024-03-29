﻿using System;
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
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrderByUser(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var orders =  _context.Orders.Where(x => x.UserId == id);

            if (orders == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<IEnumerable<OrderModel>>(orders);
            return Ok(result);
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
            var items = _mapper.Map<List<OrderItemModel>>(_context.OrderItems.Where(oi => oi.OrderId == result.Id).Include(oi => oi.Package).ToList());
            result.Items = items;
            return Ok(result);
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
            //var currentOrder = _context.Orders.FirstOrDefault(o => o.Id == id);
            //if (currentOrder == null)
            if (!_context.Orders.Any(o => o.Id == id))
            {
                return BadRequest("Order not found!");
            }
            //if(currentOrder.Status != "Pending" && model.Status == "Completed")
            //{
            //    return BadRequest("The current order status is not pending.");
            //}
            if (model.Status == "Completed")
            {
                string updateError = updateStorage(id);
                if(updateError != null)
                {
                    return BadRequest(updateError);
                }
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

            var order = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
            var package = _context.Packages.FirstOrDefault(p => p.Id == model.PackageId);
            if (package == null)
            {
                return BadRequest("Invalid package!");
            }
            int sale = model.Quantity ?? 0;
            if (package.Quantity < sale)
            {
                return BadRequest($"The package [{package.Title}] has less quantity than the selected quantity.");
            }
            if (package.Quantity <= 0)
            {
                return BadRequest($"The package [{package.Title}] has been already sold out.");
            }

            var existingItem = order.OrderItems.FirstOrDefault(oi => oi.PackageId == model.PackageId);

            if(existingItem != null)
            {
                existingItem.Quantity = model.Quantity;
                existingItem.Price = model.Price;
                _context.Entry(existingItem).State = EntityState.Modified;
            }
            else
            {
                OrderItem orderItem = _mapper.Map<OrderItem>(model);
                _context.OrderItems.Add(orderItem);
            }
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

        [HttpPut("complete/{id}")]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("The Orders is null!");
            }
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if(order == null)
            {
                return BadRequest("Order not found!");
            }
            var items = _context.OrderItems.Where(oi => oi.OrderId == id).ToList();
            if(items == null || !items.Any())
            {
                return BadRequest("Order has no item!");
            }
            //update the quantity of packages in db
            int sale = 0;
            Package package;
            foreach(var item in items)
            {
                package = _context.Packages.FirstOrDefault(p => p.Id == item.PackageId);
                if(package == null)
                {
                    return BadRequest("Order contains a invalid package!");
                }
                Console.WriteLine(item.Quantity);
                sale = item.Quantity ?? 0;
                if(package.Quantity < sale)
                {
                    return BadRequest($"The package {package.Title} has less quantity than the selected quantity.");
                }
                if(package.Quantity <= 0)
                {
                    return BadRequest($"The package [{package.Title}] has been already sold out.");
                }
                package.Sales += sale;
                package.Quantity -= sale;
                _context.Entry(package).State = EntityState.Modified;
            }
            order.ShipDate = DateTime.Now;
            order.Status = "Completed";
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

        private string updateStorage(int id)
        {
            var items = _context.OrderItems.Where(oi => oi.OrderId == id).ToList();
            if (items == null || !items.Any())
            {
                return "Order has no item!";
            }
            //update the quantity of packages in db
            int sale = 0;
            Package package;
            foreach (var item in items)
            {
                package = _context.Packages.FirstOrDefault(p => p.Id == item.PackageId);
                if (package == null)
                {
                    return "Order contains a invalid package!";
                }
                Console.WriteLine(item.Quantity);
                sale = item.Quantity ?? 0;
                if (package.Quantity < sale)
                {
                    return $"The package {package.Title} has less quantity than the selected quantity.";
                }
                if (package.Quantity <= 0)
                {
                    return $"The package [{package.Title}] has been already sold out.";
                }
                //package.Sales += sale;
                package.Quantity -= sale;
                _context.Entry(package).State = EntityState.Modified;
            }
            _context.SaveChanges();
            return null;
        }
    }
}
