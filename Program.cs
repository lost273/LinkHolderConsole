using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace LinkHolderConsole
{
    class Program {
        private const string APP_PATH= "http://localhost:5000/";
        private static string token;
        private static Interpreter interpreter;
        static void Main(string[] args) {

            Console.WriteLine("Welcome, please enter the command.");
            interpreter = new Interpreter(Console.ReadLine());

            interpreter.CommandRun();
            
            string userName = "admin@example.com";
            string password = "Secret123$";
 
            Dictionary<string, string> tokenDictionary = GetTokenDictionary(userName, password);
            token = tokenDictionary["access_token"];
 
            Console.WriteLine("Access Token:");
            Console.WriteLine(token);
 
            Console.WriteLine();
            string userInfo = GetUserInfo(token);
            Console.WriteLine("Пользователь:");
            Console.WriteLine(userInfo);
 
            Console.WriteLine();
            string values = GetValues(token);
            Console.WriteLine("Values:");
            Console.WriteLine(values);
 
        }
        
        // получение токена
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
                // Десериализация полученного JSON-объекта
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary;
            }
        }
 
        // создаем http-клиента с токеном 
        static HttpClient CreateClient(string accessToken = "") {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(accessToken)) {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }
 
        // получаем информацию о клиенте 
        static string GetUserInfo(string token) {
            using (var client = CreateClient(token)) {
                var response = client.GetAsync(APP_PATH + "api/admin").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }
 
        // обращаемся по маршруту api/values 
        static string GetValues(string token) {
            using (var client = CreateClient(token)) {
                var response = client.GetAsync(APP_PATH + "/api/values").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
