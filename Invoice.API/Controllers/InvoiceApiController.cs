using Invoice.Core.Interfaces;
using Invoice.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceApiController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceApiController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // POST: api/InvoiceApi
        [HttpPost]
        public async Task<IActionResult> Create(Invoices invoice)
        {
            var response = await _invoiceService.CreateAsync(invoice);

            if (!response.IsValid)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // GET: api/InvoiceApi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _invoiceService.GetAllAsync();

            return Ok(response);
        }

        // GET: api/InvoiceApi/3
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _invoiceService.GetByIdAsync(id);

            if (!response.IsValid)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        // PUT: api/InvoiceApi/3
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            Invoices invoice)
        {
            var response =
                await _invoiceService.UpdateAsync(id, invoice);

            if (!response.IsValid)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // DELETE: api/InvoiceApi/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response =
                await _invoiceService.DeleteAsync(id);

            if (!response.IsValid)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}