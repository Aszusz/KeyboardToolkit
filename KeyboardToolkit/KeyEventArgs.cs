﻿using System;
using System.Windows.Input;

namespace KeyboardToolkit
{
    public class KeyEventArgs : EventArgs, IEquatable<KeyEventArgs>
    {
        public KeyEventArgs(Key key, KeyAction keyAction)
        {
            KeyAction = keyAction;
            Key = key;
        }

        public Key Key { get; }

        public KeyAction KeyAction { get; }

        public static bool operator ==(KeyEventArgs left, KeyEventArgs right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(KeyEventArgs left, KeyEventArgs right)
        {
            return !Equals(left, right);
        }

        public bool Equals(KeyEventArgs other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Key == other.Key && KeyAction == other.KeyAction;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType()
                   && Equals((KeyEventArgs) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Key*397) ^ (int) KeyAction;
            }
        }
    }
}