using BullkyBook.DataAccess;
using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using BullkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreTest2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }
        public IActionResult Index()
        {

            IEnumerable<BullkyBookModel> objCategorylist = _unitOfWork.CategoryRepository.GetAll();
            return View(objCategorylist);
        }

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BullkyBookModel obj)
        {
            if (obj.book_Name == obj.display_Order.ToString())
            {
                ModelState.AddModelError("CustomError", "The DisplayOrder Cannot Exactly match the name.");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDB = _unitOfWork.CategoryRepository.bullkyBookModels.Find(id);
            var categoryfromDbFirst = _unitOfWork.CategoryRepository.getFirstOrDefault(u => u.book_Id == id);
            if (categoryfromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryfromDbFirst);
        }
        [HttpPost]

        public IActionResult Edit(BullkyBookModel obj)
        {
            if (obj.book_Name == obj.display_Order.ToString())
            {
                ModelState.AddModelError("CustomError", "The DisplayOrder Cannot Exactly match the name.");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDB = _unitOfWork.CategoryRepository.getFirstOrDefault(u => u.book_Id == id);

            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(BullkyBookModel mol)
        {

            //var obj = _unitOfWork.CategoryRepository.getFirstOrDefault(u => u.book_Id == mol.book_Id) ;
            // if (obj == null)
            // {
            //     return NotFound();
            // }
            _unitOfWork.CategoryRepository.Remove(mol);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");

        }
    }
}
