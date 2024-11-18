using BullkyBook.DataAccess;
using BullkyBook.DataAccess.Repository;
using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using BullkyBook.Model.ViewModels;
using BullkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;

namespace CoreTest2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitofWork, IWebHostEnvironment webHostEnvironment)
        {
            this._unitOfWork = unitofWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Upsert(int? pro_id)
        {
            ProductVM productVM = new()
            {
                product = new(),
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.book_Name,
                    Value = i.book_Id.ToString()
                }),

                coverTypeList = _unitOfWork.CoverTypeRepository.GetAll().Select(i => new SelectListItem
                {
                    Text = i.cover_Name,
                    Value = i.cover_Id.ToString()
                }),
            };


            if (pro_id == null || pro_id == 0)
            {
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = coverTypeList;
                return View(productVM);
            }
            else
            {
                productVM.product = _unitOfWork.ProductRepository.getFirstOrDefault(u => u.pro_id == pro_id);
                    return View(productVM);
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                //---------------------------------Image Upload code----------------------------------------------------------------
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(wwwRootPath, @"Images\ProductImg");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.product.ImageURL != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageURL.Trim('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.product.ImageURL = @"\Images\ProductImg\" + fileName + extension;
                }
                if (obj.product.pro_id == 0)
                {
                    _unitOfWork.ProductRepository.Add(obj.product);
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(obj.product);
                }
               
                _unitOfWork.Save();
                TempData["success"] = "Product Added Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }




        #region API CALLS

       
        [HttpGet]


        public IActionResult GetAll()
        {
            var productList = _unitOfWork.ProductRepository.GetAll(includeProperties: "category,coverType");
            return Json(new { data = productList });
        }

        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {

            var obj = _unitOfWork.ProductRepository.getFirstOrDefault(u => u.pro_id == id);
            if (obj == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageURL.Trim('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.ProductRepository.Remove(obj);
            _unitOfWork.Save();

            return Json(new { success = true, Message = "Delete Successful" });

        }
        #endregion
    }
}
