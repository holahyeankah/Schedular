using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedular.Interface
{
    public interface IMaillingService
    {
        void SendMail(string message, string email);
    }
}
