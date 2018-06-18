using System;

internal sealed class Interpreter {
    private String[] words;
    public Interpreter(String message) {
        words = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }
}