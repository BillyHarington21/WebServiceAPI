using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public ProductCategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetCategories()
        {
            return await _context.ProductCategories.Include(p => p).ToListAsync();

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetCategory(int id)
        {
            var category = await _context.ProductCategories.FirstOrDefaultAsync(p => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        [HttpPost]
        public async Task<ActionResult<ProductCategory>> CreateCategory(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = productCategory.Id }, productCategory);
        }

        [HttpPut]
        public async Task<ActionResult<ProductCategory>> UpdateCategory(int id, ProductCategory productCategory)
        {
            if (id != productCategory.Id)
            {
                return BadRequest();
            }
            _context.Entry(productCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync(); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductCategory>> DeleteCategory(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
