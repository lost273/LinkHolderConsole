using System;
using System.Collections.Generic;

internal sealed class Interpreter {
    private String[] words;
    private Dictionary<String, Commands> commandDictionary;
    private List<String> runStatus;
    public Interpreter(String message) {
        words = message.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        commandDictionary = new Dictionary<String, Commands> {
            {"register", new Register()},
            {"login", new Login()}
        };
        runStatus = new List<string>();
    }
    public void CommandRun (){
        commandDictionary[words[0]].Run();
    }
}