using MarketSummaryWeb.Models;
using MarketSummaryWeb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MarketSummaryWeb.Controllers
{
    public class HomeController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var items = await CosmosRepository<ProspectSearchCriteria>.GetProspectsAsync(d=>d.Id != null);
            return View(items);
        }

        [HttpPost]
        [ActionName("CreateProspectSearchCriteria")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,ProspectName,SearchCriteria")] ProspectSearchCriteria prospectSearchCriteria)
        {
            if (ModelState.IsValid)
            {
                await CosmosRepository<ProspectSearchCriteria>.CreateSearchDataAsync(prospectSearchCriteria);
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
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,ProspectName,SearchCriteria")] ProspectSearchCriteria prospectSearchCriteria)
        {
            if (ModelState.IsValid)
            {
                await CosmosRepository<ProspectSearchCriteria>.UpdateItemAsync(prospectSearchCriteria.Id, prospectSearchCriteria);
                return RedirectToAction("Index");
            }

            return View(prospectSearchCriteria);
        }

        [ActionName("EditProspectSearchCriteria")]
        public async Task<ActionResult> EditAsync(string id, string category)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProspectSearchCriteria prospectSearchCriteria = await CosmosRepository<ProspectSearchCriteria>.GetProspectsAsync(id);
            if (prospectSearchCriteria == null)
            {
                return HttpNotFound();
            }

            return View(prospectSearchCriteria);
        }

        [ActionName("DeleteProspectSearchCriteria")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProspectSearchCriteria prospectSearchCriteria = await CosmosRepository<ProspectSearchCriteria>.GetProspectsAsync(id);
            if (prospectSearchCriteria == null)
            {
                return HttpNotFound();
            }

            return View(prospectSearchCriteria);
        }

        [HttpPost]
        [ActionName("DeleteProspectSearchCriteria")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id,")] string id)
        {
            await CosmosRepository<ProspectSearchCriteria>.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

    }
}