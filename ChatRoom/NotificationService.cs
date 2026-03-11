using System;
using System.Collections.Generic;
using System.Text;

public class NotificationService
{
    public void SendNfc(string sender, string message)
    {
        if (message.Contains("긴급"))
        {
            Console.WriteLine("[알림] 긴급 메시지 수신!");
        }
    }
}