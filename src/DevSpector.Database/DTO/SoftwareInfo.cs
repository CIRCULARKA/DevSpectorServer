namespace DevSpector.Database.DTO
{
    /// <summary>
    /// This is the DTO object needed to provide information from HTTP request to controller's action methods
    /// Defines contract between clients and server
    /// </summary>
    public class SoftwareInfo
    {
        public string SoftwareName { get; set; }

        public string SoftwareVersion { get; set; }
    }
}
