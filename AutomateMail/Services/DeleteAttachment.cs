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

namespace AutomateMail
{

    public static class DeleteAttachment
    {

        public static void Delete()
        {
            try
            {
                var sourceDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory).Split("\\bin")[0];
                string[] xlsxList = Directory.GetFiles(sourceDir, "*.xlsx");
                // Delete source files that were copied.
                foreach (string f in xlsxList)
                {
                    File.Delete(f);
                }

            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
        }
    }
}
