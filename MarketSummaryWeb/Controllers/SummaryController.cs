using MarketSummaryWeb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MarketSummaryWeb.Models;

namespace MarketSummaryWeb.Controllers
{
    public class SummaryController : Controller
    {
        // GET: Summary
        public async Task<ActionResult> Index(int? page,string prospectName)
        {
            int pageSize = 10;
            int pageIndex = 1;
            ViewBag.FilterValue = prospectName;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;             
            IDataAccess dataAccess = DataAccess.GetInstance();
            IEnumerable<ProspectMarketSummary> prospectSummaryList = await dataAccess.GetProspectSummaryAsync(prospectName);            
            return View(prospectSummaryList.ToPagedList(pageIndex, pageSize));                 
        }
    }
}