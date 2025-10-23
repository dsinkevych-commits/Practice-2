using System;
using System.Text;

namespace InheritanceDemo_V2
{
    // Абстрактний базовий клас
    public abstract class Vyrib
    {
        private string _name = string.Empty;
        private double _weight;
        private string _color = string.Empty;

        public string Name
        {
            get => _name;
            protected set => _name = value ?? "Невідомо";
        }

        public double Weight
        {
            get => _weight;
            protected set => _weight = value < 0 ? 0 : value;
        }

        public string Color
        {
            get => _color;
            protected set => _color = value ?? "Невідомо";
        }

        protected Vyrib(string name, double weight, string color)
        {
            Name = name;
            Weight = weight;
            Color = color;
        }

        // ТЕПЕР це абстрактні методи (реалізація в нащадках)
        public abstract string GetInfo();
        public abstract string GetWeightCategory();
    }

    public class Detal : Vyrib
    {
        public double Price { get; private set; }

        public Detal(string name, double weight, string color, double price)
            : base(name, weight, color)
        {
            Price = price < 0 ? 0 : price;
        }

        public override string GetInfo()
            => $"Виріб: {Name}, Вага: {Weight} кг, Колір: {Color}, Тип: Деталь, Ціна: {Price:F2} грн";

        public override string GetWeightCategory()
            => Weight switch
            {
                < 10 => "[Деталь] Легкий виріб",
                > 50 => "[Деталь] Важкий виріб",
                _ => "[Деталь] Середньої ваги"
            };

        public void ShowDetail(string detailName, double detailPrice)
        {
            Console.WriteLine($"Деталь: {detailName}, Ціна: {detailPrice:F2} грн");
        }

        public double CalculateTotalCost(int count)
        {
            if (count < 0) count = 0;
            return Price * count;
        }
    }

    public class Vuzol : Vyrib
    {
        public string Purpose { get; private set; }
        public double Price { get; private set; }

        public Vuzol(string name, double weight, string color, string purpose, double price)
            : base(name, weight, color)
        {
            Purpose = purpose ?? "Невідомо";
            Price = price < 0 ? 0 : price;
        }

        public override string GetInfo()
            => $"Виріб: {Name}, Вага: {Weight} кг, Колір: {Color}, Тип: Вузол, Призначення: {Purpose}, Ціна вузла: {Price:F2} грн";

        public override string GetWeightCategory()
            => Weight switch
            {
                < 10 => "[Вузол] Легкий виріб",
                > 50 => "[Вузол] Важкий виріб",
                _ => "[Вузол] Середньої ваги"
            };

        // ВИМОГА 2: демонструємо ToString та Equals у нащадку

        public override string ToString()
            => $"Vuzol: Name={Name}, Purpose={Purpose}, Price={Price:F2}, Weight={Weight}, Color={Color}";

        public override bool Equals(object? obj)
        {
            if (obj is not Vuzol other) return false;
            // Приклад критерію рівності: однакові Назва + Призначення
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(Purpose, other.Purpose, StringComparison.OrdinalIgnoreCase)
                   && Math.Abs(Price - other.Price) < 0.0001;
        }

        public override int GetHashCode()
        {
            // Якщо перевизначено Equals — слід перевизначити і GetHashCode
            return HashCode.Combine(Name?.ToLowerInvariant(), Purpose?.ToLowerInvariant(), Math.Round(Price, 2));
        }

        // Метод без параметрів для виводу всіх даних
        public void PrintAll()
        {
            Console.WriteLine("=== Інформація про вузол ===");
            Console.WriteLine(GetInfo());
            Console.WriteLine(GetWeightCategory());
            Console.WriteLine(ToString());
            Console.WriteLine("============================");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var detal = new Detal("Шестерня", 0.35, "Сірий", 210.00);
            var vuzolA = new Vuzol("Редуктор", 62.0, "Чорний", "Передача крутного моменту", 12500);
            var vuzolB = new Vuzol("Редуктор", 62.0, "Чорний", "Передача крутного моменту", 12500);

            // Виклик усіх розроблених методів
            Console.WriteLine(detal.GetInfo());
            Console.WriteLine(detal.GetWeightCategory());
            detal.ShowDetail("Шайба 8мм", 1.90);
            double sum = detal.CalculateTotalCost(50);
            Console.WriteLine($"Сума за {50} деталей: {sum:F2} грн");

            Console.WriteLine();

            vuzolA.PrintAll();
            Console.WriteLine(vuzolA.GetInfo());
            Console.WriteLine(vuzolA.GetWeightCategory());

            // Демонстрація ToString та Equals
            Console.WriteLine($"ToString для вузла A: {vuzolA}");
            Console.WriteLine($"A дорівнює B? => {vuzolA.Equals(vuzolB)}");

            // Нерівність (інша ціна)
            var vuzolC = new Vuzol("Редуктор", 62.0, "Чорний", "Передача крутного моменту", 12999);
            Console.WriteLine($"A дорівнює C? => {vuzolA.Equals(vuzolC)}");
        }
    }
}
