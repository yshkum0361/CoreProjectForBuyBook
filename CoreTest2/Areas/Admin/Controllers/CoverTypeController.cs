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
    public class CoverTypeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverTypeModel> objCoverType = _unitOfWork.CoverTypeRepository.GetAll();
            return View(objCoverType);
        }
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverTypeModel obj)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverTypeRepository.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = " Cover Type Successfully";
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
            var CoverfromDbFirst = _unitOfWork.CoverTypeRepository.getFirstOrDefault(u => u.cover_Id == id);
            if (CoverfromDbFirst == null)
            {
                return NotFound();
            }
            return View(CoverfromDbFirst);
        }
        [HttpPost]

        public IActionResult Edit(CoverTypeModel obj)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverTypeRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cover Type Updated Successfully";
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
            var CoverFromDB = _unitOfWork.CoverTypeRepository.getFirstOrDefault(u => u.cover_Id == id);

            if (CoverFromDB == null)
            {
                return NotFound();
            }
            return View(CoverFromDB);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(CoverTypeModel mol)
        {

            //var obj = _unitOfWork.CategoryRepository.getFirstOrDefault(u => u.book_Id == mol.book_Id) ;
            // if (obj == null)
            // {
            //     return NotFound();
            // }
            _unitOfWork.CoverTypeRepository.Remove(mol);
            _unitOfWork.Save();
            TempData["success"] = "Cover Type Deleted Successfully";
            return RedirectToAction("Index");

        }
    }
}
