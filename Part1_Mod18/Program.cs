using System;
using System.Collections.Generic;

namespace Part1_Mod18
{
    class Program
    {
        static void Main(string[] args)
        {
            CarPlant carPlant = new();
            Conveyor builder = new BikeConveyor();
            carPlant.Construct(builder);
            builder.Product.Show();

            Console.WriteLine("-- end --");
        }
    }
    class Product
    {
        private string _type;

        // составные части
        private Dictionary<string, string> _parts = new Dictionary<string, string>();
        public Product(string type)
        {
            _type = type;
        }
        // Индексатор
        public string this[string key]
        {
            set
            {
                _parts[key] = value;
            }
        }
        public void Show()
        {
            Console.WriteLine();
            Console.WriteLine($"Вид транспортного средства: {_type}");
            Console.WriteLine($" Рама : {_parts["frame"]}");
            Console.WriteLine($" Двигатель : {_parts["engine"]}");
            Console.WriteLine($" Колеся: {_parts["wheels"]}");
            Console.WriteLine($" Двери : {_parts["doors"]}");
        }
    }
    abstract class Conveyor
    {
        protected Product _product;
        public Product Product
        {
            get { return _product; }
        }
        // Методы для постройки составных частей
        public abstract void BuildFrame();
        public abstract void BuildEngine();
        public abstract void BuildWheels();
        public abstract void BuildDoors();
    }

    class BikeConveyor : Conveyor
    {
        public BikeConveyor()
        {
            _product = new Product("Bike");
        }
        // Методы для постройки составных частей
        public override void BuildFrame() => _product["frame"] = "Bike frame";
        public override void BuildEngine() => _product["engine"] = "Bike engine";
        public override void BuildWheels() => _product["wheels"] = "Bike wheels";
        public override  void BuildDoors() => _product["doors"] = "0";
    }


    class CarPlant 
    {
        public void Construct(Conveyor conveyor)
        {
            conveyor.BuildFrame();
            conveyor.BuildEngine();
            conveyor.BuildWheels();
            conveyor.BuildDoors();
        }
    }

    interface IBuilder
    {
        void BuildFrame();
        void BuildEngine();
        void BuildWheels();
        void BuildDoors();
    }

}
