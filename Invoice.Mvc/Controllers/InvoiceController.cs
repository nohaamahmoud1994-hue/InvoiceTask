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

        // GET: /Invoice
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceApiService.GetAllAsync();

            return View(invoices);
        }

        // GET: /Invoice/Create
        [HttpGet]
        public IActionResult Create()
        {
            var invoice = new Invoices
            {
                DateTimeIssued = DateTime.UtcNow,
                InvoiceLines = new()
            };

            return View(invoice);
        }

        // POST: /Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invoices invoice)
        {
            invoice.InvoiceLines ??= new();

            if (!ModelState.IsValid)
            {
                return View(invoice);
            }

            try
            {
                var result = await _invoiceApiService.CreateAsync(invoice);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    return View(invoice);
                }

                TempData["SuccessMessage"] = result.Message;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    "Error occurred: " + ex.Message);

                return View(invoice);
            }
        }

        // GET: /Invoice/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _invoiceApiService.GetByIdAsync(id);

            if (invoice == null)
            {
                TempData["ErrorMessage"] =
                    $"Invoice with Id {id} was not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(invoice);
        }

        // GET: /Invoice/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var invoice = await _invoiceApiService.GetByIdAsync(id);

            if (invoice == null)
            {
                TempData["ErrorMessage"] =
                    $"Invoice with Id {id} was not found.";

                return RedirectToAction(nameof(Index));
            }

            invoice.InvoiceLines ??= new();

            return View(invoice);
        }

        // POST: /Invoice/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit( int id,Invoices invoice)
        {
            invoice.InvoiceLines ??= new();

            // The route ID is the source of truth
            invoice.Id = id;

            if (!ModelState.IsValid)
            {
                return View(invoice);
            }

            try
            {
                var result =
                    await _invoiceApiService.UpdateAsync(id, invoice);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    return View(invoice);
                }

                TempData["SuccessMessage"] = result.Message;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    "Error occurred: " + ex.Message);

                return View(invoice);
            }
        }

        // GET: /Invoice/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _invoiceApiService.GetByIdAsync(id);

            if (invoice == null)
            {
                TempData["ErrorMessage"] =
                    $"Invoice with Id {id} was not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(invoice);
        }

        // POST: /Invoice/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result =
                    await _invoiceApiService.DeleteAsync(id);

                if (!result.IsValid)
                {
                    TempData["ErrorMessage"] = result.Message;

                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = result.Message;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    "Error occurred: " + ex.Message;

                return RedirectToAction(nameof(Index));
            }
        }
    }
}