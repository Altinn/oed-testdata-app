namespace oed_testdata.Api.Models
{
    public enum StatusNotificationType
    {
        OpenApp
    }

    public class StatusNotification
    {
        public int PartyID { get; set; }
        public DateTime Timestamp { get; set; }
        public StatusNotificationType StatusNotificationType { get; set; }
    }
}
