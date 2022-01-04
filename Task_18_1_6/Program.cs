using System;

namespace Task_18_1_6
{
    class Program
    {
        /// <summary>
        ///  Клиентский код
        /// </summary>
        static void Main()
        {
            IFace myObject = new BaseClass(1); // ImplementationOne(1);
            myObject.GetId();

            IFace clone = myObject.Clone();
            clone.GetId();

            myObject = new BaseClass2(2); // ImplementationTwo(2);
            myObject.GetId();

            clone = myObject.Clone();
            clone.GetId();
        }
    }

    internal class BaseClass : IFace
    {
        public int Id { get; set; }

        public BaseClass(int id) => Id = id;
        public IFace Clone()
        {
            return new BaseClass(Id);
        }
    }

    internal class BaseClass2 : IFace
    {
        public int Id { get; set; }

        public BaseClass2(int id) => Id = id;
        public IFace Clone()
        {
            return new BaseClass2(Id);
        }
    }

        interface IFace
    {
        public int Id { get; set; }
        public IFace Clone();
        public void GetId() =>  Console.WriteLine($"Creating an object with Id {Id}");

    }



}
