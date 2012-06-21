YieldMachine
============
Inline state machines in C#


C# contains a state machine generator, used solely to support the `yield return` statement. How about we use it for, well, state machines?

Example: 

```C#
    public class Lamp : StateMachine
    {
        // Triggers (or events, or actions, whatever) that your state machine understands.
        [Trigger]
        public readonly Action PressSwitch;

        [Trigger]
        public readonly Action GotError;

        protected override IEnumerable WalkStates()
        {
        off:                                       // Each goto label is a state
            Console.WriteLine("off.");             // State entry actions
            yield return null;                     // Wait until a trigger is called
                                                   // Ah, a trigger was called! 
                                                   //   perform state exit actions (none, in this case)
            if (Trigger == PressSwitch) goto on;   // Transitions go here: depending on the trigger that was called, go to
                                                   //   the right state
            InvalidTrigger();                      // Throw exception on invalid trigger

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

The `[Trigger]` fields are automatically assigned action objects upon construction. These action objects, when called, set the base class's `Trigger` property to the action that was called, and then moves the state machine to the next state.