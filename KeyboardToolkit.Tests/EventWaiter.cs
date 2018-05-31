using System;
using System.Collections.Generic;
using System.Threading;

namespace KeyboardToolkit.Tests
{
    public class EventWaiter
    {
        public static IEnumerable<TArgs> WaitEvent<TArgs>(
            Action<EventHandler<TArgs>> subscriber,
            Action<EventHandler<TArgs>> unsubscriber,
            Action eventTrigger,
            int expectedEventCount)
        {
            var actual = new List<TArgs>();
            var countdownEvent = new CountdownEvent(expectedEventCount);
            var eventHandler = new EventHandler<TArgs>((sender, args) =>
            {
                actual.Add(args);
                countdownEvent.Signal();
            });
            subscriber(eventHandler);
            eventTrigger();
            countdownEvent.Wait(100);
            unsubscriber(eventHandler);
            return actual;
        }

        public static bool WaitEvent(
            Action<Action> subscriber,
            Action<Action> unsubscriber,
            Action eventTrigger)
        {
            var actual = false;
            var countdownEvent = new AutoResetEvent(false);
            var eventHandler = new Action(() =>
            {
                actual = true;
                countdownEvent.Set();
            });
            subscriber(eventHandler);
            eventTrigger();
            countdownEvent.WaitOne(100);
            unsubscriber(eventHandler);
            return actual;
        }
    }
}