using Amana.ControllerHelpers;
using Amana.Models.Entities;
using Amana.Models.Filters;
using Amana.Repository;
using Amana.Repository.Repository.Classifications;
using Amana.Repository.Repository.Setting;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Amana.Areas.Admin.Controllers
{
    [CustomAuthorize(IsBackend = true)]
    public class ClassificationsCategoryController : ControllerHelper
    {
        readonly AmanaConcreteDBEntities _context = new AmanaConcreteDBEntities();
        // GET: Admin/ClassificationsCategory
        public ActionResult Index( string sortOrder, string currentFilter, int? page)
        {
            var query = ClassificationsCategoryRepository.GetItems(5);

            //var Data= _context.ConClsCategories.GroupBy(a => a.SuperCategory).Select(g => new { g.Key, SuperCategoryName = g.Max(x => x.SuperCategoryName) }).ToList();
            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageNumber, PageSize));
        }

       



    }
}