using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Aquarium aquarium = new Aquarium();
        Menu menu = new Menu(aquarium);

        menu.Work();
    }
}

class Menu
{
    private Aquarium _aquarium;

    public Menu(Aquarium aquarium)
    {
        _aquarium = aquarium;
    }

    public void Work()
    {
        const string CommandAddFish = "1";
        const string CommandRemoveFish = "2";
        const string CommandSkip = "3";
        const string CommandExit = "4";

        bool isWork = true;

        while (isWork)
        {
            Console.WriteLine($"{CommandAddFish} - Добавить рыбку.\n{CommandRemoveFish} - Убрать рыбку.\n{CommandSkip} - Пропустить один цикл.\n{CommandExit} - Выход.");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case CommandAddFish:
                    _aquarium.AddFish();
                    break;

                case CommandRemoveFish:
                    _aquarium.RemoveFish();
                    break;

                case CommandSkip:
                    _aquarium.SkipLifeCycle();
                    break;

                case CommandExit:
                    isWork = false;
                    break;

                default:
                    Console.WriteLine("Ошибка! Нет такой команды.");
                    break;
            }

            _aquarium.ShowInfo();
        }
    }
}

class Aquarium
{
    private int _totalСapacity = 15;

    private FishFactory _fishCreator = new FishFactory();
    private List<Fish> _fishes = new List<Fish>();
    private Random _random = new Random();

    public void AddFish()
    {
        if (_totalСapacity > _fishes.Count)
        {
            _fishes.Add(_fishCreator.Create());

            Console.WriteLine("Новая рыбка уже в аквариуме!");
        }
        else
        {
            Console.WriteLine("Рыбкам будет слишком тесно, Вы не можете добавить еще рыб :(");
        }
    }

    public void RemoveFish()
    {
        if (_fishes.Count > 0)
        {
            Fish fish = _fishes[_random.Next(_fishes.Count)];
            _fishes.Remove(fish);

            Console.WriteLine("Вы достали одну рыбку из аквариума!");
        }
        else
        {
            Console.WriteLine("В аквариуме рыбки закончились :(");
        }
    }

    public void SkipLifeCycle()
    {
        TryDeleteFishes();

        foreach (Fish fish in _fishes)
        {
            fish.Grow();
        }
    }

    public void ShowInfo()
    {
        if (_fishes.Count > 0)
        {
            foreach (Fish fish in _fishes)
            {
                Console.WriteLine($"\n{fish.Name}. Возраст: {fish.LifeTime}. Продолжительность жизни: {fish.LifeExpectancy}\n");
            }
        }
        else
        {
            Console.WriteLine("Сейчас в аквариуме нет рыбок :(");
        }
    }

    private void TryDeleteFishes()
    {
        List<Fish> deadFish = new List<Fish>();

        foreach (Fish fish in _fishes)
        {
            if (fish.LifeTime >= fish.LifeExpectancy)
            {
                deadFish.Add(fish);
            }
        }

        foreach (Fish fish in deadFish)
        {
            fish.NotifyDeath();
            _fishes.Remove(fish);
        }
    }
}

class Fish
{
    public Fish(int lifeExpectancy, int lifeTime, string name)
    {
        LifeExpectancy = lifeExpectancy;
        LifeTime = lifeTime;
        Name = name;
    }

    public int LifeExpectancy { get; private set; }

    public int LifeTime { get; private set; }

    public string Name { get; private set; }

    public void NotifyDeath()
    {
        Console.WriteLine($"{Name} умер, в возрасте: {LifeTime} лет.\n");
    }

    public void Grow()
    {
        LifeTime++;
    }
}

class FishFactory
{
    private int _lifeExpectancy;
    private int _lifeTime = 0;
    private string _name;

    private string[] _fishNames =
    {
        "Карась", "Сомик", "Меченосец", "Минтай", "Барбус"
    };

    private Random _random = new Random();

    public Fish Create()
    {
        int maximumLifeExpectancy = 25;
        int minimumLifeExpectancy = 10;

        _name = _fishNames[_random.Next(_fishNames.Length)];
        _lifeExpectancy = _random.Next(minimumLifeExpectancy, maximumLifeExpectancy);

        return new Fish(_lifeExpectancy, _lifeTime, _name);
    }
}