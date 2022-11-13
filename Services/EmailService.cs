using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Services
{
    public class EmailService : IBlogEmailSender
    {
        public Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
