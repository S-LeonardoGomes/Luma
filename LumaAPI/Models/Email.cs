namespace LumaEventService.Models
{
    public class Email
    {
        public string Subject { get; set; }
        public string TextContent { get; set; }
        public string HtmlContent { get; set; }
        public string Recipient { get; set; }
        public string RecipientUserName { get; set; }
        public long SendAtUTC { get; set; }
    }
}
