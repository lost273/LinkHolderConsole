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
                {"save-link", new SaveLink()},
                {"delete-link", new DeleteLink()},
                {"delete-folder", new DeleteFolder()},
                {"exit", new Exit()}
            };
        }
        public String ReadEnter(String message) {
            words = message.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words[0];
        }
        public void CommandRun () {
            if(words[0].Equals("help")) {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("List of the commands:");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                foreach(KeyValuePair<String, Commands> p in commandDictionary){
                    Console.Write($"/ {p.Key} ");
                }
                Console.WriteLine();
                Console.ResetColor();
                return;
            }
            if(!commandDictionary.ContainsKey(words[0])) {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("We don't have this command yet.");
                Console.ResetColor();
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