using System;

namespace LinkHolderConsole {
    class Program {
        static void Main(string[] args) {
            
            Interpreter interpreter = new Interpreter();
            String key = "";

            do {
                Console.WriteLine("Please enter the command.");
                key = interpreter.ReadEnter(Console.ReadLine());
                interpreter.CommandRun();
                interpreter.ShowRunStatus();
                interpreter = new Interpreter();
            } while(!key.Equals("exit"));
        }
    }
}
