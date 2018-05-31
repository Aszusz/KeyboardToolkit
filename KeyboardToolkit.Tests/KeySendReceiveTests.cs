using System.Collections.Generic;
using System.Windows.Input;
using KeyboardToolkit.Common;
using KeyboardToolkit.Receiver;
using KeyboardToolkit.Sender;
using NUnit.Framework;

namespace KeyboardToolkit.Tests
{
    [TestFixture]
    public class KeySendReceiveTests
    {
        private IKeyReceiver _receiver;
        private IKeySender _sender;

        [Test]
        public void SendingKeyShouldFireKeyReceivedEvent()
        {
            var expected = new List<KeyEventArgs>
            {
                new KeyEventArgs(Key.P, Common.KeyState.KeyDown),
                new KeyEventArgs(Key.P, Common.KeyState.KeyUp)
            };

            var actual = EventWaiter.WaitEvent<KeyEventArgs>(
                handler => _receiver.KeyReceived += handler,
                handler => _receiver.KeyReceived -= handler,
                () => _sender.Send(expected),
                2);

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void SendingKeyShouldFireKeyReceivedWhenReinstalled()
        {
            var expected = new List<KeyEventArgs>
            {
                new KeyEventArgs(Key.P, Common.KeyState.KeyDown),
                new KeyEventArgs(Key.P, Common.KeyState.KeyUp)
            };

            _receiver.Uninstall();
            _receiver.Install();

            var actual = EventWaiter.WaitEvent<KeyEventArgs>(
                handler => _receiver.KeyReceived += handler,
                handler => _receiver.KeyReceived -= handler,
                () => _sender.Send(expected),
                2);

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void SendingKeyShouldNotFireKeyReceivedWhenUninstalled()
        {
            var toSend = new List<KeyEventArgs>
            {
                new KeyEventArgs(Key.P, Common.KeyState.KeyDown),
                new KeyEventArgs(Key.P, Common.KeyState.KeyUp)
            };

            _receiver.Uninstall();

            var actual = EventWaiter.WaitEvent<KeyEventArgs>(
                handler => _receiver.KeyReceived += handler,
                handler => _receiver.KeyReceived -= handler,
                () => _sender.Send(toSend),
                2);

            Assert.That(actual, Is.Empty);
        }

        [SetUp]
        public void SetUp()
        {
            _sender = new KeySender();
            _receiver = new KeyReceiver();
            _receiver.Install();
        }

        [TearDown]
        public void TearDown()
        {
            _receiver.Dispose();
        }
    }
}