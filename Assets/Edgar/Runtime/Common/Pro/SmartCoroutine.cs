using System;
using System.Collections;
using UnityEngine;

namespace Edgar.Unity
{
    /// <summary>
    /// Smart coroutine that can catch exceptions and store return values.
    /// <example>
    /// Example usage:
    /// <code>
    ///    var coroutineObject = new SmartCoroutine<TValue>(coroutine, throwImmediately);
    ///    coroutineObject.Coroutine = monoBehaviour.StartCoroutine(coroutineObject.InternalCoroutine);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class SmartCoroutine<TValue>
    {
        private TValue returnValue;

        /// <summary>
        /// Coroutine that wraps the smart coroutine.
        /// Must be set manually after calling StartCoroutine(coroutineObject.InternalCoroutine).
        /// </summary>
        public Coroutine Coroutine { get; set; }

        /// <summary>
        /// Actual logic of the coroutine.
        /// </summary>
        public IEnumerator InternalCoroutine { get; }

        /// <summary>
        /// Whether the coroutine has already finished.
        /// </summary>
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Exception that was thrown during execution, null if there was no exception.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Whether there was an exception or not.
        /// </summary>
        public bool IsSuccessful => Exception == null;

        /// <summary>
        /// Value that was returned from the coroutine.
        /// If no value was returned, default value is returned by this property.
        /// </summary>
        public TValue Value
        {
            get
            {
                if (Exception != null)
                {
                    throw Exception;
                }

                return returnValue;
            }
        }

        /// <summary>
        /// Initialize the smart coroutine with another coroutine.
        /// </summary>
        /// <param name="coroutine">Coroutine that should be wrapped in the smart coroutine.</param>
        /// <param name="throwImmediately">Whether to throw immediately if an exception is encountered.</param>
        public SmartCoroutine(IEnumerator coroutine, bool throwImmediately = false)
        {
            InternalCoroutine = InternalRoutine(coroutine, throwImmediately);
        }

        /// <summary>
        /// Rethrow the exception if there was any.
        /// </summary>
        public void ThrowIfNotSuccessful()
        {
            if (Exception != null)
            {
                throw Exception;
            }
        }

        private IEnumerator InternalRoutine(IEnumerator coroutine, bool throwImmediately = false)
        {
            while (true)
            {
                try
                {
                    if (!coroutine.MoveNext())
                    {
                        IsFinished = true;
                        yield break;
                    }
                }
                catch (Exception e)
                {
                    if (throwImmediately)
                    {
                        throw;
                    }

                    Exception = e;
                    yield break;
                }

                var yielded = coroutine.Current;
                if (yielded != null && yielded.GetType() == typeof(TValue))
                {
                    returnValue = (TValue)yielded;
                    IsFinished = true;
                    yield break;
                }

                yield return coroutine.Current;
            }
        }
    }
}