using System;

namespace Inquisition.Reporting.Models
{
    /// <summary>
    /// Default implementation of IReport interface
    /// </summary>
    public class Report : IReport
    {
        public Guid Guid { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string Path { get; set; }

        /// <summary>
        /// Default constructor, necessary for serialization
        /// </summary>
        public Report()
        {

        }

        /// <summary>
        /// Automatic binding from Exception object
        /// </summary>
        /// <param name="e">Exception</param>
        public Report(Exception e)
        {
            Guid = Guid.NewGuid();
            ErrorMessage = e.Message;
            StackTrace = e.StackTrace;
        }
    }
}
