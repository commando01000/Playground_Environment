using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Repository.Layer.Specification.TicketSpecs;
using Services.Layer.Dtos;
using Services.Layer.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Playground_Environment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: api/<TicketController>
        [HttpGet]
        public async Task<OkObjectResult> GetWithSpecs([FromQuery] TicketSpecification input)
        {
            var tickets = await _ticketService.GetTicketsAsync(input);
            return Ok(new { success = true, data = tickets, message = "Success" });
        }

        // GET: api/<TicketController>
        [HttpGet]
        public async Task<OkObjectResult> GetPaginatedWithSpecs([FromQuery] TicketSpecification input)
        {
            var tickets = await _ticketService.GetTicketsAsync(input);
            return Ok(new { success = true, data = tickets, message = "Success" });
        }

        // GET api/<TicketController>/5
        [HttpGet("{id}")]
        public async Task<OkObjectResult> Get(string id)
        {
            var ticket = await _ticketService.GetTicketAsync(id);

            return Ok((new
            {
                success = true,
                data = ticket,
                message = "Success"
            }));
        }

        // POST api/<TicketController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TicketController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TicketController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
