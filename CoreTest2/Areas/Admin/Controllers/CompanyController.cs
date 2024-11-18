using BullkyBook.DataAccess;
using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using BullkyBook.Model.ViewModels;
using BullkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CoreTest2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CompanyController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;

        }
        public IActionResult Index()
        {

            return View();
        }



        public IActionResult Upsert(int? comp_Id)
        {
            CompanyModel company = new();
            if (comp_Id == null || comp_Id == 0)
            {
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = coverTypeList;
                return View(company);
            }
            else
            {
                company = _unitOfWork.CompanyRepository.getFirstOrDefault(u => u.Comp_Id == comp_Id);
                return View(company);
            }

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CompanyModel obj)
        {

            if (ModelState.IsValid)
            {
                if (obj.Comp_Id == 0)
                {
                    _unitOfWork.CompanyRepository.Add(obj);
                    TempData["success"] = "Company Saved Successfully";
                }
                else
                {
                    _unitOfWork.CompanyRepository.Update(obj);
                    TempData["success"] = "Company Updated Successfully";
                }
                
                _unitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        #region API CALLS


        [HttpGet]


        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            return Json(new { data = companyList });
        }

        [HttpDelete]

        public IActionResult Delete(int? id)
        {

            var obj = _unitOfWork.CompanyRepository.getFirstOrDefault(u => u.Comp_Id == id);
            if (obj == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

           

            _unitOfWork.CompanyRepository.Remove(obj);
            _unitOfWork.Save();

            return Json(new { success = true, Message = "Delete Successful" });

        }
        #endregion
    }
}
