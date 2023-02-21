using System;
using UniRx;

namespace Reactive
{
    public class ReactiveTrigger : IDisposable, IReadOnlyReactiveTrigger
    {
        private readonly Subject<bool> _subject;

        public ReactiveTrigger()
        {
            _subject = new Subject<bool>();
        }

        public IDisposable Subscribe(Action action)
        {
            return _subject.Subscribe(_ => action?.Invoke());
        }

        public void Notify()
        {
            _subject.OnNext(true);
        }

        public void Dispose()
        {
            _subject.Dispose();
        }
    }

    public class ReactiveTrigger<T> : IDisposable, IReadOnlyReactiveTrigger<T>
    {
        private readonly Subject<T> _subject;

        public ReactiveTrigger()
        {
            _subject = new Subject<T>();
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return _subject.Subscribe(action);
        }

        public void Notify(T value)
        {
            _subject.OnNext(value);
        }

        public void Dispose()
        {
            _subject.Dispose();
        }
    }

    public class ReactiveTrigger<T1, T2> : IDisposable, IReadOnlyReactiveTrigger<T1, T2>
    {
        public struct Entry
        {
            public T1 arg1;
            public T2 arg2;
        }

        private readonly Subject<Entry> _subject;

        public ReactiveTrigger()
        {
            _subject = new Subject<Entry>();
        }

        public IDisposable Subscribe(Action<T1, T2> action)
        {
            return _subject.Subscribe(e => action?.Invoke(e.arg1, e.arg2));
        }

        public void Notify(T1 arg1, T2 arg2)
        {
            _subject.OnNext(new Entry { arg1 = arg1, arg2 = arg2 });
        }

        public void Dispose()
        {
            _subject.Dispose();
        }
    }
}