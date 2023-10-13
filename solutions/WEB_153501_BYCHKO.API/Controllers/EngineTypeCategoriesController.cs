using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.API.Services.CategoryService;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineTypeCategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public EngineTypeCategoriesController(ICategoryService svc)
        {
            _service = svc;
        }

        // GET: api/EngineTypeCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EngineTypeCategory>>> GetEngineTypes()
        {
            var categories = (await _service.GetCategoryListAsync());
          if (categories == null)
          {
              return NotFound();
          }
            return Ok(categories);
        }

        // GET: api/EngineTypeCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EngineTypeCategory>> GetEngineTypeCategory(int id)
        {
            var categories = (await _service.GetCategoryListAsync()).Data;
            if (categories == null)
          {
              return NotFound();
          }
            var engineTypeCategory = categories.Where(c => c.Id == id).FirstOrDefault();

            if (engineTypeCategory == null)
            {
                return NotFound();
            }

            return engineTypeCategory;
        }

        //// PUT: api/EngineTypeCategories/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEngineTypeCategory(int id, EngineTypeCategory engineTypeCategory)
        //{
        //    if (id != engineTypeCategory.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _service.Entry(engineTypeCategory).State = EntityState.Modified;

        //    try
        //    {
        //        await _service.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EngineTypeCategoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/EngineTypeCategories
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<EngineTypeCategory>> PostEngineTypeCategory(EngineTypeCategory engineTypeCategory)
        //{
        //  if (_service.engineTypes == null)
        //  {
        //      return Problem("Entity set 'AppDbContext.engineTypes'  is null.");
        //  }
        //    _service.engineTypes.Add(engineTypeCategory);
        //    await _service.SaveChangesAsync();

        //    return CreatedAtAction("GetEngineTypeCategory", new { id = engineTypeCategory.Id }, engineTypeCategory);
        //}

        //// DELETE: api/EngineTypeCategories/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEngineTypeCategory(int id)
        //{
        //    if (_service.engineTypes == null)
        //    {
        //        return NotFound();
        //    }
        //    var engineTypeCategory = await _service.engineTypes.FindAsync(id);
        //    if (engineTypeCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    _service.engineTypes.Remove(engineTypeCategory);
        //    await _service.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool EngineTypeCategoryExists(int id)
        //{
        //    return (_service.engineTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
