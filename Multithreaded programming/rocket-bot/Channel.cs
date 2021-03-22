using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        private int lastIndex = -1;
        private readonly List<T> collection = new List<T>();
        /// <summary>
        /// Возвращает элемент по индексу или null, если такого элемента нет.
        /// При присвоении удаляет все элементы после.
        /// Если индекс в точности равен размеру коллекции, работает как Append.
        /// </summary>
        public T this[int index]
        {
            get
            {
                lock (collection)
                    return lastIndex >= index ? collection[index] : null;
            }
            set
            {
                lock (collection)
                {
                    if (index < collection.Count)
                        collection[index] = value;
                    else
                        collection.Add(value);
                    lastIndex = index;
                }
            }
        }

        /// <summary>
        /// Возвращает последний элемент или null, если такого элемента нет
        /// </summary>
        public T LastItem()
        {
            lock (collection)
                return lastIndex < 0? null: collection[lastIndex];
        }

        /// <summary>
        /// Добавляет item в конец только если lastItem является последним элементом
        /// </summary>
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (collection)
            {
                if (collection.Count > lastIndex && lastIndex >= 0)
                    if (collection[lastIndex] != knownLastItem)
                        return;
                this[lastIndex+1] = item;
            }
        }

        /// <summary>
        /// Возвращает количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                lock (collection)
                    return lastIndex+1;
            }
        }
    }
}