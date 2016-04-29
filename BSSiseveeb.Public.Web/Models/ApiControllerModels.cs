using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BSSiseveeb.Public.Web.Models
{
    public class ApiControllerModels
    {
        public class VacationModel
        {
            public int Id { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public string Comment { get; set; }
        }

        public class GeneralIdModel
        {
            public int Id { get; set; }
        }

        public class RequestModel
        {
            public string Title { get; set; }
            public string Info { get; set; }
        }


    }
}