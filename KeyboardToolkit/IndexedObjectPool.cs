using System;
using System.Collections.Generic;

namespace KeyboardToolkit
{
    public class IndexedObjectPool<T>
    {
        private readonly Func<int, T> _constructor;
        private readonly object _lock;
        private readonly Dictionary<int, T> _objects;
        private int _counter;

        public IndexedObjectPool(Func<int, T> constructor)
        {
            _constructor = constructor;
            _counter = 0;
            _objects = new Dictionary<int, T>();
            _lock = new object();
        }

        public T Create()
        {
            lock (_lock)
            {
                var instance = _constructor(_counter);
                _objects.Add(_counter, instance);
                _counter++;
                return instance;
            }
        }

        public T Get(int index)
        {
            return _objects[index];
        }

        public void Release(int index)
        {
            lock (_lock)
            {
                _objects[index] = default(T);
            }
        }
    }
}