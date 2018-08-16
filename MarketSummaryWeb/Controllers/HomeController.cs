using MarketSummaryWeb.Models;
using MarketSummaryWeb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;

namespace MarketSummaryWeb.Controllers
{
    public class HomeController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            IDataAccess dataAccess = DataAccess.GetInstance();
            return View(await dataAccess.GetProspectSearchCriteriaAsync());
        }

        [HttpPost]
        [ActionName("CreateProspectSearchCriteria")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(ProspectSearchCriteria prospectSearchCriteria)
        {
            IDataAccess dataAccess = DataAccess.GetInstance();
            Expression<Func<ProspectSearchCriteria, bool>> predicate = (p => p.ProspectName.ToLower() == prospectSearchCriteria.ProspectName.ToLower() 
                                                                        && p.SearchString.ToLower() == prospectSearchCriteria.SearchString.ToLower() 
                                                                        && p.Id != prospectSearchCriteria.Id);

            ProspectSearchCriteria existingProspectSearchCriteria = await dataAccess.GetProspectSearchCriteriaAsync(predicate);
            if (existingProspectSearchCriteria != null)
            {
                ModelState.AddModelError("SearchString", "Combination of Prospect Name and Search String already exist.");
            }

            if (ModelState.IsValid)
            {
                await dataAccess.InsertProspectSearchData(prospectSearchCriteria);
                return RedirectToAction("Index");
            }
            return View(prospectSearchCriteria);
        }

        [ActionName("CreateProspectSearchCriteria")]
        public async Task<ActionResult> CreateAsync()
        {
            return View();
        }

        [HttpPost]
        [ActionName("EditProspectSearchCriteria")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ProspectSearchCriteria prospectSearchCriteria)
        {
            IDataAccess dataAccess = DataAccess.GetInstance();
            Expression<Func<ProspectSearchCriteria, bool>> predicate = (p => p.ProspectName.ToLower() == prospectSearchCriteria.ProspectName.ToLower() 
                                                                        && p.SearchString.ToLower() == prospectSearchCriteria.SearchString.ToLower() 
                                                                        && p.Id != prospectSearchCriteria.Id);

            ProspectSearchCriteria existingProspectSearchCriteria = await dataAccess.GetProspectSearchCriteriaAsync(predicate);
            if (existingProspectSearchCriteria != null)
            {
                ModelState.AddModelError("SearchString", "Combination of Prospect Name and Search String already exist.");
            }
            if (ModelState.IsValid)
            {
                await dataAccess.UpdateProspectSearchData(prospectSearchCriteria);
                return RedirectToAction("Index");
            }
            return View(prospectSearchCriteria);
        }

        [ActionName("EditProspectSearchCriteria")]
        public async Task<ActionResult> EditViewAsync(int id)
        {
            IDataAccess dataAccess = DataAccess.GetInstance();
            ProspectSearchCriteria prospectSearchCriteria = await dataAccess.GetProspectSearchCriteriaAsync(id);

            if (prospectSearchCriteria == null)
            {
                return HttpNotFound();
            }
            return View(prospectSearchCriteria);
        }

        [ActionName("DeleteProspectSearchCriteria")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            IDataAccess dataAccess = DataAccess.GetInstance();
            ProspectSearchCriteria prospectSearchCriteria = await dataAccess.GetProspectSearchCriteriaAsync(id);

            if (prospectSearchCriteria == null)
            {
                return HttpNotFound();
            }
            return View(prospectSearchCriteria);
        }
        [HttpPost]
        [ActionName("DeleteProspectSearchCriteria")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteViewAsync(int id)
        {
            IDataAccess dataAccess = DataAccess.GetInstance();
            ProspectSearchCriteria prospectSearchCriteria = await dataAccess.GetProspectSearchCriteriaAsync(id);
            if (prospectSearchCriteria == null)
            {
                return HttpNotFound();
            }

            await dataAccess.DeleteProspectSearchData(prospectSearchCriteria);
            return RedirectToAction("Index");
        }

        [ActionName("ProspectReport")]
        public ActionResult RunReport()
        {

            string url = ConfigurationManager.AppSettings["functionurl"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json";
            Stream stream = req.GetRequestStream();
            string json = "{\"name\": \"Azure\" }";
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            stream.Write(buffer, 0, buffer.Length);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            return RedirectToAction("Index");
        }

    }
}