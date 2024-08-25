using Amana.ControllerHelpers;
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Entities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Filters;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.SEO;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Models.Utilities;
#pragma warning restore CS0234 // The type or namespace name 'Models' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.Repository;
#pragma warning restore CS0234 // The type or namespace name 'Repository' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using Amana.ViewModels;
#pragma warning restore CS0234 // The type or namespace name 'ViewModels' does not exist in the namespace 'Amana' (are you missing an assembly reference?)
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Amana.Areas.Admin.Controllers
{
#pragma warning disable CS0246 // The type or namespace name 'CustomAuthorize' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CustomAuthorizeAttribute' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IsBackend' could not be found (are you missing a using directive or an assembly reference?)
    [CustomAuthorize(IsBackend = true)]
#pragma warning restore CS0246 // The type or namespace name 'IsBackend' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'CustomAuthorizeAttribute' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'CustomAuthorize' could not be found (are you missing a using directive or an assembly reference?)
    public class C_NodeController : ControllerHelper
    {
        #region helpers

#pragma warning disable CS0246 // The type or namespace name 'NodeRepository' could not be found (are you missing a using directive or an assembly reference?)
        private NodeRepository nodeDi;
#pragma warning restore CS0246 // The type or namespace name 'NodeRepository' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'CategoriesRepository' could not be found (are you missing a using directive or an assembly reference?)
        private CategoriesRepository catsDi;
#pragma warning restore CS0246 // The type or namespace name 'CategoriesRepository' could not be found (are you missing a using directive or an assembly reference?)
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            nodeDi = new NodeRepository(LangId, db);
            catsDi = new CategoriesRepository(LangId, db);
            base.OnActionExecuting(filterContext);
        }

        #endregion

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page, int id, int? catId, int? parentId)
        {
            CheckAllPermissions(id);

            C_NodeTypeLoc nodeTypeLoc = nodeDi.SingleNodeTypeLocItem(id);
            ViewBag.NodeTypeLoc = nodeTypeLoc;
            List<C_CategoriesLoc> cats = null;
            if (nodeTypeLoc.C_NodeType.CatVocId != null && !(parentId != null && (id == 15)))
            {
                cats = new CategoriesRepository(LangId, db).GetNestedCategories(nodeTypeLoc.C_NodeType.CatVocId ?? 0);
                if (cats != null)
                    ViewData["CatId"] = new SelectList(cats, "CatId", "Title", "");
            }

            IQueryable<C_NodeLoc> moduleItems = nodeDi.GetItemsLocList(typeId: id, isActive: null, isTranslated: null, catId: catId, parentId: parentId);
            if (parentId != null)
                ViewBag.ParentNode = nodeDi.SingleLocItem(parentId ?? 0);
            if (catId != null)
                ViewBag.CatLoc = catsDi.SingleLocItem(catId ?? 0);

            PageTitle(id);
            ViewBag.LangId = LangId;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = sortOrder == "DateAsc" ? "DateDesc" : "DateAsc";
            ViewBag.TitleSortParam = sortOrder == "TitleAsc" ? "TitleDesc" : "TitleAsc";
            ViewBag.ActiveSortParam = sortOrder == "ActiveAsc" ? "ActiveDesc" : "ActiveAsc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            switch (sortOrder)
            {
                case "TitleDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.Title);
                    break;
                case "TitleAsc":
                    moduleItems = moduleItems.OrderBy(m => m.Title);
                    break;
                case "DateAsc":
                    moduleItems = moduleItems.OrderBy(m => m.C_Node.PostDate);
                    break;
                case "DateDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.C_Node.PostDate);
                    break;
                case "ActiveAsc":
                    moduleItems = moduleItems.OrderBy(m => m.C_Node.IsActive);
                    break;
                case "ActiveDesc":
                    moduleItems = moduleItems.OrderByDescending(m => m.C_Node.IsActive);
                    break;
                default:
                    moduleItems = moduleItems.OrderByDescending(m => m.C_Node.NumOrder).ThenByDescending(m => m.NodeId);
                    break;
            }

            if (!string.IsNullOrEmpty(searchString))
                moduleItems = moduleItems.Where(m => m.Title.ToUpper().Contains(searchString.ToUpper()));

            int pageNumber = (page ?? 1);
            return View(moduleItems.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Activate(int id, int typeId, int? page, int? catId, int? parentId)
        {
            if (!CheckPermission(typeId, EnumPermissions.Update))
                return RedirectToAction("Permission", "Home");

            C_Node entity = nodeDi.SingleItem(id);
            if (entity.IsActive)
                entity.IsActive = false;
            else
                entity.IsActive = true;

            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { id = entity.TypeId, page = page, catId = catId, parentId = parentId });
        }

        [HttpPost]
#pragma warning disable CS0246 // The type or namespace name 'C_NodeLoc' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult UpdateNum(IEnumerable<C_NodeLoc> entity, int typeId, int? page, int? catId, int? parentId)
#pragma warning restore CS0246 // The type or namespace name 'C_NodeLoc' could not be found (are you missing a using directive or an assembly reference?)
        {
            if (!CheckPermission(typeId, EnumPermissions.Update))
                return RedirectToAction("Permission", "Home");

            foreach (C_NodeLoc item in entity)
            {
                C_Node node = nodeDi.SingleItem(item.NodeId);
                node.NumOrder = item.C_Node.NumOrder;
                db.Entry(node).State = EntityState.Modified;
            }

            db.SaveChanges();

            return RedirectToAction("Index", new { id = typeId, page = page, catId = catId, parentId = parentId });
        }

        public ActionResult Create(int? id, bool? isSuccess, int typeId, string command, int? catId, int? parentId)
        {
            if (isSuccess == true)
                ViewBag.Success = true;

            PageTitle(typeId);

            C_NodeType nodeType = (from b in db.C_NodeType where b.TypeId == typeId select b).SingleOrDefault();
            ViewBag.NodeType = nodeType;

            List<C_CategoriesLoc> cats = null;
            if (nodeType.CatVocId != null && !(parentId != null && (typeId == 15)))
                cats = new CategoriesRepository(LangId, db).GetNestedCategories(nodeType.CatVocId ?? 0);

            if (id == null)
            {
                if (cats != null)
                    ViewData["Categories"] = new SelectList(cats, "CatId", "Title", catId);
                return View();
            }
            else
            {
                C_Node entity = nodeDi.SingleItem(id ?? 0);
                C_NodeLoc entityLoc = entity.C_NodeLoc.SingleOrDefault(x => x.LanguageId == LangId);
                C_NodeViewModel entityVM = new C_NodeViewModel();
                entityVM.CatId = entity.CatId;
                entityVM.ImageUrl = entity.ImageUrl;
                entityVM.HeaderImageUrl = entity.HeaderImageUrl;
                entityVM.NodeId = entity.NodeId;
                entityVM.PostDate = entity.PostDate;
                entityVM.TypeId = entity.TypeId;
                entityVM.UserId = GetUserId();
                entityVM.IsActive = entity.IsActive;
                entityVM.LinkUrl = entity.LinkUrl;
                entityVM.DateCustom = entity.DateCustom;
                entityVM.IsHome = entity.IsHome;
                entityVM.ParentId = entity.ParentId;
                entityVM.Latitude = entity.Latitude;
                entityVM.Longitude = entity.Longitude;
                entityVM.TubeId = entity.TubeId;
                entityVM.TubeUrl = "https://www.youtube.com/watch?v=" + entity.TubeId;
                if (entityLoc != null)
                {
                    entityVM.Title = entityLoc.Title;
                    entityVM.Title2 = entityLoc.Title2;
                    entityVM.Title3 = entityLoc.Title3;
                    entityVM.Details = entityLoc.Details;
                    entityVM.Brief = entityLoc.Brief;
                    entityVM.LanguageId = entityLoc.LanguageId;
                    entityVM.ImageLocUrl = entityLoc.ImageLocUrl;
                    entityVM.MetaDescription = entityLoc.MetaDescription;
                    entityVM.MetaKeywords = entityLoc.MetaKeywords;
                    entityVM.MetaTitle = entityLoc.MetaTitle;
                    entityVM.PermaLink = entityLoc.PermaLink;
                    entityVM.MetaAlt = entityLoc.MetaAlt;
                    entityVM.Details2 = entityLoc.Details2;
                    entityVM.Brief2 = entityLoc.Brief2;
                    entityVM.LinkLocUrl = entityLoc.LinkLocUrl;
                }

                if (cats != null)
                    ViewData["Categories"] = new SelectList(cats, "CatId", "Title", entityVM.CatId);

                return View(entityVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#pragma warning disable CS0246 // The type or namespace name 'C_NodeViewModel' could not be found (are you missing a using directive or an assembly reference?)
        public ActionResult Create(C_NodeViewModel entityVM, int typeId, HttpPostedFileBase ImageUrl, HttpPostedFileBase HeaderImageUrl, HttpPostedFileBase ImageLocUrl, List<HttpPostedFileBase> ImagesUrl, string command, int? catId, int? parentId)
#pragma warning restore CS0246 // The type or namespace name 'C_NodeViewModel' could not be found (are you missing a using directive or an assembly reference?)
        {
            ModelState.Remove("NodeId"); ModelState.Remove("Latitude"); ModelState.Remove("Longitude");
            try
            {
                if (ModelState.IsValid)
                {
                    entityVM.LanguageId = LangId;
                    C_Node entity = new C_Node
                    {
                        TypeId = typeId,
                        UserId = GetUserId(),
                        CatId = entityVM.CatId,
                        IsActive = entityVM.IsActive,
                        LinkUrl = entityVM.LinkUrl,
                        Latitude = entityVM.Latitude,
                        Longitude = entityVM.Longitude,
                        DateCustom = entityVM.DateCustom,
                        IsHome = entityVM.IsHome,
                        TubeId = HelperMethods.GetYoutubeVideoId(entityVM.TubeUrl),
                        ParentId = parentId
                    };
                    C_NodeLoc entityLoc = new C_NodeLoc
                    {
                        LanguageId = LangId,
                        Title = entityVM.Title,
                        Title2 = entityVM.Title2,
                        Title3 = entityVM.Title3,
                        Brief = entityVM.Brief,
                        Details = entityVM.Details,
                        NodeId = entity.NodeId,
                        IsTranslated = true,
                        MetaDescription = entityVM.MetaDescription,
                        MetaKeywords = entityVM.MetaKeywords,
                        MetaTitle = entityVM.MetaTitle,
                        MetaAlt = entityVM.MetaAlt,
                        PermaLink = SeoUrlHelper.URLFriendly(entityVM.PermaLink),
                        Brief2 = entityVM.Brief2,
                        Details2 = entityVM.Details2,
                        LinkLocUrl = entityVM.LinkLocUrl
                    };
                    if (entityVM.NodeId <= 0)
                    {
                        if (!CheckPermission(typeId, EnumPermissions.Insert))
                            return RedirectToAction("Permission", "Home");

                        entity.PostDate = HelperMethods.GetCurrentDateTime();
                        entity.NumOrder = 0;
                        if (typeId == 1)
                        {
                            entity.ImageUrl = ImageResizer.SaveImage(1800, 900, "Uploads/Node", ImageUrl);
                            entity.HeaderImageUrl = ImageResizer.SaveImage(1800, 900, "Uploads/Node", HeaderImageUrl);
                        }
                        else
                        {
                            entity.ImageUrl = ImageResizer.SaveImage("Uploads/Node", ImageUrl);
                            entity.HeaderImageUrl = ImageResizer.SaveImage("Uploads/Node", HeaderImageUrl);
                        }

                        db.C_Node.Add(entity);

                        List<C_Languages> langs = db.C_Languages.ToList();
                        foreach (var item in langs)
                        {
                            C_NodeLoc LocsList = new C_NodeLoc
                            {
                                LanguageId = item.LanguageId,
                                Title = entityVM.Title,
                                Title2 = entityVM.Title2,
                                Title3 = entityVM.Title3,
                                Brief = entityVM.Brief,
                                Details = entityVM.Details,
                                NodeId = entity.NodeId,
                                IsTranslated = item.LanguageId == LangId ? true : false,
                                MetaDescription = entityVM.MetaDescription,
                                MetaKeywords = entityVM.MetaKeywords,
                                MetaTitle = entityVM.MetaTitle,
                                Brief2 = entityVM.Brief2,
                                Details2 = entityVM.Details2,
                                LinkLocUrl = entityVM.LinkLocUrl
                            };
                            if (item.LanguageId == LangId)
                                LocsList.ImageLocUrl = ImageResizer.SaveImage("Uploads/Node", ImageLocUrl);
                            db.C_NodeLoc.Add(LocsList);
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                        if (!CheckPermission(typeId, EnumPermissions.Update))
                            return RedirectToAction("Permission", "Home");

                        C_Node currentEntity = nodeDi.SingleItem(entityVM.NodeId);
                        C_NodeLoc currentEtityLoc = nodeDi.SingleLocItem(entityVM.NodeId);
                        entity.NodeId = entityLoc.NodeId = entityVM.NodeId;
                        entity.PostDate = currentEntity.PostDate;
                        entity.NumOrder = currentEntity.NumOrder;
                        if (ImageUrl != null)
                        {
                            HelperMethods.DeleteFile("Uploads/Node/", currentEntity.ImageUrl);
                            if (typeId == 1)
                                entity.ImageUrl = ImageResizer.SaveImage(1800, 900, "Uploads/Node", ImageUrl);
                            else
                                entity.ImageUrl = ImageResizer.SaveImage("Uploads/Node", ImageUrl);
                        }
                        else
                            entity.ImageUrl = currentEntity.ImageUrl;

                        if (HeaderImageUrl != null)
                        {
                            HelperMethods.DeleteFile("Uploads/Node/", currentEntity.HeaderImageUrl);
                            if (typeId == 1)
                                entity.HeaderImageUrl = ImageResizer.SaveImage(1800, 900, "Uploads/Node", HeaderImageUrl);
                            else
                                entity.HeaderImageUrl = ImageResizer.SaveImage("Uploads/Node", HeaderImageUrl);
                        }
                        else
                            entity.HeaderImageUrl = currentEntity.HeaderImageUrl;

                        if (ImageLocUrl != null)
                        {
                            HelperMethods.DeleteFile("Uploads/Node/", currentEtityLoc.ImageLocUrl);
                            entityLoc.ImageLocUrl = ImageResizer.SaveImage("Uploads/Node", ImageLocUrl);
                        }
                        else
                            entityLoc.ImageLocUrl = currentEtityLoc.ImageLocUrl;

                        db.Entry(currentEntity).CurrentValues.SetValues(entity);
                        if (currentEtityLoc != null)
                            db.Entry(currentEtityLoc).CurrentValues.SetValues(entityLoc);
                        else
                            db.C_NodeLoc.Add(entityLoc);
                        db.SaveChanges();
                    }


                    if (command == Amana.GlobalResources.Cpanel.Save)
                        return RedirectToAction("Create", new { id = entity.NodeId, typeId = typeId, isSuccess = true, catId = catId, parentId = parentId });
                    else
                        return RedirectToAction("Create", new { id = "", typeId = typeId, isSuccess = true, catId = catId, parentId = parentId });
                }
                return View("UcCustomError", "_Layout", ValidationErrors());
            }
            catch (Exception ex)
            {
                return View("UcCustomError", "_Layout", ex.Message);
            }
        }

        public ActionResult Delete(int? id, int? page, int? catId, int? parentId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C_NodeLoc entityLoc = nodeDi.SingleLocItem(id.Value);
            if (entityLoc != null)
                return View(entityLoc);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? page, int? catId, int? parentId)
        {
            C_Node entity = nodeDi.SingleItem(id);

            if (!CheckPermission(entity.TypeId, EnumPermissions.Delete))
                return RedirectToAction("Permission", "Home");

            try
            {
                List<ImagePathesHelper> imgsPaths = nodeDi.DeleteItem(entity);
                //foreach (var item in imgsPaths)
                //    HelperMethods.DeleteFile(item.FoldersPath, item.FileName);
                return RedirectToAction("Index", new { id = entity.TypeId, page = page, catId = catId, parentId = parentId });
            }
            catch
            {
                C_NodeLoc c = nodeDi.SingleLocItem(id);
                ViewBag.Error = true;
                return View(c);
            }
        }



    }
}