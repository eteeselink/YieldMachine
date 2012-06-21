YieldMachine
============
Inline state machines in C#


C# contains a state machine generator, used solely to support the `yield return` statement. How about we use it for, well, state machines?

For example, the code below runs the following state machine:

![Lamp state machine](https://chart.googleapis.com/chart?chl=+digraph+lamp+%7B%0D%0A+++++off+-%3E+on+%5Blabel%3DPressSwitch%2C+fontsize%3D8%5D%0D%0A+++++on+-%3E+off+%5Blabel%3DPressSwitch%2C+fontsize%3D8%5D%0D%0A+++++on+-%3E+error+%5Blabel%3DGotError%2C+fontsize%3D8%5D%0D%0A+++++error+-%3E+off+%5Blabel%3DPressSwitch%2C+fontsize%3D8%5D%0D%0A+%7D%0D%0A++++++++&cht=gv)

Note that this state machine has 2 triggers and 3 states. In code, a state becomes a `goto` label, and a trigger becomes a property or field of type `Action`, decorated with the `Trigger` attribute:

```C#
    public class Lamp : StateMachine
    {
        // Triggers (or events, or actions, whatever) that your state machine understands.
        [Trigger]
        public readonly Action PressSwitch;

        [Trigger]
        public readonly Action GotError;

        // Actual state machine logic
        protected override IEnumerable WalkStates()
        {
        off:                                       // Each goto label is a state
            Console.WriteLine("off.");             // State entry actions
            yield return null;                     // Wait until a trigger is called
                                                   // Ah, a trigger was called! 
                                                   //   perform state exit actions (none, in this case)
            if (Trigger == PressSwitch) goto on;   // Transitions go here: depending on the trigger that was called,
                                                   //   go to the right state
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