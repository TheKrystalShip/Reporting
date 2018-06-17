using System;

namespace Inquisition.Reporting.Models
{
    public class Report : IReport
    {
        public Guid Guid { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string Path { get; set; }
    }
}
