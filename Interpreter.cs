using System;
using System.Collections.Generic;

namespace LinkHolderConsole {
    internal sealed class Interpreter {
        private String[] words;
        private Dictionary<String, Commands> commandDictionary;
        public Interpreter() {
            commandDictionary = new Dictionary<String, Commands> {
                {"register", new Register()},
                {"login", new Login()},
                {"value", new GetValue()},
                {"lsave", new SaveLink()},
                {"ldelete", new DeleteLink()},
                {"fdelete", new DeleteFolder()},
                {"lchange", new ChangeLink()},
                {"fchange", new ChangeFolder()},
                {"ushow", new ShowUsers()},
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
            commandDictionary[words[0]].Run();
        }
    }
}