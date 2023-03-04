using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Aquarium aquarium = new Aquarium();

        aquarium.Work();
    }
}

class Aquarium
{
    private const string CommandAddFish = "1";
    private const string CommandRemoveFish = "2";
    private const string CommandSkip = "3";
    private const string CommandExit = "4";

    private int _totalСapacity = 15;

    private List<Fish> _fishes = new List<Fish>();
    private Random _random = new Random();
    private FishFactory _fishCreator = new FishFactory();

    public void Work()
    {
        while (true)
        {
            Console.WriteLine($"{CommandAddFish} - Добавить рыбку.\n{CommandRemoveFish} - Убрать рыбку.\n{CommandSkip} - Пропустить один цикл.\n{CommandExit} - Выход.");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case CommandAddFish:
                    AddFish();
                    break;

                case CommandRemoveFish:
                    RemoveFish();
                    break;

                case CommandSkip:
                    SkipLifeCycle();
                    break;

                case CommandExit:
                    return;

                default:
                    break;
            }

            ShowInfo();
            SkipLifeCycle();
        }
    }

    private void AddFish()
    {
        if (_totalСapacity >= _fishes.Count)
        {
            _fishes.Add(_fishCreator.Create());

            Console.WriteLine("Новая рыбка уже в аквариуме!");
        }
        else
        {
            Console.WriteLine("Рыбкам будет слишком тесно, Вы не можете добавить еще рыб :(");
        }
    }

    private void RemoveFish()
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

    private void SkipLifeCycle()
    {
        CheckDeadFish();

        foreach (Fish fish in _fishes)
        {
            fish.GetOlder();
        }
    }

    private void ShowInfo()
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

    private void CheckDeadFish()
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

    public void GetOlder()
    {
        LifeTime++;
    }
}

class FishFactory
{
    private int _lifeExpectancy;
    private int _lifeTime = 0;
    private string _name;

    private List<string> _names = new List<string>
    {
        "Карась", "Сомик", "Меченосец", "Минтай", "Барбус"
    };

    private Random _random = new Random();

    public Fish Create()
    {
        int maximumLifeExpectancy = 25;
        int minimumLifeExpectancy = 10;

        _name = _names[_random.Next(0, _names.Count)];
        _lifeExpectancy = _random.Next(minimumLifeExpectancy, maximumLifeExpectancy);

        return new Fish(_lifeExpectancy, _lifeTime, _name);
    }
}