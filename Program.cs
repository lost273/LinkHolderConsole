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
            } while (!key.Equals("exit"));
        }
    }
}
