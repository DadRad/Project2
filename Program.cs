using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Как вас зовут?");
        string name = Console.ReadLine();
        Console.Write("Укажите ваш возраст:");
        int age;
        while (!int.TryParse(Console.ReadLine(), out age))
        { 
            Console.WriteLine("Введите корректный возраст!");
        }
        Console.Write("Укажите ваш опыт работы в нашей сфере:");
        int workExpirience;
        while (!int.TryParse(Console.ReadLine(), out workExpirience))
        {
            Console.WriteLine("Введите корректное число!");
        }
        Console.WriteLine($"Вас зовут {name}, ваш возраст - {age}, ваш опыт работы - {workExpirience}");
    }
}