using Schedular.Models;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedular.Interface
{
    public interface IMaillingService
    {

        Task<Response> SendMail(GridEmailRequest request);
    }
}
