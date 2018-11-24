using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DomainModel
{
    public class Mayor
    {
        public int CityId { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime BirthTime { get; set; }

       public City City { get; set; }
    }
}
