using System;
using System.Collections.Generic;

namespace LinkHolderConsole {
    internal sealed class Interpreter {
        private String[] words;
        private Dictionary<String, Commands> commandDictionary;
        private List<String> runStatus;
        private static String token;
        public Interpreter() {
            commandDictionary = new Dictionary<String, Commands> {
                {"register", new Register()},
                {"login", new Login()},
                {"value", new Value()},
                {"exit", new Exit()}
            };
            runStatus = new List<string>();
        }
        public String ReadEnter(String message) {
            words = message.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words[0];
        }
        public void CommandRun () {
            if(words[0].Equals("login")) {
                token = commandDictionary["login"].Run("");
                if(token != null) {runStatus.Add("Login OK!");}
            } else {
                String result = commandDictionary[words[0]].Run(token);
                runStatus.Add(result);
            }
        }
        public void ShowRunStatus(){
            foreach(String s in runStatus){
                Console.WriteLine(s);
            }
        }
    }
}