using System;
using System.Collections.Generic;

internal abstract class Commands {
    public abstract String Run();
}
internal sealed class Login : Commands {
    public override String Run(){
        //Console.WriteLine("Enter the email:");
        String userName = "admin@example.com";
        //Console.WriteLine("Enter the password:");
        String password = "Secret123$";
        Dictionary<string, string> tokenDictionary = GetTokenDictionary(userName, password);
        token = tokenDictionary["access_token"];
        return "OK";
    }
}
internal sealed class Register : Commands {
    public override String Run(){
        return "OK";
    }
}