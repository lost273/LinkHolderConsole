using System;
using System.Collections.Generic;

internal sealed class Interpreter {
    private String[] words;
    private Dictionary<String, Commands> commandDictionary;
    public Interpreter(String message) {
        words = message.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }
    public void CommandRun (){
        commandDictionary[words[0]].Run();
    }
}