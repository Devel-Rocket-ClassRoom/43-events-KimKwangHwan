using System;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        {
            Notify notify = SayHello;
            notify += SayGoodbye;

            notify();
        }
        {
            Publisher pub = new Publisher();
            pub.PublicDelegate = () => Console.WriteLine("대리자 호출");
            pub.PublicDelegate();

            pub.MyEvent += () => Console.WriteLine("이벤트 호출");
        }
        {
            Button button = new Button();

            button.Click += HandleClick;
            button.Click += HandleClickAgain;

            button.OnClick();
        }
        {
            Player player = new Player();
            HealthBar healthBar = new HealthBar();
            SoundManager soundManager = new SoundManager();

            player.DamageTaken += healthBar.OnPlayerDamaged;
            player.DamageTaken += soundManager.OnPlayerDamaged;

            player.TakeDamage(30);
        }
        {
            Timer timer = new Timer();
            Logger logger = new Logger();

            timer.Tick += logger.OnTick;

            Console.WriteLine("=== 구독 상태 ===");
            timer.Start();

            timer.Tick -= logger.OnTick;

            Console.WriteLine("\n=== 구독 해제 후 ===");
            timer.Start();
        }
        {
            Sensor sensor = new Sensor();
            sensor.Alert += message =>
            {
                Console.WriteLine($"[경보] {message}");
            };
            sensor.Alert += message =>
            {
                Console.WriteLine($"[로그] {DateTime.Now}: {message}");
            };
            sensor.Detect("움직임 감지됨");
            sensor.Detect("온도 상승");
        }
        {
            GameCharacter hero = new GameCharacter("용사");

            hero.OnDeath += () => Console.WriteLine("캐릭터가 사망했습니다.");
            hero.OnDamaged += health => Console.WriteLine($"남은 체력: {health}");

            hero.OnAttack += (damage, target) => Console.WriteLine($"{target}에게 {damage} 데미지!");

            hero.Attack(50, "슬라임");
            hero.TakeDamage(30);
            hero.TakeDamage(80);
        }
    }
    static void SayHello()
    {
        Console.WriteLine("안녕하세요!");
    }
    static void SayGoodbye()
    {
        Console.WriteLine("안녕히 가세요!");
    }
    static void HandleClick()
    {
        Console.WriteLine("버튼이 클릭되었습니다!");
    }
    static void HandleClickAgain()
    {
        Console.WriteLine("클릭 처리 완료");
    }
}
delegate void Notify();
delegate void MyDelegate();
delegate void EventHandler();
class Publisher
{
    public MyDelegate PublicDelegate;
    public event MyDelegate MyEvent;
}
class Button
{
    public event EventHandler Click;
    public void OnClick()
    {
        if (Click != null)
        {
            Click();
        }
    }
}
class Player
{
    public event Action<int> DamageTaken;

    private int _health = 100;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        Console.WriteLine($"플레이어 체력: {_health}");
        DamageTaken?.Invoke(_health);
    }
}
class HealthBar
{
    public void OnPlayerDamaged(int currentHealth)
    {
        Console.WriteLine($"[UI] 체력바 업데이트: {currentHealth}%");
    }
}
class SoundManager
{
    public void OnPlayerDamaged(int currentHealth)
    {
        Console.WriteLine("[Sound] 피격 효과음 재생");
    }
}
class Timer
{
    public event Action Tick;
    private int _count;
    public void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            _count++;
            Console.WriteLine($"타이머: {_count}초");
            Tick?.Invoke();
        }
    }
}
class Logger
{
    public void OnTick()
    {
        Console.WriteLine("[Logger] 틱 기록됨");
    }
}
class Sensor
{
    public event Action<string> Alert;
    public void Detect(string message)
    {
        Console.WriteLine($"감지: {message}");
        Alert?.Invoke(message);
    }
}
class GameCharacter
{
    public event Action OnDeath;
    public event Action<int> OnDamaged;
    public event Action<int, string> OnAttack;

    private int _health = 100;
    private string _name;

    public GameCharacter(string name)
    {
        _name = name; 
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        OnDamaged?.Invoke(_health);

        if (_health <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Attack(int damage, string targetName)
    {
        OnAttack?.Invoke(damage, targetName);
    }
}