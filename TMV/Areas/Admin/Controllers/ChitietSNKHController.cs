﻿using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.IO;
using System.Text;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using System.Linq;

namespace TMV.Areas.Admin.Controllers
{
    public class ChitietSNKHController : BaseController
    {
        // GET: Admin/ChitietSNKH
        #region ActionResult
        public ActionResult Index(string searchString, int page = 1)
        {
            var dao = new ChitietSNKHDao();
            var model = dao.ListAllPaging(searchString, page);
            ViewBag.SearchString = searchString;
            return View(model);
        }
       
        public void KhachHang()
        {
            var dao = new ChitietSNKHDao();
            var model = dao.Khachhang();
            ViewBag.Khachhang = model;
            ViewBag.Toatal = model.Count();
        }

       
        public void Noidung()
        {
            OnlineTMV db = new OnlineTMV();
            List<APP_SINHNHAT_KHACHHANG> sn = db.APP_SINHNHAT_KHACHHANG.ToList();
            SelectList sinhnhat = new SelectList(sn, "ID_SINHNHAT", "NOI_DUNG");
            ViewBag.Sinhnhat = sinhnhat;
        }

        public ActionResult Create()
        {
            OnlineTMV db = new OnlineTMV();
            Noidung();
            KhachHang();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(string mkh, int Select)
        {
            APP_CHITIET_SINHNHAT_KHACHHANG model = new APP_CHITIET_SINHNHAT_KHACHHANG();
            model.MA_KHACHHANG = mkh;
            model.ID_SINHNHAT = Select;
            if (ModelState.IsValid)
            {
                if (ChitietSNKHDao.Instance.insert(model))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm khuyến mãi thất bại!");
                }
            }
            Noidung();
            KhachHang();
            return View();
        }
        #endregion

        #region ActionResult
        public JsonResult Delete(int id)
        {
            var result = ChitietSNKHDao.Instance.delete(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(string mkh, int Select)
        {
            int _month = DateTime.Now.Month;
            int _year = DateTime.Now.Year;
            APP_CHITIET_SINHNHAT_KHACHHANG model = new APP_CHITIET_SINHNHAT_KHACHHANG();
            model.MA_KHACHHANG = mkh;
            model.ID_SINHNHAT = Select;
            model.THANG_SINHNHAT = _month;
            model.NAM_SINHNHAT = _year;
            model.MODIFIED = DateTime.Now;
            //Noidung();
            //KhachHang();
            var result = ChitietSNKHDao.Instance.insert(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}