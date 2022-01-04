using System;

namespace CommandExample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Called from Receiver");
            Receiver r = new();
            r.Operation();

            Console.WriteLine("\nCalled from CommandOne");
            CommandOne c1 = new(r);
            c1.Execute();

            Console.WriteLine("\nCalled from sender");
            Sender s = new();
            s.SetCommand(c1);
            s.Execute();

        }
    }

    class Receiver
    {
        public void Operation()
        {
            Console.WriteLine("Receiver: Процесс запущен");
        }
    }
    interface ICommand
    {
        public void Execute();
    }

    class CommandOne : ICommand
    {
        Receiver receiver;
        public CommandOne(Receiver receiver)
        {
            this.receiver = receiver;
        }
        public void Execute()
        {
            Console.WriteLine("CommandOne: Execute");
            receiver.Operation();
        }
    }
    class Sender
    {
        ICommand command;
        public void SetCommand(ICommand command)
        {
            this.command = command;
        }
        public void Execute()
        {
            Console.WriteLine("Sender: Execute");
            command.Execute();
        }
    }
}