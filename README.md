YieldMachine
============
Inline state machines in C#
---------------------------

C# contains a state machine generator, used solely to support the `yield return` statement. How about we use it for, well, state machines?

Example: 

```C#
    public class Lamp : StateMachine
    {
        [Trigger]
        public readonly Action PressSwitch;

        [Trigger]
        public readonly Action GotError;

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
```