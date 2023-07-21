namespace BlogApp.Core.Utilities.Concrete
{
    public class MessageHelpers
    {
        public string FormatMessage(string area, string controller, string action, string message, string linkText)
        {
            string link = $"<a asp-area=\"{area}\" asp-controller=\"{controller}\" asp-action=\"{action}\">{linkText}</a>";
            string formatMessage = message.Replace(linkText, link);

            return formatMessage;
        }

    }
}
