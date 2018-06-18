using System;

namespace LinkHolderConsole {
    class Program {
        
        private static string token;
        private static Interpreter interpreter;
        static void Main(string[] args) {

            Console.WriteLine("Please enter the command.");

            interpreter = new Interpreter(Console.ReadLine());
            interpreter.CommandRun();
            
        }
    }
}
