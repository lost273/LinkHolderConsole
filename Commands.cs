using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace LinkHolderConsole {
    internal abstract class Commands {
        public abstract String Run(String token);
        public const string APP_PATH= "http://localhost:5000/";
        public HttpClient CreateClient(string accessToken = "") {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(accessToken)) {
                client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }

    }
    internal sealed class Login : Commands {
        public override String Run(String token) {
            //Console.WriteLine("Enter the email:");
            String userName = "admin@example.com";
            //Console.WriteLine("Enter the password:");
            String password = "Secret123$";
            return GetTokenDictionary(userName, password)["access_token"];
        }
        static Dictionary<string, string> GetTokenDictionary(string userName, string password) {
            var pairs = new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>( "grant_type", "password" ), 
                        new KeyValuePair<string, string>( "username", userName ), 
                        new KeyValuePair<string, string> ( "Password", password )
            };
            var content = new FormUrlEncodedContent(pairs);
    
            using (var client = new HttpClient()) {
                var response =
                    client.PostAsync(APP_PATH + "api/account/token", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary;
            }
        }
    }
    internal sealed class Register : Commands {
        public override String Run(String token) {
            return "OK";
        }
    }
    internal sealed class Value : Commands {
        public override String Run(String token) {
            using (var client = CreateClient(token)) {
                var response = client.GetAsync(APP_PATH + "api/roleadmin").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
     internal sealed class Exit : Commands {
        public override String Run(String token) {
           return "exit";
        }
    }
}