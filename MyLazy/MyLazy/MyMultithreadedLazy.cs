﻿using System;

namespace MyLazy
{
    /// <summary>
    /// Мультипопоточная версия Lazy с гарантией корректной работы в многопоточном режиме
    /// </summary>
    public class MyMultithreadedLazy<T> : ILazy<T>
    {
        /// <summary>
        /// Переменная, хранящая результат вычисления
        /// </summary>
        private T result;

        /// <summary>
        /// Объект, предоставляющий вычисление
        /// </summary>
        private Func<T> supplier;

        /// <summary>
        /// Флаг, если вычисление уже произошло, то true
        /// </summary>
        private volatile bool counted = false;

        private object locker = new object();

        public MyMultithreadedLazy(Func<T> supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException();
            }
            this.supplier = supplier;
        }

        /// <summary>
        /// Возвращает вычисленное значение
        /// </summary>
        public T Get()
        {
            if (!counted)
            {
                lock (locker)
                {
                    if (counted)
                    {
                        return result;
                    }

                    result = supplier();
                    supplier = null;
                    counted = true;
                    return result;
                }
            }
            return result;
        }
    }
}
