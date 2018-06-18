using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace LinkHolderConsole {
    internal abstract class Commands {
        public abstract String Run();
        public const string APP_PATH= "http://localhost:5000/";
    }
    internal sealed class Login : Commands {
        public override String Run() {
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
                    client.PostAsync(Commands.APP_PATH + "api/account/token", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary;
            }
        }
    }
    internal sealed class Register : Commands {
        public override String Run(){
            return "OK";
        }
    }
    
    
            // создаем http-клиента с токеном 
            // static HttpClient CreateClient(string accessToken = "") {
            //     var client = new HttpClient();
            //     if (!string.IsNullOrWhiteSpace(accessToken)) {
            //         client.DefaultRequestHeaders.Authorization =
            //             new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            //     }
            //     return client;
            // }
    
            // получаем информацию о клиенте 
            // static string GetUserInfo(string token) {
            //     using (var client = CreateClient(token)) {
            //         var response = client.GetAsync(APP_PATH + "api/admin").Result;
            //         return response.Content.ReadAsStringAsync().Result;
            //     }
            // }
    
            // обращаемся по маршруту api/values 
            // static string GetValues(string token) {
            //     using (var client = CreateClient(token)) {
            //         var response = client.GetAsync(APP_PATH + "/api/values").Result;
            //         return response.Content.ReadAsStringAsync().Result;
            //     }
            // }
}