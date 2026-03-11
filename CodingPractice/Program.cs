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
        {
            Stock msStock = new Stock("MSFT", 100.00m);
            msStock.PriceChanged += (sender, e) =>
            {
                Stock stock = (Stock)sender;
                Console.WriteLine($"[{stock}]");
                Console.WriteLine($"    이전 가격: {e.OldPrice:C}");
                Console.WriteLine($"    현재 가격: {e.NewPrice:C}");
                Console.WriteLine($"    변동률: {e.ChangePercent:F2}%");
                Console.WriteLine();
            };

            msStock.Price = 110.00m;
            msStock.Price = 105.50m;
            msStock.Price = 120.00m;
        }
        {
            Car car = new Car(50);
            Dashboard dashboard = new Dashboard();

            dashboard.Subscribe(car);
            for (int i = 0; i < 7; i++)
            {
                car.Drive();
                Console.WriteLine();
            }
            dashboard.UnSubscribe(car);
        }
        {
            SecurePublisher publisher = new SecurePublisher();
            publisher.MyEvent += Handler1;
            publisher.MyEvent += Handler2;

            Console.WriteLine("\n이벤트 발생: ");
            publisher.RaiseEvent();

            Console.WriteLine();
            publisher.MyEvent -= Handler1;
            Console.WriteLine("\n이벤트 발생: ");
            publisher.RaiseEvent();
        }
        {
            Module1 m1 = new Module1();
            Module2 m2 = new Module2();

            GlobalNotifier.SendMessage("시스템 시작");
            Console.WriteLine();
            GlobalNotifier.SendMessage("데이터 로드 완료");
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
    static void Handler1(object sender, EventArgs e)
    {
        Console.WriteLine("Handler1 실행됨");
    }
    static void Handler2(object sender, EventArgs e)
    {
        Console.WriteLine("Handler2 실행됨");
    }
}
delegate void Notify();
delegate void MyDelegate();
delegate void EventHandler2();
class Publisher
{
    public MyDelegate PublicDelegate;
    public event MyDelegate MyEvent;
}
class Button
{
    public event EventHandler2 Click;
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
class PriceChangedEventArgs : EventArgs
{
    public decimal OldPrice { get; }
    public decimal NewPrice { get; }
    public decimal ChangePercent { get; }

    public PriceChangedEventArgs(decimal oldPrice, decimal newPrice)
    {
        OldPrice = oldPrice;
        NewPrice = newPrice;
        if (oldPrice != 0)
        {
            ChangePercent = (newPrice - oldPrice) / oldPrice * 100;
        }
    }
}
class Stock
{
    private string _symbol;
    private decimal _price;

    public event EventHandler<PriceChangedEventArgs> PriceChanged;

    public Stock(string symbol, decimal initialPrice)
    {
        _symbol = symbol;
        _price = initialPrice;
    }
    public decimal Price
    {
        get => _price;
        set
        {
            if (_price == value)
            {
                return;
            }

            decimal oldPrice = _price;
            _price = value;

            OnPriceChanged(new PriceChangedEventArgs(oldPrice, _price));
        }
    }

    protected virtual void OnPriceChanged(PriceChangedEventArgs e)
    {
        PriceChanged?.Invoke(this, e);
    }

    public override string ToString()
    {
        return $"{_symbol}: {_price:C}";
    }
}
class FuelEventArgs : EventArgs
{
    public int FuelLevel { get; }
    public string Warning { get; }
    public FuelEventArgs(int fuelLevel, string warning)
    {
        FuelLevel = fuelLevel;
        Warning = warning;
    }
}
class Car
{
    private int _fuelLevel;
    public event EventHandler<FuelEventArgs> FuelLow;
    public event Action<int> FuelChanged;

    public Car(int initialFuel)
    {
        _fuelLevel = initialFuel;
    }

    public int FuelLevel => _fuelLevel;
    public void Drive()
    {
        if (_fuelLevel <= 0)
        {
            Console.WriteLine("연료 없음! 운전 불가");
            return;
        }

        _fuelLevel -= 10;
        Console.WriteLine($"운전 중... 연료: {_fuelLevel}%");

        FuelChanged?.Invoke(_fuelLevel);

        if (_fuelLevel <= 0)
        {
            OnFuelLow(new FuelEventArgs(_fuelLevel, "연료가 바닥났습니다!"));
        }
        else if (_fuelLevel <= 20)
        {
            OnFuelLow(new FuelEventArgs(_fuelLevel, "연료가 부족합니다"));
        }
    }

    protected virtual void OnFuelLow(FuelEventArgs e)
    {
        FuelLow?.Invoke(this, e);
    }
}
class Dashboard
{
    public void Subscribe(Car car)
    {
        car.FuelChanged += OnFuelChanged;
        car.FuelLow += OnFuelLow;
    }
    public void UnSubscribe(Car car)
    {
        car.FuelChanged -= OnFuelChanged;
        car.FuelLow -= OnFuelLow;
    }

    private void OnFuelChanged(int fuelLevel)
    {
        string guage = new string('█', fuelLevel / 10);
        Console.WriteLine($"[대시보드] 연료 게이지: {guage}");
    }
    private void OnFuelLow(object sender, FuelEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[경고!] {e.Warning} (잔량: {e.FuelLevel}%");
        Console.ResetColor();
    }
}
class SecurePublisher
{
    private EventHandler _myEvent;
    private readonly object _lock = new object();

    public event EventHandler MyEvent
    {
        add
        {
            lock (_lock)
            {
                Console.WriteLine($"구독자 추가: {value.Method.Name}");
                _myEvent += value;
            }
        }
        remove
        {
            lock (_lock)
            {
                Console.WriteLine($"구독자 제거: {value.Method.Name}");
                _myEvent -= value;
            }
        }
    }

    public void RaiseEvent()
    {
        _myEvent?.Invoke(this, EventArgs.Empty);
    }
}
class GlobalNotifier
{
    public static event Action<string> OnGlobalMessage;
    public static void SendMessage(string message)
    {
        Console.WriteLine($"[Global] 메시지 전송: {message}");
        OnGlobalMessage?.Invoke(message);
    }
}
class Module1
{
    public Module1()
    {
        GlobalNotifier.OnGlobalMessage += HandleMessage;
    }
    private void HandleMessage(string message)
    {
        Console.WriteLine($"[Modul1] 수신: {message}");
    }
}
class Module2
{
    public Module2()
    {
        GlobalNotifier.OnGlobalMessage += HandleMessage;
    }
    private void HandleMessage(string message)
    {
        Console.WriteLine($"[Modul2] 수신: {message}");
    }
}