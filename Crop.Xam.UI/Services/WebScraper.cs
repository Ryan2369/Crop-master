using System;

namespace Crop.Xam.Utility
{
    public class WebScraper
    {
        private string URL { get; set; }
        public string Content { get; private set; }
        public WebScraper(string url)
        {
            this.URL = url;
            this.ReadContentAsync();
        }

        public void ReadContentAsync()
        {
            // var client = new System.Net.Http.HttpClient();
            var uri = new System.Uri(this.URL);
            var client = new System.Net.HttpWebRequest(uri);
            var response = client.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new System.IO.StreamReader(stream);
            this.Content = reader.ReadToEnd();
        }
    }
}