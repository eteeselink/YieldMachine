using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YieldMachine.LampExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sm = new Lamp();
            sm.PressSwitch(); //go on
            sm.PressSwitch(); //go off

            sm.PressSwitch(); //go on
            sm.GotError();    //get error
            sm.PressSwitch(); //go off
        }
    }
}
