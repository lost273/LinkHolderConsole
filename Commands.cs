using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using LinkHolderConsole.Models;
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
            if(result.IndexOf(" successfully ") == -1) {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            } else {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            Console.WriteLine(result);
            Console.ResetColor();
        }

    }
    internal sealed class Login : Commands {
        public override String Run(String token) {
            //Console.WriteLine("Enter the email:");
            String userName = "bob@example.com";
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
                ShowResult($"Register result -> {response.Content.ReadAsStringAsync().Result}");
            }
            return "";
        }
    }
    internal sealed class GetValue : Commands {
        public override String Run(String token) {
            using (var client = CreateClient(token)) {
                var response = client.GetAsync(APP_PATH + "api/values").Result;
                var content = response.Content.ReadAsStringAsync().Result;
                if(!response.IsSuccessStatusCode) {
                    ShowResult($"Service is not accessible for you!");
                    return "";
                }
                IEnumerable<ViewFolderModel> folder = 
                    JsonConvert.DeserializeObject<IEnumerable<ViewFolderModel>>(content);
                ShowFolder(folder);
                return response.Content.ReadAsStringAsync().Result;
            }
        }
        private void ShowFolder(IEnumerable<ViewFolderModel> folder){
            foreach(ViewFolderModel f in folder){
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("============================");
                Console.WriteLine($"FOLDER(id-{f.Id})\t[{f.Name}]");
                Console.WriteLine("============================");
                foreach(ViewLinkModel l in f.MyLinks){
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"LINK(id-{l.Id})\t{l.Body}");
                    Console.WriteLine($"DESCRIPTION\t{l.Description}");
                    Console.WriteLine("----------------------------");
                }
            }
            Console.ResetColor();
        }
    }
    internal sealed class SaveLink : Commands {
        public override String Run(string token) {
            SaveLinkModel link = new SaveLinkModel();
            Console.Write("Body: ");
            link.LinkBody = Console.ReadLine();
            Console.Write("Description: ");
            link.LinkDescription = Console.ReadLine();
            Console.Write("Folder Name: ");
            link.FolderName = Console.ReadLine();
            using (var client = CreateClient(token)) {
                var dataAsString = JsonConvert.SerializeObject(link);
                var content = new StringContent(dataAsString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(APP_PATH + "api/values", content).Result;
                ShowResult($"SaveLink result -> {response.Content.ReadAsStringAsync().Result}");
            }
            return "";
        }
    }
    internal sealed class DeleteLink : Commands {
        public override String Run(String token) {
            Console.Write("Id: ");
            String id = Console.ReadLine();
            using (var client = CreateClient(token)) {
                var response = client.DeleteAsync(APP_PATH + "api/values/link/" + id).Result;
                ShowResult($"Value result -> {response.Content.ReadAsStringAsync().Result}");
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
    internal sealed class DeleteFolder : Commands {
        public override String Run(String token) {
            Console.Write("Id: ");
            String id = Console.ReadLine();
            using (var client = CreateClient(token)) {
                var response = client.DeleteAsync(APP_PATH + "api/values/folder/" + id).Result;
                ShowResult($"Value result -> {response.Content.ReadAsStringAsync().Result}");
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
    internal sealed class ChangeLink : Commands {
        public override String Run(String token) {
            SaveLinkModel link = new SaveLinkModel();
            Console.Write("Id: ");
            String id = Console.ReadLine();
            Console.Write("Body: ");
            link.LinkBody = Console.ReadLine();
            Console.Write("Description: ");
            link.LinkDescription = Console.ReadLine();
            using (var client = CreateClient(token)) {
                var dataAsString = JsonConvert.SerializeObject(link);
                var content = new StringContent(dataAsString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PutAsync(APP_PATH + "api/values/link/" + id, content).Result;
                ShowResult($"ChangeLink result -> {response.Content.ReadAsStringAsync().Result}");
            }
            return "";
        }
    }
    internal sealed class ChangeFolder : Commands {
        public override String Run(String token) {
            Console.Write("Id: ");
            String id = Console.ReadLine();
            Console.Write("Name: ");
            String name = Console.ReadLine();
            using (var client = CreateClient(token)) {
                var dataAsString = JsonConvert.SerializeObject(name);
                var content = new StringContent(dataAsString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PutAsync(APP_PATH + "api/values/folder/" + id, content).Result;
                ShowResult($"ChangeFolder result -> {response.Content.ReadAsStringAsync().Result}");
            }
            return "";
        }
    }
    internal sealed class ShowUsers : Commands {
        public override String Run(String token) {
            using (var client = CreateClient(token)) {
                var response = client.GetAsync(APP_PATH + "api/admin").Result;
                var content = response.Content.ReadAsStringAsync().Result;
                IEnumerable<ViewUserModel> users = 
                    JsonConvert.DeserializeObject<IEnumerable<ViewUserModel>>(content);
                ShowUser(users);
                return response.Content.ReadAsStringAsync().Result;
            }
        }
        private void ShowUser(IEnumerable<ViewUserModel> users) {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("=============================================================");
            Console.WriteLine($"ID\t\t\t\t\tNAME\tEMAIL");
            Console.WriteLine("=============================================================");
            foreach(ViewUserModel u in users) {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{u.Id}\t{u.Name}\t{u.Email}");
                Console.WriteLine("----------------------------");
            }
            Console.ResetColor();
        }
    }
    internal sealed class Exit : Commands {
        public override String Run(String token) {
           return "exit";
        }
    }
}