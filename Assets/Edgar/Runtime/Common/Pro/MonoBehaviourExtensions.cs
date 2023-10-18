using System.Collections;
using UnityEngine;

namespace Edgar.Unity
{
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Starts a smart coroutine that wraps a given coroutine object.
        /// </summary>
        /// <typeparam name="TValue">Type of value that can be returned from the coroutine.</typeparam>
        /// <param name="monoBehaviour"></param>
        /// <param name="coroutine">Coroutine that should be run in the smart coroutine.</param>
        /// <param name="throwImmediately">Whether to throw immediately if an exception is encountered.</param>
        /// <returns></returns>
        public static SmartCoroutine<TValue> StartSmartCoroutine<TValue>(this MonoBehaviour monoBehaviour, IEnumerator coroutine, bool throwImmediately = false)
        {
            var coroutineObject = new SmartCoroutine<TValue>(coroutine, throwImmediately);
            coroutineObject.Coroutine = monoBehaviour.StartCoroutine(coroutineObject.InternalCoroutine);
            return coroutineObject;
        }

        /// <summary>
        /// Starts a smart coroutine that wraps a given coroutine object.
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <param name="coroutine">Coroutine that should be run in the smart coroutine.</param>
        /// <param name="throwImmediately">Whether to throw immediately if an exception is encountered.</param>
        /// <returns></returns>
        public static SmartCoroutine<object> StartSmartCoroutine(this MonoBehaviour monoBehaviour, IEnumerator coroutine, bool throwImmediately = false)
        {
            return StartSmartCoroutine<object>(monoBehaviour, coroutine, throwImmediately);
        }
    }
}