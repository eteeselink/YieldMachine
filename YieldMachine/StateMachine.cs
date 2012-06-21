using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;

namespace YieldMachine
{
    /// <summary>
    /// Base class for state machines. Implement the WalkStates() method to use.
    /// See the LampExample for an example
    /// </summary>
    public abstract class StateMachine
    {
        private IEnumerator currentState;

        /// <summary>
        /// Is set to the trigger that was called most recently. Always contains a reference
        /// to a child class property/field decorated with the [Trigger] attribute.
        /// </summary>
        protected Action Trigger { get; private set; }

        public StateMachine()
        {
            SetupTriggers();
            currentState = WalkStates().GetEnumerator();
            currentState.MoveNext();
        }

        protected abstract IEnumerable WalkStates();

        protected void InvalidTrigger()
        {
            throw new InvalidTriggerException("Invalid trigger!");
        }

        /// <summary>
        /// Gets all members returned by the `getMembers` method that have the TriggerAttribute set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getMembers"></param>
        /// <returns></returns>
        private IEnumerable<T> TriggerMembers<T>(Func<BindingFlags, T[]> getMembers) where T : MemberInfo
        {
            return getMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttributes(typeof(TriggerAttribute), false).Any());
        }

        /// <summary>
        /// Creates an action object that, when called, sets `Trigger` to itself and then moves to the next state.
        /// </summary>
        private Action MakeTrigger()
        {
            Action foo = null;
            foo = () =>
            {
                Trigger = foo;
                currentState.MoveNext();
            };
            return foo;
        }

        private void VerifyMemberType(Type type)
        {
            if (type != typeof(Action))
            {
                throw new InvalidStateMachineException("Fields/properties decorated with [Trigger] must be of type Action");
            }
        }

        /// <summary>
        /// Finds all fields and properties that have the [Trigger] attribute, and assigns a trigger action to them.
        /// </summary>
        private void SetupTriggers()
        {
            var type = GetType();
            foreach (var field in TriggerMembers(type.GetFields))
            {
                VerifyMemberType(field.FieldType);
                field.SetValue(this, MakeTrigger());
            }
            foreach (var prop in TriggerMembers(type.GetProperties))
            {
                VerifyMemberType(prop.PropertyType);
                prop.SetValue(this, MakeTrigger(), null);
            }
        }
    }
}
