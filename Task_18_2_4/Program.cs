using System;

namespace Task_18_2_4
{
    class Program
    {
        static void Main(string[] args)
        {
            Pult pult = new Pult();     // sender
            Gate gate = new Gate();     // receiver

            pult.SetAction(new GateOpenAction(gate));
            pult.OpenButton();
            pult.CloseButton();
        }

        interface IAction
        {
            public void Open();
            public void Close();

        }
        class GateOpenAction :IAction
        {
            Gate gate;
            public GateOpenAction(Gate gate)
            {
                this.gate = gate;
            }

            public void Close()
            {
                gate.Close();
            }

            public void Open()
            {
                gate.Open();
            }
        }

        class Pult 
        {
            IAction action;
            
            public void SetAction(IAction act)
            {
                action = act;
            }
            public void OpenButton()
            {
                action.Open();
            }
            public void CloseButton()
            {
                action.Close();
            }
        }

        class Gate
        {
            public void Open()
            {
                Console.WriteLine("Открываем ворота");
            }

            public void Close()
            {
                Console.WriteLine("Закрываем ворота");
            }
        }
    }

}
