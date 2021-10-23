using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AutomateMail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MailController> _logger;

        public MailController(ILogger<MailController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<Hashtable>  GetAttachmentFile()
        {
            MailReader mailReader = new MailReader("imap.gmail.com", 993, true, "nirbhay.us.2020@gmail.com", "nir@US2020");
            var allUnreadEmailAttachment = mailReader.GetUnreadMails(); 
            foreach (var email in allUnreadEmailAttachment)
            {
                Console.WriteLine(email);
            }
            var attachmentList=Task.Run(()=> allUnreadEmailAttachment).ConfigureAwait(false);
            return await attachmentList;
        }
        [HttpPost]
        [Route("mail/saveattachmentdata")]
        public async Task<Hashtable> SaveAttachmentData([FromBody] Attachment data)
        {
            MailReader mailReader = new MailReader("imap.gmail.com", 993, true, "nirbhay.us.2020@gmail.com", "nir@US2020");
            var allUnreadEmailAttachment = mailReader.GetUnreadMails();
            foreach (var email in allUnreadEmailAttachment)
            {
                Console.WriteLine(email);
            }
            var attachmentList = Task.Run(() => allUnreadEmailAttachment).ConfigureAwait(false);
            return await attachmentList;
        }
    }
}
