using System;
using System.Collections.Generic;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using System.Linq;
using ExcelDataReader;
using System.Collections;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;

namespace AutomateMail
{
    
    public class MailReader
    {
        private readonly string mailServer, login, password;
        private readonly int port;
        private readonly bool ssl;
        public MailReader(string mailServer, int port, bool ssl, string login, string password)
        {
            this.mailServer = mailServer;
            this.port = port;
            this.ssl = ssl;
            this.login = login;
            this.password = password;
        }
        public IEnumerable<string> GetAllMails()
        {
            var messages = new List<string>();

            using (var client = new ImapClient())
            {
                try
                {
                    client.Connect(mailServer, port, ssl);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(login, password);

                    // The Inbox folder is always available on all IMAP servers...
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);
                    var results = inbox.Search(SearchOptions.All, SearchQuery.NotSeen);
                    foreach (var uniqueId in results.UniqueIds)
                    {
                        var message = inbox.GetMessage(uniqueId);

                        messages.Add(message.HtmlBody);

                        //Mark message as read
                        inbox.AddFlags(uniqueId, MessageFlags.Seen, true);

                        foreach (var attachment in message.Attachments)
                        {
                            var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                            using (var stream = File.Create(fileName)) //--please choose your own filepath
                            {
                                if (attachment is MessagePart)
                                {
                                    var rfc822 = (MessagePart)attachment;
                                    rfc822.Message.WriteTo(stream);
                                }
                                else
                                {
                                    var part = (MimePart)attachment;
                                    part.Content.DecodeTo(stream);
                                }
                            }
                        }
                    }

                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return messages;
        }

        public Hashtable GetUnreadMails()
        {
            Hashtable eachRow = new Hashtable();

            using (var client = new ImapClient())
            {
                try
                {
                    client.Connect(mailServer, port, ssl);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(login, password);

                    // The Inbox folder is always available on all IMAP servers...
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);
                    var results = inbox.Search(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
                    foreach (var uniqueId in results.UniqueIds)
                    {
                        var message = inbox.GetMessage(uniqueId);
                         
                        foreach (var attachment in message.Attachments)
                        {
                            var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                            if (fileName.Contains(".xlsx")) {
                                using (var stream = File.Create(fileName)) //--please choose your own filepath
                                {
                                    if (attachment is MessagePart)
                                    {
                                        var rfc822 = (MessagePart)attachment;
                                        rfc822.Message.WriteTo(stream);
                                    }
                                    else
                                    {
                                        var part = (MimePart)attachment;
                                        part.Content.DecodeTo(stream);
                                    }
                                }
                               
                            }

                            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite))
                            {
                                using (var reader = ExcelReaderFactory.CreateReader(stream))
                                {
 
                                    do
                                    {
                                        int row = 0;
                                        while (reader.Read()) //Each ROW
                                        {
                                            var QuoteVal = "";
                                            row++;
                                            for (int column = 0; column < reader.FieldCount; column++)
                                            {
                                                if (!string.IsNullOrWhiteSpace((string)reader.GetValue(column)))
                                                {
                                                    QuoteVal += reader.FieldCount - 1 > column ? reader.GetValue(column) + ";" : reader.GetValue(column);
                                                    //Console.WriteLine(reader.GetString(column));//Will blow up if the value is decimal etc. 
                                                    Console.WriteLine(reader.GetValue(column));//Get Value returns object
                                                }
                                            }
                                            eachRow.Add("Quote"+ row, QuoteVal);
                                        }
                                    } while (reader.NextResult()); //Move to NEXT SHEET

                                }
                            }

                        }
                        //Mark message as read
                        inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                    }

                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return eachRow;
        }
        
    }
}
