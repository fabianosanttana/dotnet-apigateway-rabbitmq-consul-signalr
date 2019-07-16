using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APIClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // GET api/values
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Jorge Luis", "James Li" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<object> Get(int id)
        {
            return new { Id = id, Message = $"Teste Agregação - {id}"};
        }
    }
}
