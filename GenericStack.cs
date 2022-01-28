using System;
using System.Collections;
using System.Collections.Generic;

namespace GenericStack
{
    /// <summary>
    /// Represents extendable last-in-first-out (LIFO) collection of the specified type T.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
    public class GenericStack<T> : IEnumerable<T>
    {
        /// <summary>
        /// field to specify default size of stack on creation.
        /// </summary>
        private const int defaultSize = 30;
        /// <summary>
        /// stores iterator of the top element of the stack.
        /// </summary>
        private int top;
        /// <summary>
        /// controls version of the generic stack.
        /// </summary>
        protected int version;
        /// <summary>
        /// stores elements of the generic stack.
        /// </summary>
        private T[] stack;
        /// <summary>
        /// Comparer to compare generic types.
        /// </summary>
        private readonly Comparer<T> comparer = Comparer<T>.Default;
        /// <summary>
        /// IEnumerator<T> gives Generic stack support for "Foreach", LINQ.
        /// It adds ability for stack to be iterated.
        /// </summary>
        struct GenericStackEnumerator : IEnumerator<T>
        {
            private readonly GenericStack<T> _stack;
            private int iterator;
            private readonly int _version;
            internal GenericStackEnumerator(GenericStack<T> _stack)
            {
                this._stack = _stack;
                iterator = _stack.top + 1;
                _version = _stack.version;
            }
            public T Current
            {
                get
                {
                    try
                    {
                        return _stack.stack[iterator];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        throw new InvalidOperationException(e.Message);
                    }
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                iterator = -1;
                Console.WriteLine("Disposted Unmanaged code.");
            }

            public bool MoveNext()
            {
                if (_version != _stack.version) throw new InvalidOperationException("Collection modified");

                iterator = iterator - 1;

                if (iterator < 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public void Reset()
            {
                if (_version != _stack.version) throw new InvalidOperationException("Collection modified");

                iterator = _stack.top + 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the stack class that is empty and has the default initial capacity.
        /// </summary>
        public GenericStack() : this(defaultSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the stack class that is empty and has
        /// the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements of stack.</param>
        public GenericStack(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }
            else
            {
                stack = new T[capacity];
                top = -1;
                version = 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the stack class that contains elements copied
        /// from the specified collection and has sufficient capacity to accommodate the
        /// number of elements copied.
        /// </summary>
        /// <param name="collection">The collection to copy elements from.</param>
        public GenericStack(IEnumerable<T> collection)
        {
            T[] array = collection as T[];
            stack = new T[array.Length];

            foreach (var item in array)
            {
                stack[top] = item;
                top = top + 1;
            }
            top = stack.Length - 1;
            version = 0;
        }

        /// <summary>
        /// Gets the number of elements contained in the stack.
        /// </summary>
        public int Count => top + 1;

        /// <summary>
        /// Removes and returns the object at the top of the stack.
        /// </summary>
        /// <returns>The object removed from the top of the stack.</returns>
        public T Pop()
        {
            if (top <= 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                version++;
                var popValue = stack[top];
                stack[top] = default;
                top = top - 1;
                return popValue;
            }
        }

        /// <summary>
        /// Returns the object at the top of the stack without removing it.
        /// </summary>
        /// <returns>The object at the top of the stack.</returns>
        public T Peek()
        {
            if (top < 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                return stack[top];
            }
        }

        /// <summary>
        /// Inserts an object at the top of the stack.
        /// </summary>
        /// <param name="item">The object to push onto the stack.
        /// The value can be null for reference types.</param>
        public void Push(T item)
        {
            if (top == stack.Length - 1)
            {
                T[] arr = new T[2 * stack.Length];
                Array.Copy(stack, 0, arr, 0, top);
                stack = arr;
            }

            top = top + 1;
            stack[top] = item;
            version++;
        }

        /// <summary>
        /// Copies the elements of stack to a new array.
        /// </summary>
        /// <returns>A new array containing copies of the elements of the stack.</returns>
        public T[] ToArray()
        {
            return stack;
        }
        /// <summary>
        /// Reverses stack.
        /// </summary>
        /// <returns>A new array containing reversed stack.</returns>
        public T[] Reverse()
        {
            var arr = new T[stack.Length];

            for (int i = stack.Length - 1; i >= 0; i--)
            {
                arr[stack.Length - i - 1] = stack[i];
            }

            return arr;
        }

        /// <summary>
        /// Determines whether an element is in the stack.
        /// </summary>
        /// <param name="item">The object to locate in the stack. The value can be null for reference types.</param>
        /// <returns>Return true if item is found in the stack; otherwise, false.</returns>
        public bool Contains(T item)
        {
            for (int i = 0; i < stack.Length; i++)
            {
                if (comparer.Compare(stack[i], item) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes all objects from the stack.
        /// Sets top element before first (Empty).
        /// </summary>
        public void Clear()
        {
            stack = new T[0];
            top = -1;
        }

        /// <summary>
        /// Returns an enumerator for the stack.
        /// </summary>
        /// <returns>Return Enumerator object for the stack.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new GenericStackEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }
}

