﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using KeyboardToolkit.Common;

namespace KeyboardToolkit.HotKeys
{
    public class HotKey : IDisposable, IHotKey
    {
        private static readonly Dictionary<int, HotKey> HotKeys;
        private static readonly object Lock;
        private static int _nextId;

        static HotKey()
        {
            HotKeyManager.HotKeyPressed += HotKeyManagerOnHotKeyPressed;
            HotKeys = new Dictionary<int, HotKey>();
            _nextId = 0;
            Lock = new object();
        }

        private HotKey(Key key, ModifierKeys modifiers, int id)
        {
            Key = key;
            Modifiers = modifiers;
            Id = id;
        }

        public event Action Pressed;

        public int Id { get; }

        public Key Key { get; }

        public ModifierKeys Modifiers { get; }

        public static IHotKey Create(Key key, ModifierKeys modifiers)
        {
            lock (Lock)
            {
                var hotKey = new HotKey(key, modifiers, _nextId);
                _nextId++;
                HotKeys.Add(hotKey.Id, hotKey);
                return hotKey;
            }
        }

        public void Dispose()
        {
            lock (Lock)
            {
                Unregister();
                HotKeys[Id] = null;
            }
        }

        public void Register()
        {
            HotKeyManager.RegisterHotKey(Key, Modifiers, Id);
        }

        public void Unregister()
        {
            HotKeyManager.UnregisterHotKey(Id);
        }

        protected virtual void RaisePressed()
        {
            Pressed?.Invoke();
        }

        private static void HotKeyManagerOnHotKeyPressed(object sender, HotKeyEventArgs hotKeyEventArgs)
        {
            HotKeys[hotKeyEventArgs.Id].RaisePressed();
        }
    }
}