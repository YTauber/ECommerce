using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Manager;
using System.Data.Sql;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace EcommerceApplication.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _environment;
        private string _connectionString;

        public HomeController(IHostingEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }



        public IActionResult Index(int? CategoryId)
        {
            int id = CategoryId ?? 1;
            IndexViewModel vm = new IndexViewModel();
            DBManager mgr = new DBManager(_connectionString);

            vm.Categoties = mgr.GetCategories();
            vm.Products = mgr.GetProductsByCategoryId(id);
            vm.CurrentCategory = id;
            return View(vm);
        }



        public IActionResult ItemView(int ProductId)
        {
            ItemViewModel vm = new ItemViewModel();
            DBManager mgr = new DBManager(_connectionString);

            vm.Product = mgr.GetProductById(ProductId);

            return View(vm);
        }

        public IActionResult ViewCart()
        {
            DBManager mgr = new DBManager(_connectionString);

            Carts cart = HttpContext.Session.Get<Carts>("Cart");
            if (cart == null)
            {
                return Redirect("/home/index");
            }
            
            CartViewModel vm = new CartViewModel();
            vm.CartItems = mgr.GetProductsByCartId(cart.Id);
            return View(vm);
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogInCheck(string password)
        {
            DBManager mgr = new DBManager(_connectionString);
            bool good = mgr.CheckPassword(password);
            if (good)
            {
                var claims = new List<Claim>
                {
                    new Claim("user", password)
                };
                HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

                return Redirect("/admin/index");
            }
            else
            {
                return Redirect("/home/login");
            }
        }

        [HttpPost]
        public void AddCart(CartItems cartItems)
        {
            DBManager mgr = new DBManager(_connectionString);

            Carts cart = HttpContext.Session.Get<Carts>("Cart");

            if (cart == null)
            {
                cart = new Carts();
                cart.Date = DateTime.Now;
                mgr.InsertCart(cart);

                HttpContext.Session.Set("Cart", cart);
            }

            cartItems.CartId = cart.Id;
            mgr.InsertCartItem(cartItems);

            
        }


        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }

}
