using AutomateMail.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMMC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutomateMail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {


        private readonly ILogger<MailController> _logger;

        public MailController(ILogger<MailController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<Hashtable> GetAttachmentFile()
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
        [HttpPost]
        [Route("saveattachmentdata")]
        public async Task<Response> SaveAttachmentData()
        {
            string result = string.Empty;
            Response response = new Response();
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string str = await reader.ReadToEndAsync();
                response.status = OracleDatabase.SaveAttachmentData(str);
            }
            DeleteAttachment.Delete();
            var responseResult = Task.Run(() => response).ConfigureAwait(false);
            return await responseResult;
        }

    }
}
