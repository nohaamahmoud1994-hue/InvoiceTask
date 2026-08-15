using Invoice.Core.Models;
using Invoice.Mvc.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.Mvc.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceApiService _invoiceApiService;

        public InvoiceController(IInvoiceApiService invoiceApiService)
        {
            _invoiceApiService = invoiceApiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Invoices
            {
                DateTimeIssued = DateTime.Now,
                InvoiceLines=new()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(Invoices invoice)
        {

          
            if (!ModelState.IsValid)
            {

                return View(invoice);
            }

            try
            {

                var result = await _invoiceApiService.ValidateInvoiceAsync(invoice);

                ViewBag.Result = result;
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError("", "ErrorAccoured: " + ex.Message);
            }
            return View(invoice);
        }
    }
}
