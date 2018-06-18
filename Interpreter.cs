using System;
using System.Collections.Generic;

namespace LinkHolderConsole {
    internal sealed class Interpreter {
        private String[] words;
        private Dictionary<String, Commands> commandDictionary;
        private List<String> runStatus;
        private String token;
        public Interpreter(String message) {
            words = message.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            commandDictionary = new Dictionary<String, Commands> {
                {"register", new Register()},
                {"login", new Login()}
            };
            runStatus = new List<string>();
        }
        public void CommandRun () {
            if(words[0].Equals("login")) {
                token = commandDictionary["login"].Run();
            } else {
                commandDictionary[words[0]].Run();
            }
        }
    }
}