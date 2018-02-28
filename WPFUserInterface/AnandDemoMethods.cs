using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPFUserInterface
{
    public static class AnandDemoMethods
    {
        public static List<string> PrepData()
        {
            List<string> output = new List<string>();

            output.Add("https://www.yahoo.com");
            output.Add("https://www.google.com");
            output.Add("https://www.microsoft.com");
            output.Add("https://www.cnn.com");
            output.Add("https://www.amazon.com");
            output.Add("https://www.facebook.com");
            output.Add("https://www.twitter.com");
            output.Add("https://www.codeproject.com");
            output.Add("https://www.stackoverflow.com");
            output.Add("https://en.wikipedia.org/wiki/.NET_Framework");

            return output;
        }

        public static List<WebsiteDataModel> DownloadSync()
        {
            var output = new List<WebsiteDataModel>();

            foreach (var site in PrepData())
            {
                output.Add(DownloadWebsite(site));
            }

            return output;
        }

        public static async Task<List<WebsiteDataModel>> DownloadAsyn()
        {
            var output = new List<WebsiteDataModel>();

            foreach (var site in PrepData())
            {
              await  Task.Run(() => { output.Add(DownloadWebsite(site)); });              
            }

            return output;
        }

        public static async Task<List<WebsiteDataModel>> DownloadAsynUsingAsync()
        {
            var output = new List<WebsiteDataModel>();

            foreach (var site in PrepData())
            {
                var outputfromSite = await DownloadWebsiteAsync(site);
                output.Add(outputfromSite);
            }

            return output;
        }

        public static async Task<List<WebsiteDataModel>> DownloadAsyncUsingTpl()
        {
            var tasks = new List<Task<WebsiteDataModel>>();

            foreach (var site in PrepData())
            {
                tasks.Add(DownloadWebsiteAsync(site));
            }

            var results = await Task.WhenAll(tasks);
            
            return results.ToList();
        }

        // this is use full because Task.WhenAll(tasks) starts all parallel but dont know when it exactly completes each one. 
        // But Parallel for acheive that

        public static async Task<List<WebsiteDataModel>> DownloadAsyncUsingParallelAndRun()
        {
            var outPut = new List<WebsiteDataModel>();

          await  Task.Run(() =>
            {
                Parallel.ForEach(PrepData(), (site) =>
                {
                    outPut.Add(DownloadWebsite(site));
                });

            });

            return outPut;
        }

        private static async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
        {
            WebProxy p = new WebProxy("10.216.190.6", true);
            p.Credentials = new NetworkCredential("john.reese", "john.reese");
            WebRequest.DefaultWebProxy = p;

            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();
            client.Proxy = p;

            output.WebsiteUrl = websiteURL;
            output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL);

            return output;
        }

        private static WebsiteDataModel DownloadWebsite(string websiteURL)
        {

            WebProxy p = new WebProxy("10.216.190.6", true);
            p.Credentials = new NetworkCredential("john.reese", "john.reese");
            WebRequest.DefaultWebProxy = p;

            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();
            client.Proxy = p;

            output.WebsiteUrl = websiteURL;
            output.WebsiteData = client.DownloadString(websiteURL);

            return output;
        }
    }
}
