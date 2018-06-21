using System;
using System.Collections.Generic;

namespace LinkHolderConsole {
    internal sealed class Interpreter {
        private String[] words;
        private Dictionary<String, Commands> commandDictionary;
        private static String token;
        public Interpreter() {
            commandDictionary = new Dictionary<String, Commands> {
                {"register", new Register()},
                {"login", new Login()},
                {"value", new GetValue()},
                {"delete-value", new DeleteValue()},
                {"exit", new Exit()}
            };
        }
        public String ReadEnter(String message) {
            words = message.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words[0];
        }
        public void CommandRun () {
            if(words[0].Equals("help")) {
                Console.WriteLine("List of the commands:");
                foreach(KeyValuePair<String, Commands> p in commandDictionary){
                    Console.WriteLine($"{p.Key}");
                }
                return;
            }
            if(!commandDictionary.ContainsKey(words[0])) {
                Console.WriteLine("We don't have this command yet.");
                return;
            }
            if(words[0].Equals("login")) {
                token = commandDictionary["login"].Run("");
            } else {
                commandDictionary[words[0]].Run(token);
            }
        }
    }
}