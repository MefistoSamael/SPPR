using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineTypeCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EngineTypeCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EngineTypeCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EngineTypeCategory>>> GetengineTypes()
        {
          if (_context.engineTypes == null)
          {
              return NotFound();
          }
            return await _context.engineTypes.ToListAsync();
        }

        // GET: api/EngineTypeCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EngineTypeCategory>> GetEngineTypeCategory(int id)
        {
          if (_context.engineTypes == null)
          {
              return NotFound();
          }
            var engineTypeCategory = await _context.engineTypes.FindAsync(id);

            if (engineTypeCategory == null)
            {
                return NotFound();
            }

            return engineTypeCategory;
        }

        // PUT: api/EngineTypeCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEngineTypeCategory(int id, EngineTypeCategory engineTypeCategory)
        {
            if (id != engineTypeCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(engineTypeCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EngineTypeCategoryExists(id))
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

        // POST: api/EngineTypeCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EngineTypeCategory>> PostEngineTypeCategory(EngineTypeCategory engineTypeCategory)
        {
          if (_context.engineTypes == null)
          {
              return Problem("Entity set 'AppDbContext.engineTypes'  is null.");
          }
            _context.engineTypes.Add(engineTypeCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEngineTypeCategory", new { id = engineTypeCategory.Id }, engineTypeCategory);
        }

        // DELETE: api/EngineTypeCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEngineTypeCategory(int id)
        {
            if (_context.engineTypes == null)
            {
                return NotFound();
            }
            var engineTypeCategory = await _context.engineTypes.FindAsync(id);
            if (engineTypeCategory == null)
            {
                return NotFound();
            }

            _context.engineTypes.Remove(engineTypeCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EngineTypeCategoryExists(int id)
        {
            return (_context.engineTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
