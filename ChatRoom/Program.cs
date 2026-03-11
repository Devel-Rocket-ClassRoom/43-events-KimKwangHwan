using System;

public class Program
{
    public static void Main()
    {
        ChatRoom cr = new ChatRoom();
        ChatLogger logger = new ChatLogger();
        NotificationService nfc = new NotificationService();

        cr.MessageReceived += logger.SendLog;
        cr.MessageReceived += nfc.SendNfc;

        cr.SendMessage("철수", "안녕하세요.");
        cr.SendMessage("영희", "긴급 회의가 있습니다.");
        cr.SendMessage("민수", "점심 뭐 먹을까요?");
    }
}
