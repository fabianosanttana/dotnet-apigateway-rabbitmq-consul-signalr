using System.Threading.Tasks;
using ApiProdutos.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiprodutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET api/values
        [Authorize(AuthenticationSchemes = "Aut")]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            return Ok(await _context.Products.FindAsync(id));
        }

        // POST api/values
        [Authorize(AuthenticationSchemes = "Aut")]
        [HttpPost]
        public async Task<ActionResult> Post(Product value)
        {
            await _context.Products.AddAsync(value);
            await _context.SaveChangesAsync();
            return Ok("Produto cadastrado com sucesso");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
