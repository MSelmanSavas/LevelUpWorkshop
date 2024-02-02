using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UsefulExtensions.MonoBehaviour
{
    public static class BehaviourExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="instanceRef"></param>
        /// <param name="destroyOtherGameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if this behaviour becomes a singleton</returns>
        public static bool MakeSingleton<T>(this T behaviour, ref T instanceRef, bool destroyOtherGameObject = false) where T : UnityEngine.MonoBehaviour
        {
            if (instanceRef == null)
            {
                instanceRef = behaviour;
                Object.DontDestroyOnLoad(behaviour.gameObject);
                return true;
            }

            if (destroyOtherGameObject && instanceRef.gameObject != behaviour.gameObject)
            {
                Object.Destroy(behaviour.gameObject);
            }
            else
            {
                Object.Destroy(behaviour);
            }

            return false;
        }

        public static bool MakeSingletonDestroyOthers<T>(this T behaviour, ref T instanceRef) where T : UnityEngine.MonoBehaviour
        {
            return MakeSingleton(behaviour, ref instanceRef, true);
        }

        public static void CheckStopCoroutine(this UnityEngine.MonoBehaviour @this, IEnumerator coroutine)
        {
            if (coroutine != null)
            {
                @this.StopCoroutine(coroutine);
            }
        }

        public static void CheckStopCoroutine(this UnityEngine.MonoBehaviour @this, Coroutine coroutine)
        {
            if (coroutine != null)
            {
                @this.StopCoroutine(coroutine);
            }
        }

        public static Coroutine StopStartCoroutine(this UnityEngine.MonoBehaviour @this, ref IEnumerator storedCor, IEnumerator newCor)
        {
            @this.CheckStopCoroutine(storedCor);

            storedCor = newCor;
            return @this.StartCoroutine(newCor);
        }

        public static Coroutine StopStartCoroutine(this UnityEngine.MonoBehaviour @this, ref Coroutine storedCor, IEnumerator newCor)
        {
            @this.CheckStopCoroutine(storedCor);
            return storedCor = @this.StartCoroutine(newCor);
        }

        public static Coroutine RunDelayed(this UnityEngine.MonoBehaviour @this, float delay, Action callback) =>
            @this.StartCoroutine(RunDelayedCoroutine(delay, callback));

        public static Coroutine RunDelayed<T>(this UnityEngine.MonoBehaviour @this, float delay, Action<T> callback, T param) =>
            @this.StartCoroutine(RunDelayedCoroutine(delay, callback, param));

        public static Coroutine RunDelayed<T1, T2>(this UnityEngine.MonoBehaviour @this, float delay, Action<T1, T2> callback, T1 param1, T2 param2) =>
            @this.StartCoroutine(RunDelayedCoroutine(delay, callback, param1, param2));

        private static IEnumerator RunDelayedCoroutine(float delay, Action callback)
        {
            float waitUntil = Time.time + delay;
            while (Time.time < waitUntil)
                yield return null;

            callback();
        }

        private static IEnumerator RunDelayedCoroutine<T>(float delay, Action<T> callback, T param)
        {
            float waitUntil = Time.time + delay;
            while (Time.time < waitUntil)
                yield return null;

            callback(param);
        }

        private static IEnumerator RunDelayedCoroutine<T1, T2>(float delay, Action<T1, T2> callback, T1 param1, T2 param2)
        {
            float waitUntil = Time.time + delay;
            while (Time.time < waitUntil)
                yield return null;

            callback(param1, param2);
        }

        public static T GetComponentOnlyInChildren<T>(this UnityEngine.MonoBehaviour script, bool includeInactive = false) where T : class
        {
            T component = null;
            //collect only if its an interface or a Component
            if (typeof(T).IsInterface
             || typeof(T).IsSubclassOf(typeof(Component))
             || typeof(T) == typeof(Component))
            {
                foreach (UnityEngine.Transform child in script.transform)
                {
                    component = child.GetComponentInChildren<T>(includeInactive);

                    if (component != null)
                        return component;
                }
            }

            return null;
        }

        public static T[] GetComponentsOnlyInChildren<T>(this UnityEngine.MonoBehaviour script, bool includeInactive = false) where T : class
        {
            List<T> group = new List<T>();

            //collect only if its an interface or a Component
            if (typeof(T).IsInterface
             || typeof(T).IsSubclassOf(typeof(Component))
             || typeof(T) == typeof(Component))
            {
                foreach (UnityEngine.Transform child in script.transform)
                {
                    group.AddRange(child.GetComponentsInChildren<T>(includeInactive));
                }
            }

            return group.ToArray();
        }
    }


}
