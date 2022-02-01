using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;
using PagedList;
using PagedList.Mvc;
using System.Web.Security;
using System.Web.Routing;

namespace AlmaCMS.Areas.Manage.Controllers
{
  
    public class ManageQuestionsController : Controller
    {
    private    DB_AlmaCmsEntities db;
    private QuestionRepository repQuestions;
    public static bool premission = false;
        // GET: Manage/ManageQuestions
        public ManageQuestionsController()
    {
        db = new DB_AlmaCmsEntities();
        repQuestions = new QuestionRepository(db);
    }
        public ActionResult Index()
        {
            var questionList = repQuestions.GetAll().OrderByDescending(c => c.id).ToList();

            var VMList = new List<VMQuestions>();

            foreach (var item in questionList)
            {
                VMList.Add(item.toVMQuestion());
            }

            return View(VMList);
        }

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = repQuestions.FindById((int)id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question.toVMQuestion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( VMQuestions vmQuestion)
        {
            if (ModelState.IsValid)
            {
                repQuestions.Update(vmQuestion.toQuestion());
                Helpers.UserLogHelper.AddLog("پرسش و پاسخ", (User.Identity.Name), "ویرایش", vmQuestion.Title);

                return RedirectToAction("index");
            }
            return View(vmQuestion);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = repQuestions.FindById((int)id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question.toVMQuestion());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CurrentItem = repQuestions.FindById(id);
            //int GroupID = (int)repQuestions.FindById(id).ArticleGroupID;
            repQuestions.Delete(id);
            Helpers.UserLogHelper.AddLog("پرسش و پاسخ", (User.Identity.Name), "حذف", CurrentItem.Title);

            return RedirectToAction("Index");
        }
        #endregion
    }


}