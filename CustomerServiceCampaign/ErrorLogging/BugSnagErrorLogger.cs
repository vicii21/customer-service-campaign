using Bugsnag;
using CustomerServiceCampaign.Application.Logging;

namespace CustomerServiceCampaign.API.ErrorLogging
{
    public class BugSnagErrorLogger : Application.Logging.IErrorLogger
    {
        private readonly Bugsnag.IClient _bugsnag;

        public BugSnagErrorLogger(IClient bugsnag)
        {
            _bugsnag = bugsnag;
        }

        public void Log(AppError error)
        {
            _bugsnag.Notify(error.Exception, (report) => {
                report.Event.Metadata.Add("Error", new Dictionary<string, string> {
                    { "user", error.Username },
                    { "erroCode", error.ErrorId.ToString() },
        });
            });
        }
    }
}
