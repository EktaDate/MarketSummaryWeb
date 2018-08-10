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

namespace MarketSummaryWeb.Controllers
{
    public class HomeController : Controller
    {
        [ActionName("Index")]
        public  ActionResult Index()
        {            
            SQLRepository sqldal = new SQLRepository();
            List<ProspectSearchCriteria> listProspects =  sqldal.GetProspectData();
            return View(listProspects);
        }        

        [HttpPost]
        [ActionName("CreateProspectSearchCriteria")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProspectSearchCriteria prospectSearchCriteria)
        {
            if (ModelState.IsValid)
            {
                SQLRepository sqldal = new SQLRepository();
                sqldal.InsertProspects(prospectSearchCriteria);
                return RedirectToAction("Index");
            }
            return View(prospectSearchCriteria);
        }

        [ActionName("CreateProspectSearchCriteria")]
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ActionName("EditProspectSearchCriteria")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProspectSearchCriteria prospectSearchCriteria)
        {
            if (ModelState.IsValid)
            {
                SQLRepository sqldal = new SQLRepository();
                sqldal.UpdateProspects(prospectSearchCriteria);
                return RedirectToAction("Index");
            }

            return View(prospectSearchCriteria);
        }

        [ActionName("EditProspectSearchCriteria")]
        public ActionResult Edit(string id, string category)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SQLRepository sqldal = new SQLRepository();
            string WhereClause = " Where id = " + id;
            List<ProspectSearchCriteria> listProspects = sqldal.GetProspectData(WhereClause);            

            return View(listProspects.FirstOrDefault());
        }

        [ActionName("DeleteProspectSearchCriteria")]
        public ActionResult Delete(string id, string category)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SQLRepository sqldal = new SQLRepository();
            string WhereClause = " Where id = " + id;
            List<ProspectSearchCriteria> listProspects = sqldal.GetProspectData(WhereClause);

            return View(listProspects.FirstOrDefault());
        }                
        [HttpPost]
        [ActionName("DeleteProspectSearchCriteria")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            SQLRepository sqldal = new SQLRepository();
            sqldal.DeleteProspects(id);
            return RedirectToAction("Index");        
        }

    }
}