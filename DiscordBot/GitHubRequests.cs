using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public static class GitHubRequests
    {
        private static readonly HttpClient client = new HttpClient();
        private const string URL = "https://github.com/login/oauth/authorize";
        public async static Task<string> AuthorizeAsync(string githubToken)
        {
            string encode = Convert.ToBase64String(Encoding.UTF8.GetBytes(githubToken));
            client.DefaultRequestHeaders.Add("Authorization", encode);
            var response = await client.GetAsync(URL);

            client.Dispose();

            // händer grejer
            var accessToken = "123";
            return accessToken;
        }
    }
}
