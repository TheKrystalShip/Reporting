namespace Inquisition.Reporting
{
    public enum EmailProvider
    {
        /// <summary>
        /// Host: smtp.gmail.com, Port: 
        /// </summary>
        Google,

        /// <summary>
        /// Host: , Port: 
        /// </summary>
        Microsoft,

        /// <summary>
        /// Use SetHostAndPort method in EmailServiceBuilder
        /// to specify host and port manually
        /// </summary>
        Manual
    }
}
