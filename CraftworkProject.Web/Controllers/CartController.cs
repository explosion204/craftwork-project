using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CraftworkProject.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IDataManager _dataManager;
        
        public CartController(IDataManager dataManager, IUserManager userManager)
        {
            _dataManager = dataManager;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            var cartStr = HttpContext.Session.Keys.Contains("cart") ?
                HttpContext.Session.GetString("cart") :
                "[]";

            var jArray = JArray.Parse(cartStr!);
            var makeOrderAllowed = false;

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindUserByName(User.Identity.Name);

                if (jArray.Count != 0 && user.PhoneNumberConfirmed)
                {
                    makeOrderAllowed = true;
                }
            }

            var (products, quantities) = RetrieveCart(jArray);
            var viewModel = new CartViewModel
            {
                Products = products,
                Quantities = quantities,
                MakeOrderAllowed = makeOrderAllowed
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SetCart(string jArray)
        {
            var cartStr = HttpContext.Session.Keys.Contains("cart") ?
                HttpContext.Session.GetString("cart") :
                "[]";

            var cart = JArray.Parse(cartStr!);
            cart.Merge(JArray.Parse(jArray));
            HttpContext.Session.SetString("cart", cart.ToString());

            return Json(new {success = true});
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MakeOrder()
        {
            var cartStr = HttpContext.Session.Keys.Contains("cart") ?
                HttpContext.Session.GetString("cart") :
                "[]";
            var jArray = JArray.Parse(cartStr);
            var (products, quantities) = RetrieveCart(jArray);

            var newOrder = new Order
            {
                Created = DateTime.UtcNow,
                User = await _userManager.FindUserByName(User.Identity.Name)
            };
            var newOrderId = _dataManager.OrderRepository.SaveEntity(newOrder);

            foreach (var product in products)
            {
                var newPurchaseDetail = new PurchaseDetail
                {
                    OrderId = newOrderId,
                    Product = product,
                    Amount = quantities[products.IndexOf(product)]
                };

                _dataManager.PurchaseDetailRepository.SaveEntity(newPurchaseDetail);
            }
            
            HttpContext.Session.SetString("cart", "[]");
            return Json(new {success = true});
        }

        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string itemId)
        {
            var cartStr = HttpContext.Session.Keys.Contains("cart") ?
                HttpContext.Session.GetString("cart") :
                "[]";

            var cart = JArray.Parse(cartStr);
            JToken itemToRemove;

            do
            {
                itemToRemove = cart.FirstOrDefault(x => x.ToString() == itemId);
                itemToRemove?.Remove();
            } while (itemToRemove != null);
            
            HttpContext.Session.SetString("cart", cart.ToString());
            return Json(new {success = true});
        }

        private (List<Product>, List<int>) RetrieveCart(JArray jArray)
        {
            var idWithQuantityDict = new Dictionary<string, int>();
            var products = new List<Product>();
            var quantities = new List<int>();
            
            foreach (var jProductId in jArray)
            {
                var productId = jProductId.ToString();
                
                if (idWithQuantityDict.Keys.Contains(productId))
                {
                    idWithQuantityDict[productId]++;
                }
                else
                {
                    idWithQuantityDict[productId] = 1;
                }
            }

            foreach (var (productId, productQuantity) in idWithQuantityDict)
            {
                products.Add(_dataManager.ProductRepository.GetEntity(Guid.Parse(productId)));
                quantities.Add(productQuantity);
            }

            return (products, quantities);
        }
    }
}