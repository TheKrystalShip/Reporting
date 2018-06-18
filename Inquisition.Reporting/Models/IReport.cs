using System;

namespace Inquisition.Reporting.Models
{
    /// <summary>
    /// Any implementation of this class needs to have one parameterless constructor
    /// in order for it to be serialized by the Reporter.
    /// It can have more than constructor, but one of them needs to be empty.
    /// </summary>
    public interface IReport
    {
        Guid Guid { get; set; }
        string ErrorMessage { get; set; }
        string StackTrace { get; set; }
        string Path { get; set; }
    }
}
