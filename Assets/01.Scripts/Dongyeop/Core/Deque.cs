using System.Collections.Generic;

public class Deque<T>
{
    private Stack<T> _stack = new Stack<T>();
    private Queue<T> _queue = new Queue<T>();

    public void Push(T t)
    {
        _stack.Push(t);
        _queue.Enqueue(t);
    }

    public int Count() => _stack.Count;
    public T First() => _stack.Peek();
    public T Last() => _queue.Peek();
}
