using System;


namespace BSSiseveeb.Public.Web.Models
{
    public class VacationModel : BaseViewModel
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Comment { get; set; }
    }

    public class GeneralIdModel : BaseViewModel
    {
        public int Id { get; set; }
    }

    public class RequestModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Info { get; set; }
    }

    public class GeneratePdfModel : BaseViewModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Status { get; set; }
    }
}