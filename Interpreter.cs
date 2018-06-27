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
                {"ucreate", new CreateUser()},
                {"udelete", new DeleteUser()},
                {"uchange", new ChangeUser()},
                {"rshow", new ShowRoles()},
                {"rcreate", new CreateRole()},
                {"rdelete", new DeleteRole()},
                {"exit", new Exit()}
            };
            String commandsString = "help ";
            foreach(KeyValuePair<String, Commands> p in commandDictionary){
                commandsString += $"/ {p.Key} ";
            }
            commandDictionary.Add("help", new Help(commandsString));
        }
        public String ReadEnter(String message) {
            words = message.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words[0];
        }
        public void CommandRun () {
            if(commandDictionary.ContainsKey(words[0])) {
                commandDictionary[words[0]].Run();
            } else {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("We don't have this command yet.");
                Console.ResetColor();
            }
        }
    }
}