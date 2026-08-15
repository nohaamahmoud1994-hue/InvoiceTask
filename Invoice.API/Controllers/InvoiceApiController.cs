using Invoice.Core.Interfaces;
using Invoice.Core.Models;
using Microsoft.AspNetCore.Http;
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
        [HttpPost("validate")]
        public IActionResult ValidateInvoice([FromBody] Invoices invoice)
        {
            var result = _invoiceService.ValidateInvoice(invoice);

            return Ok(result);
        }
    }
}
