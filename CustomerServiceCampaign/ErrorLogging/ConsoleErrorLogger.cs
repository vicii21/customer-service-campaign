using CustomerServiceCampaign.Application.Logging;
using System.Text;

namespace CustomerServiceCampaign.API.ErrorLogging
{
    public class ConsoleErrorLogger : IErrorLogger
    {
        public void Log(AppError err)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Error code: " + err.ErrorId.ToString());
            builder.AppendLine("Exception message: " + err.Exception.Message);
            builder.AppendLine("Exception stack trace: ");
            builder.AppendLine(err.Exception.StackTrace);
            Console.WriteLine(builder.ToString());
        }
    }
}
