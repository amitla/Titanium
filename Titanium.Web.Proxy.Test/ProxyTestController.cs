using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Helpers;
using System.IO;

namespace Titanium.Web.Proxy.Test
{
    public partial class ProxyTestController
    {

        public int ListeningPort { get; set; }
        public bool EnableSSL { get; set; }
        public bool SetAsSystemProxy { get; set; }
        public string BlockedURLsRegexListFilePath { get; set; } = "storage";
        private List<BO.BlackListRecord> mRegexBlackList = new List<BO.BlackListRecord>();

        public void StartProxy()
        {
            ProxyServer.BeforeRequest += OnRequest;
            ProxyServer.BeforeResponse += OnResponse;
            ProxyServer.EnableSSL = EnableSSL;
            ProxyServer.SetAsSystemProxy = SetAsSystemProxy;
            ProxyServer.ListeningPort = this.ListeningPort;

            //Exclude Https addresses you don't want to proxy
            //Usefull for clients that use certificate pinning
            //for example dropbox.com
            //ProxyServer.ExcludedHttpsHostNameRegex.Add(".dropbox.com");
            //ProxyServer.RedirectToSaftyDomainsList.Add(".apple.com");
            //ProxyServer.RedirectToSaftyDomainsList.Add(".ynet.co.il");

            //if (!string.IsNullOrEmpty(BlockedURLsRegexListFilePath))
            //{
            //    LoadBlockedList();
            //}

            ProxyServer.Start();

            Console.WriteLine(String.Format("Proxy listening on local machine port: {0} ", ProxyServer.ListeningPort));

        }

        private void LoadBlockedList()
        {
            Console.Write("Would you like to (1)Add black list records/ (2)Display the list/ (N)Skip? ");
            var response = Console.ReadLine();
            if (response == "1")
            {
                //AddBlackListRecords();
            }
            else if (response == "2")
            {
                ShowCurrentRecords();
            }



            //if (File.Exists(this.BlockedURLsRegexListFilePath))
            //{
            //    string line = null;
            //    using (var fileStream = File.OpenRead(this.BlockedURLsRegexListFilePath))
            //    {
            //        using (var file = new StreamReader(fileStream))
            //        {
            //            while((line = file.ReadLine()) != null)
            //            {
            //                line.sp
            //            }
            //        }
            //    }
            //}
        }

        //private void ShowCurrentRecords()
        //{
        //    this.mRegexBlackList = Dal.GetAllBlacklistRecords();
        //    if (this.mRegexBlackList != null)
        //    {
        //        for (int i = 0; i < this.mRegexBlackList.Count; i++)
        //        {
        //            Console.WriteLine(
        //                "ID: {0} | Regex: {1}, ReplacementHTML: {2}",
        //                this.mRegexBlackList[i].Id,
        //                this.mRegexBlackList[i].Regex,
        //                string.IsNullOrEmpty(this.mRegexBlackList[i].ReplacementHTML) ? "default html" : this.mRegexBlackList[i].ReplacementHTML );
        //        }
        //    }
        //}

        //private void AddBlackListRecords()
        //{
        //    BO.BlackListRecord newRecord = new BO.BlackListRecord();

        //    Console.Write("Enter Regex Pattern: ");
        //    newRecord.Regex = Console.ReadLine();

        //    Console.Write("Replacement HTML (leave empty for default): ");
        //    newRecord.ReplacementHTML = Console.ReadLine();

        //    Dal.AddBlacklistRecord(newRecord);
        //    Console.WriteLine("Save Successful");

        //    Console.WriteLine("Would you like to add another record (Y/N)? ");
        //    if (Console.ReadLine().Trim().ToLower() == "y")
        //    {
        //        AddBlackListRecords();
        //    }
        //    else
        //    {
        //        Console.WriteLine("Would you like to view the records (Y/N)? ");
        //        if (Console.ReadLine().Trim().ToLower() == "y")
        //        {
        //            ShowCurrentRecords();
        //        }
        //    }
        //}

        public void Stop()
        {
            ProxyServer.BeforeRequest -= OnRequest;
            ProxyServer.BeforeResponse -= OnResponse;

            ProxyServer.Stop();
        }




        //Test On Request, intecept requests
        //Read browser URL send back to proxy by the injection script in OnResponse event
        public void OnRequest(object sender, SessionEventArgs e)
        {

            Console.WriteLine(e.RequestURL);

            //read request headers
            var requestHeaders = e.RequestHeaders;

            //if ((e.RequestMethod.ToUpper() == "POST" || e.RequestMethod.ToUpper() == "PUT"))
            //{
            //    //Get/Set request body bytes
            //    byte[] bodyBytes = e.GetRequestBody();
            //    e.SetRequestBody(bodyBytes);

            //    //Get/Set request body as string
            //    string bodyString = e.GetRequestBodyAsString();
            //    e.SetRequestBodyString(bodyString);

            //}

            ////To cancel a request with a custom HTML content
            ////Filter URL
            var record = this.mRegexBlackList.Where(x => Regex.IsMatch(e.RequestURL, x.Regex)).FirstOrDefault();

            if (record != null)
            {
                if(string.IsNullOrEmpty(record.ReplacementHTML))
                {
                    e.Ok("<!DOCTYPE html><html><body><h1>Website Blocked</h1><p>URL " + e.RequestURL + " Blocked by Amit's simple proxy.</p></body></html>");
                }
                else
                {
                    e.Ok(record.ReplacementHTML);
                }
                
            }

        }

        //Test script injection
        //Insert script to read the Browser URL and send it back to proxy
        public void OnResponse(object sender, SessionEventArgs e)
        {
            ////read response headers
            //var responseHeaders = e.ResponseHeaders;


            //if (e.ResponseStatusCode == HttpStatusCode.OK)
            //{
            //    if (e.ResponseContentType.Trim().ToLower().Contains("text/html"))
            //    {
            //        //Get/Set response body bytes
            //        byte[] responseBodyBytes = e.GetResponseBody();
            //        e.SetResponseBody(responseBodyBytes);

            //        //Get response body as string
            //        string responseBody = e.GetResponseBodyAsString();

            //        //Modify e.ServerResponse
            //        Regex rex = new Regex("</body>", RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //        string modified = rex.Replace(responseBody, "<script type =\"text/javascript\">alert('Response was modified by this script!');</script></body>", 1);

            //        //Set modifed response Html Body
            //        e.SetResponseBodyString(modified);
            //    }
            //}

        }

    }

}
