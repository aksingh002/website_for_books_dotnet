using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotNet_webApplication.Models.ViewModel;
using dotNet_webApplication.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace dotNet_webApplication.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _Productrepo;
        public ProductController(IUnitOfWork db)
        {
            _Productrepo=db;
        }
        
        public IActionResult Index()
        {
            List<Product> objProductlist = _Productrepo.Product.Getall().ToList();
            return View(objProductlist);
        }

        public IActionResult Create(){
            ProductVM productVM = new()
            {
                CategoryList = _Productrepo.Category.Getall().Select(u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               }),
                Product = new Product()
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(Product obj){
            if(ModelState.IsValid){
                _Productrepo.Product.Add(obj);
                _Productrepo.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id){
             if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _Productrepo.Product.Get(u => u.Id == id);
            //Product? productFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Product? productFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj){
            if(ModelState.IsValid){
                _Productrepo.Product.Update(obj);
                _Productrepo.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id){
            if(id==null || id==0){
                return NotFound();

            }
            Product? ProductFromDb = _Productrepo.Product.Get(u=>u.Id==id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id){
            Product? ProductFromDb =_Productrepo.Product.Get(u=>u.Id==id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }
            _Productrepo.Product.Remove(ProductFromDb);
            _Productrepo.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
           
        }

        
    }
}