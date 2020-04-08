using CornNuggets.DataAccess;
using CornNuggets.DataAccess.Models;
using CornNuggets.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CornNuggets.WebUI.Controllers
{
    public class FormController : Controller
    {
        private readonly CornNuggetsContext _context;
        CornNuggetsRepository repository;
        FormViewModel forms;
        public FormController(CornNuggetsContext context)
        {
            _context = context;
            repository = new CornNuggetsRepository(_context);
        }
        
        public  IActionResult Index()
        {
            forms = new FormViewModel();
            return View(forms);
        }
    }
}