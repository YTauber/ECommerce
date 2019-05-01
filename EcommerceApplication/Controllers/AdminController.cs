using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceApplication.Models;
using Manager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceApplication.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IHostingEnvironment _environment;
        private string _connectionString;

        public AdminController(IHostingEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            IndexViewModel vm = new IndexViewModel();
            DBManager mgr = new DBManager(_connectionString);

            vm.Categoties = mgr.GetCategories();
            vm.Products = mgr.GetAllProducts();

            return View(vm);
        }

        [HttpPost]
        public IActionResult AddProduct(Products product, IFormFile File)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(File.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "UploadedImages", fileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                File.CopyTo(stream);
            }

            product.PictureName = fullPath;
            DBManager mgr = new DBManager(_connectionString);

            mgr.InsertProduct(product);

            return Redirect("/admin/index");
        }

        [HttpPost]
        public IActionResult AddCategory(Categoties category)
        {
            DBManager mgr = new DBManager(_connectionString);
            mgr.InsertCategory(category);
            return Redirect("/admin/index");
        }

        [HttpPost]
        public IActionResult UpdateCategory(Categoties category)
        {
            DBManager mgr = new DBManager(_connectionString);
            mgr.UpdateCategory(category);
            return Redirect("/admin/index");
        }
    }
}