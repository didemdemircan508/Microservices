using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Messages
{
    public class SendMailMessageCommand
    {
        public string Email { get; set; }

        public string Message { get; set; }
    }
}
