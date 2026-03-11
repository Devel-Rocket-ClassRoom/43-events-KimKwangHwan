using System;
using System.Collections.Generic;
using System.Text;

public class ChatLogger
{
    public void SendLog(string sender, string message)
    {
        Console.WriteLine($"[로그] {sender}: {message}");
    }
}