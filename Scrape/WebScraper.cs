using System;

namespace Utility
{
    public class WebScraper
    {
        private string URL { get; set; }
        public string Content { get; private set; }
        public WebScraper(string url)
        {
            this.URL = url;
            this.ReadContentAsync().Wait();
        }

        public async System.Threading.Tasks.Task ReadContentAsync()
        {
            var client = new System.Net.Http.HttpClient();
            string x = await client.GetStringAsync(this.URL);
            this.Content = x;
        }   
    }
}
