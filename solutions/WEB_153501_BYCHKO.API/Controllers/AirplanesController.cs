using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153501_BYCHKO.API.Data;
using WEB_153501_BYCHKO.API.Services.ProductService;
using WEB_153501_BYCHKO.Domain.Entities;

namespace WEB_153501_BYCHKO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirplanesController : ControllerBase
    {
        private readonly IProductService _service;

        public AirplanesController(IProductService service)
        {
            _service = service;
        }

        // GET: api/Airplanes
        [HttpGet]
        [Route("")]
        [Route("{category}/pageno{pageno:int}/pagesize{pagesize:int}")]
        [Route("{category}/pageno{pageno:int}")]
        [Route("{category}/pagesize{pagesize:int}")]
        [Route("pageno{pageno:int}/pagesize{pagesize:int}")]
        [Route("pageno{pageno:int}")]
        [Route("{category}")]
        public async Task<ActionResult<IEnumerable<Airplane>>> Getairplanes(string? category = null, int pageNo = 1, int pageSize = 3)
        {
            var responde = await _service.GetProductListAsync(category, pageNo, pageSize);
            if (!responde.Success)
            {
                return NotFound();
            }

            return Ok(responde);

        }

        // GET: api/Airplanes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Airplane>> GetAirplane(int id)
        {
            var response = await _service.GetProductByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }
            var airplane = response.Data;

            if (airplane == null)
            {
                return NotFound();
            }

            //return airplane;
            return Ok(response);
        }

        // PUT: api/Airplanes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAirplane(int id, Airplane airplane)
        {
            if (id != airplane.Id)
            {
                return BadRequest();
            }            

            try
            {
                await _service.UpdateProductAsync(id, airplane);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AirplaneExists(id))
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

        // POST: api/Airplanes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Airplane>> PostAirplane(Airplane airplane)
        {
           await _service.CreateProductAsync(airplane);
            return CreatedAtAction("GetAirplane", new { id = airplane.Id }, airplane);
        }

        // DELETE: api/Airplanes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirplane(int id)
        {
            await _service.DeleteProductAsync(id);

            return NoContent();
        }

        private bool AirplaneExists(int id)
        {
            var response =  _service.GetProductByIdAsync(id).Result;
            if (!response.Success || response.Data == null)
            {
                return false;
            }

            return true;
        }
    }
}
