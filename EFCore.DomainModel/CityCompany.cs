using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DomainModel
{
    public class CityCompany
    {
        //CityId 和 CompnayId作联合主键
       

        public int CityId { get; set; }
        public City City { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        
    }
}
