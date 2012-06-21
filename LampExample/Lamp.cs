using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace YieldMachine.LampExample
{
    public class Lamp : StateMachine
    {
        [Trigger]
        public readonly Action PressSwitch;

        [Trigger]
        public readonly Action GotError;

        /// <summary>
        /// A lamp state machine. Typical structure:
        /// <code>
        ///     protected override IEnumerable WalkStates()
        ///     {
        ///     [state name]:
        ///         // State entry actions
        ///         yield return null;  // (wait for a trigger to be called)
        ///         // State exit actions
        ///         
        ///         // Transitions from this state, by checking which trigger has been called
        ///         if (Trigger == PressSwitch) goto [some other state];
        ///         
        ///         // Throw an exception if the trigger was invalid
        ///         InvalidState();
        ///         
        ///     [some other state]:
        ///         ...
        ///     }
        /// </code>    
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable WalkStates()
        {
        off:
            Console.WriteLine("off.");
            yield return null;

            if (Trigger == PressSwitch) goto on;
            InvalidTrigger();

        on:
            Console.WriteLine("*shiiine!*");
            yield return null;

            if (Trigger == GotError) goto error;
            if (Trigger == PressSwitch) goto off;
            InvalidTrigger();

        error:
            Console.WriteLine("-err-");
            yield return null;

            if (Trigger == PressSwitch) goto off;
            InvalidTrigger();
        }
    }
}
