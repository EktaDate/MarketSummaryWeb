using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MarketSummaryWeb.Models;

namespace MarketSummaryWeb.Repository
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProspectDataSearchCriteria, ProspectSearchCriteria>();            
        }
    }
}