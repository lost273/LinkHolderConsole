using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
        protected void ShowResult(String result){
            Console.WriteLine(result);
        }

    }
    internal sealed class Login : Commands {
        public override String Run(String token) {
            //Console.WriteLine("Enter the email:");
            String userName = "admin@example.com";
            //Console.WriteLine("Enter the password:");
            String password = "Secret123$";
            return GetToken(userName, password);
        }
        private String GetToken(string userName, string password) {
            var pairs = new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>( "grant_type", "password" ), 
                        new KeyValuePair<string, string>( "username", userName ), 
                        new KeyValuePair<string, string> ( "Password", password )
            };
            var content = new FormUrlEncodedContent(pairs);
    
            using (var client = new HttpClient()) {
                var response =
                    client.PostAsync(APP_PATH + "api/account/token", content).Result;

                ShowResult($"Login result -> {response.Content.ReadAsStringAsync().Result}");
                var result = response.Content.ReadAsStringAsync().Result;
                
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary["access_token"];
            }
        }
    }
    internal sealed class Register : Commands {
        public override String Run(String token) {
            CreateUserModel user = new CreateUserModel();
            Console.Write("Name: ");
            user.Name = Console.ReadLine();
            Console.Write("Email: ");
            user.Email = Console.ReadLine();
            Console.Write("Password: ");
            user.Password = Console.ReadLine();
            using (var client = CreateClient()) {
                var dataAsString = JsonConvert.SerializeObject(user);
                var content = new StringContent(dataAsString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(APP_PATH + "api/account/register",content).Result;
                //return response.Content.ReadAsStringAsync().Result;
                ShowResult($"Register result -> {response.Content.ReadAsStringAsync().Result}");
            }
            return "";
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