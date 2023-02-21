using System;

namespace Reactive
{
    public interface IReadOnlyReactiveTrigger
    {
        IDisposable Subscribe(Action action);
    }
    
    public interface IReadOnlyReactiveTrigger<out T>
    {
        IDisposable Subscribe(Action<T> action);
    }
    
    public interface IReadOnlyReactiveTrigger<out T1, out T2>
    {
        IDisposable Subscribe(Action<T1, T2> action);
    } 
}