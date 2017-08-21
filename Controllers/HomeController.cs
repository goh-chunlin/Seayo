using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Seayo.Data;
using Seayo.Models;
using Seayo.Models.FileViewModels;
using Seayo.Services;

namespace Seayo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly IFileProcessor _fileProcessor;

        public HomeController(
            IConfiguration configuration, 
            ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager, 
            IFileProcessor fileProcessor)
        {
            _configuration = configuration;
            _db = db;
            _userManager = userManager;
            _fileProcessor = fileProcessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("UploadFile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFileAsync(FileUploadViewModel model)
        {
            if  (!ModelState.IsValid)
            {
                string errorMessages = String.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)) + ". ";

                TempData["ActionMessage"] = $"[FAILED] The image { model.File.FileName } cannot be uploaded. { errorMessages }Please try again.";
            
            } else {

                var newFileUpload = await _fileProcessor.InsertFileRecordToDatabaseWithImageUploadingAsync(
                    model.File, 
                    _configuration["ConnectionString:AzureStorage"],
                    "uploaded-photos",
                    await GetCurrentUserAsync());

                await _db.FileUploads.AddAsync(newFileUpload);

                await _db.SaveChangesAsync();
                
                TempData["ActionMessage"] = $"The image { model.File.FileName } has been uploaded.";

            }

            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()  
        {  
            return await _userManager.GetUserAsync(HttpContext.User);  
        } 
    }
}
