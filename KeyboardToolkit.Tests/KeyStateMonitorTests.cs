using System.Threading;
using System.Windows.Input;
using KeyboardToolkit.Common;
using KeyboardToolkit.Receiver;
using KeyboardToolkit.Sender;
using KeyboardToolkit.StateMonitor;
using NUnit.Framework;

namespace KeyboardToolkit.Tests
{
    [TestFixture]
    public class KeyStateMonitorTests
    {
        private IKeyStateMonitor _monitor;
        private IKeyReceiver _receiver;
        private IKeySender _sender;

        [SetUp]
        public void SetUp()
        {
            _sender = new KeySender();
            _receiver = new KeyReceiver();
            _receiver.Install();
            _monitor = new KeyStateMonitor();
        }

        [TearDown]
        public void TearDown()
        {
            _receiver.Dispose();
        }

        [Test]
        public void ModalKeyShouldBeDownDuringReleasingKeyCombination()
        {
            var actual = KeyState.KeyUp;
            var are = new AutoResetEvent(false);

            _receiver.KeyReceived += (sender, args) =>
            {
                if (args.Key == Key.S && args.KeyState == KeyState.KeyUp)
                {
                    actual = _monitor.GetKeyState(Key.LeftCtrl);
                    are.Set();
                }
            };

            _sender.Send(new[] {new KeyEventArgs(Key.LeftCtrl, KeyState.KeyDown)});
            _sender.Send(new[] {new KeyEventArgs(Key.S, KeyState.KeyDown)});
            _sender.Send(new[] {new KeyEventArgs(Key.S, KeyState.KeyUp)});
            _sender.Send(new[] {new KeyEventArgs(Key.LeftCtrl, KeyState.KeyUp)});

            are.WaitOne(100);
            Assert.That(actual, Is.EqualTo(KeyState.KeyDown));
        }
    }
}