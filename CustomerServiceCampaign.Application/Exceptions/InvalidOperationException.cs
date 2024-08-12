using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Application.Exceptions
{
    public class InvalidOperationException : Exception
    {
        public InvalidOperationException(string operation)
            : base($"The operation '{operation}' is invalid or not allowed") { }
    }
}
