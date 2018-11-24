using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DomainModel
{
    public class Company
    {
        public Company()
        {
            CityCompanies = new List<CityCompany>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Establishdate { get; set; }
        public string LegalPerson { get; set; }


        public List<CityCompany> CityCompanies { get; set; }
    }
}
