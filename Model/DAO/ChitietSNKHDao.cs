﻿using Model.EF;
using Model.DAO;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Activities.Expressions;

namespace Model.DAO
{
    public class ChitietSNKHDao
    {
        /**
      * Constants
      */
        private OnlineTMV db = null;

        /**
         * @description -- init
         */

        public ChitietSNKHDao()
        {
            db = new OnlineTMV();
        }

        private static ChitietSNKHDao instance;

        public static ChitietSNKHDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChitietSNKHDao();
                }
                return instance;
            }
        }


        /**
         * @description -- get Promotion by ProdID
         * @param _key: int -- is field ProdID
         */

        public APP_CHITIET_SINHNHAT_KHACHHANG getByID(int _key)
        {
            return db.APP_CHITIET_SINHNHAT_KHACHHANG.SingleOrDefault(obj => obj.ID_CHITIET_SINHNHAT == _key);
        }

        /**
         * @description -- insert a product
         * @param _request: Promotion -- entity object
         */

        public bool hasProcuct(APP_CHITIET_SINHNHAT_KHACHHANG _pro)
        {
            var chitiet = (from p in db.APP_CHITIET_SINHNHAT_KHACHHANG
                          where p.MA_KHACHHANG.Equals(_pro.MA_KHACHHANG)
                          && (p.THANG_SINHNHAT.Value.Equals(_pro.THANG_SINHNHAT.Value))
                           && (p.NAM_SINHNHAT.Value.Equals(_pro.NAM_SINHNHAT.Value))
                          select p).ToList();
            if (chitiet != null && chitiet.Count > 0)
                return true;
            else
                return false;
        }

        public bool insert(APP_CHITIET_SINHNHAT_KHACHHANG _request)
        {
            if (!hasProcuct(_request))
            {
                db.APP_CHITIET_SINHNHAT_KHACHHANG.Add(_request);
                
                db.SaveChanges();
                return true;
            }
            return false;
        }
        /**
         * @description -- delete a product
         * @param _key: int -- is field ProdID
         */

        public bool delete(int _key)
        {
            //if (hasReference(_key))
            //    return false;
            db.APP_CHITIET_SINHNHAT_KHACHHANG.Remove(getByID(_key));
            db.SaveChanges();
            return true;
        }

        /**
         * @description -- update info product has image
         * @param _request: PromotionRequestDto -- is the data transmitted down from the display screen
         */

        public bool Update(APP_CHITIET_SINHNHAT_KHACHHANG _request)
        {
            var chitiet = getByID(_request.ID_CHITIET_SINHNHAT);
            chitiet.ID_SINHNHAT = _request.ID_SINHNHAT;
            chitiet.MA_KHACHHANG = _request.MA_KHACHHANG;
            db.SaveChanges();
            return true;
        }


        public IEnumerable<APP_CHITIET_SINHNHAT_KHACHHANG> ListAllPaging(string searchString, int page) 
        {
            var _month = DateTime.Now.Month;
            var _year = DateTime.Now.Year;
            IQueryable<APP_CHITIET_SINHNHAT_KHACHHANG> model = db.APP_CHITIET_SINHNHAT_KHACHHANG;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.MA_KHACHHANG.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ID_CHITIET_SINHNHAT).Where((x=>(x.THANG_SINHNHAT == _month)&&(x.NAM_SINHNHAT == _year))).ToPagedList(page, Constants.PageSize);

        }
        public List<TBL_KHACHHANG> Khachhang()
        {
            int _month = DateTime.Now.Month;
            int _year = DateTime.Now.Year;
            var _dasinhnhat = db.APP_CHITIET_SINHNHAT_KHACHHANG.Where(p => p.THANG_SINHNHAT.Value == _month
            && p.NAM_SINHNHAT.Value == _year).ToList();
            List<string> list = new List<string>();
            if (_dasinhnhat != null && _dasinhnhat.Count > 0)
            {
                foreach (var lst in _dasinhnhat)
                {
                    list.Add(lst.MA_KHACHHANG);
                }
            }
            List<TBL_KHACHHANG> kh = new List<TBL_KHACHHANG>();
            if (list != null && list.Count > 0)
                kh = (from item in db.TBL_KHACHHANG
                      where (!(list.Contains(item.MA_KHACHHANG)))
                      && (item.NGAY_SINH.Value.Month.Equals(_month))
                      select item).ToList();
            else
                kh = (from item in db.TBL_KHACHHANG
                      where (item.NGAY_SINH.Value.Month.Equals(_month))
                      select item).ToList();
            return kh;


        }



        //public IEnumerable<DM_DICHVU> ListAllPaging(string searchString, int page)
        //{
        //    IQueryable<DM_DICHVU> model = db.DM_DICHVU;
        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        model = model.Where(x => x.TEN_DICHVU.Contains(searchString) || x.MOTA_CHITIET.Contains(searchString));
        //    }
        //    return model.OrderByDescending(x => x.MA_DICHVU).ToPagedList(page, Constants.PageSize);

        //}

        //if(list !=null && list.Count>0)
        //var model = from kh in db.TBL_KHACHHANG
        //            where  kh.BIRTHDAY == null && kh.NGAY_SINH.Value.Month == DateTimeOffset.Now.Month 
        //            select kh;
        //return model.ToList();


        //public IEnumerable<TBL_KHACHHANG> Khachhang(int page)
        //{
        //    var model = from kh in db.TBL_KHACHHANG
        //            where  kh.NGAY_SINH.Value.Month == DateTimeOffset.Now.Month 
        //            select kh;
        //return model.ToList();
        //}

        /**
         * @description -- get products list by search key
         * @param _search: string -- is search key
         */

        public IEnumerable<APP_CHITIET_SINHNHAT_KHACHHANG> getObjectList(string _search, int page, out int totalRows, out int totalPages)
        {
            var model = db.APP_CHITIET_SINHNHAT_KHACHHANG.OrderBy(p => p.ID_CHITIET_SINHNHAT).ToList();

            if (_search != null)
            {
                model = model.Where(obj => obj.MA_KHACHHANG.Contains(_search)).ToList();
            }

            totalRows = model.Count();
            totalPages = (int)Math.Ceiling((double)(totalRows / Constants.PageSize));

            return model.Skip((page - 1) * Constants.PageSize)
                        .Take(Constants.PageSize);
        }

        public List<string> ListName(string keyword)
        {
            return db.APP_CHITIET_SINHNHAT_KHACHHANG.Where(x => x.MA_KHACHHANG.Contains(keyword)).Select(x => x.MA_KHACHHANG).ToList();
        }
        public List<APP_CHITIET_SINHNHAT_KHACHHANG> Search(string search_kw, ref int totalRecord, int pageIndex = 1)
        {
            var model = db.APP_CHITIET_SINHNHAT_KHACHHANG.Where(x => x.MA_KHACHHANG.Contains(search_kw)).ToList();
            totalRecord = db.APP_CHITIET_SINHNHAT_KHACHHANG.Where(x => x.MA_KHACHHANG.Contains(search_kw)).Count();//nghi nó bằng 0 chỗ này
            model = model.Skip((pageIndex - 1) * Constants.PageSize).Take(Constants.PageSize).ToList();
            return model;
        }
    }
}
