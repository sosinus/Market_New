using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Tables;
using Service;

namespace Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService service)
        {
            _customerService = service;
        }

        [HttpGet]        
        public IActionResult GetCustomer()
        {
            string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            Customer result = _customerService.GetCustomer(userId);
            if (result != null)
            {
                return Ok(result);
            }
            else return Ok();
        }

        [HttpPost]
        [Route("Customer")]
        public IActionResult AddCustomer(FrontCustomer frontCustomer)
        {
            string userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            var result = _customerService.CreateCustomer(frontCustomer, userId);
            if (result.succeeded)
                return Ok();
            else return BadRequest();
        }

        [HttpGet]
        [Route("GetDiscount")]
        [Authorize(Roles = "User")]
        public IActionResult GetDiscount()
        {
            var userId = User.Claims.SingleOrDefault(c => c.Type == "UserID").Value;
            var discount = _customerService.GetDiscount(userId);
            return Ok(discount);
        }
    }
}