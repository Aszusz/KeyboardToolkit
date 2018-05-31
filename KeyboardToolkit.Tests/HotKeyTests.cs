using System.Windows.Forms;
using System.Windows.Input;
using KeyboardToolkit.Common;
using KeyboardToolkit.HotKeys;
using KeyboardToolkit.Receiver;
using KeyboardToolkit.Sender;
using NUnit.Framework;
using KeyEventArgs = KeyboardToolkit.Common.KeyEventArgs;

namespace KeyboardToolkit.Tests
{
    [TestFixture]
    public class HotKeyTests
    {
        private IKeyReceiver _receiver;
        private IKeySender _sender;

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

        [Test]
        public void RegisteredHotKeyShoudReact()
        {
            var hotKey = HotKey.Create(Keys.A, KeyModifiers.None);
            hotKey.Register();

            var toSend = new[]
            {
                new KeyEventArgs(Key.A, Common.KeyState.KeyDown),
                new KeyEventArgs(Key.A, Common.KeyState.KeyUp)
            };

            var actual = EventWaiter.WaitEvent(
                handler => hotKey.Pressed += handler,
                handler => hotKey.Pressed -= handler,
                () =>
                {
                    _sender.Send(toSend);
                });

            hotKey.Unregister();

            Assert.True(actual);
            hotKey.Dispose();
        }

        [Test]
        public void UnregisteredHotKeyShoudNotReact()
        {
            var hotKey = HotKey.Create(Keys.A, KeyModifiers.None);
            hotKey.Register();
            hotKey.Unregister();

            var toSend = new[]
            {
                new KeyEventArgs(Key.A, Common.KeyState.KeyDown),
                new KeyEventArgs(Key.A, Common.KeyState.KeyUp)
            };

            var actual = EventWaiter.WaitEvent(
                handler => hotKey.Pressed += handler,
                handler => hotKey.Pressed -= handler,
                () =>
                {
                    _sender.Send(toSend);
                });

            hotKey.Unregister();

            Assert.False(actual);
            hotKey.Dispose();
        }

        [Test]
        public void MultipleRegistrationsAndUnregistrationsShouldNotRiseException()
        {
            var hotKey = HotKey.Create(Keys.A, KeyModifiers.None);
            
            Assert.That(() =>
            {
                hotKey.Register();
                hotKey.Register();
                hotKey.Register();
                hotKey.Unregister();
                hotKey.Unregister();
                hotKey.Unregister();
            }, Throws.Nothing);
        }

        [Test]
        public void DisposedHotKeyDoesNotThrowOnSend()
        {
            var hotKey = HotKey.Create(Keys.A, KeyModifiers.None);
            hotKey.Register();
            hotKey.Dispose();

            var toSend = new[]
            {
                new KeyEventArgs(Key.A, Common.KeyState.KeyDown),
                new KeyEventArgs(Key.A, Common.KeyState.KeyUp)
            };

            var actual = EventWaiter.WaitEvent(
                handler => hotKey.Pressed += handler,
                handler => hotKey.Pressed -= handler,
                () =>
                {
                    _sender.Send(toSend);
                });

            Assert.False(actual);
        }
    }
}