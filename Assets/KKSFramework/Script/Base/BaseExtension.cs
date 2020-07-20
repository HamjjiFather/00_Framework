using System;
using System.Collections.Generic;
using System.Linq;
using KKSFramework.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Guid;
using Random = UnityEngine.Random;


namespace KKSFramework
{
    /// <summary>
    ///     c#, unity3D base extended functions.
    /// </summary>
    public static class BaseExtension
    {
        #region Disposable

        /// <summary>
        ///     dispose safely.
        /// </summary>
        public static void DisposeSafe (this IDisposable disposable)
        {
            disposable?.Dispose ();
        }

        #endregion


        #region Float

        public static bool IsZero (this float floatValue)
        {
            return floatValue < float.Epsilon;
        }

        #endregion


        #region String

        /// <summary>
        ///     Add tag for formatted text.
        /// </summary>
        public static string AddTag (this string target, string tagName, object value)
        {
            return $"<{tagName}={value}>{target}</{tagName}>";
        }

        #endregion


        #region Nullable

        public static bool IsNull (this object nullable)
        {
            return nullable == null;
        }

        public static bool NotNull (this object nullable)
        {
            return nullable != null;
        }

        #endregion


        #region Boolean

        public static void TrueFlagAction (this bool boolean, params object[] flags)
        {
            flags.Foreach (flag =>
            {
                if (flag is Action<bool> flagAction) flagAction.Invoke (boolean);
            });
        }


        public static void FalseFlagAction (this bool boolean, params object[] flags)
        {
            flags.Foreach (flag =>
            {
                if (flag is Action<bool> flagAction) flagAction.Invoke (!boolean);
            });
        }

        #endregion


        #region Action

        /// <summary>
        ///     invoke action safely.
        /// </summary>
        public static void CallSafe (this Action action)
        {
            action?.Invoke ();
        }


        /// <summary>
        ///     invoke action safely.
        /// </summary>
        public static void CallSafe<T1> (this Action<T1> action, T1 t1)
        {
            action?.Invoke (t1);
        }


        /// <summary>
        ///     invoke action safely.
        /// </summary>
        public static void CallSafe<T1, T2> (this Action<T1, T2> action, T1 t1, T2 t2)
        {
            action?.Invoke (t1, t2);
        }


        /// <summary>
        ///     invoke action safely.
        /// </summary>
        public static void CallSafe<T1, T2, T3> (this Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            action?.Invoke (t1, t2, t3);
        }


        /// <summary>
        ///     invoke action safely.
        /// </summary>
        public static void CallSafe<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            action?.Invoke (t1, t2, t3, t4);
        }

        #endregion


        #region Func

        /// <summary>
        ///     invoke function safely.
        /// </summary>
        public static T1 CallSafe<T1> (this Func<T1> func)
        {
            return func != null ? func.Invoke () : default;
        }


        /// <summary>
        ///     invoke function safely.
        /// </summary>
        public static T2 CallSafe<T1, T2> (this Func<T1, T2> func, T1 t1)
        {
            return func != null ? func.Invoke (t1) : default;
        }


        /// <summary>
        ///     invoke function safely.
        /// </summary>
        public static T3 CallSafe<T1, T2, T3> (this Func<T1, T2, T3> func, T1 t1, T2 t2)
        {
            return func != null ? func.Invoke (t1, t2) : default;
        }


        /// <summary>
        ///     invoke function safely.
        /// </summary>
        public static T4 CallSafe<T1, T2, T3, T4> (this Func<T1, T2, T3, T4> func, T1 t1, T2 t2, T3 t3)
        {
            return func != null ? func.Invoke (t1, t2, t3) : default;
        }

        #endregion


        #region Vector

        /// <summary>
        ///     change X axis value only.
        /// </summary>
        public static Vector2 X (this Vector2 vector2, float value)
        {
            return new Vector2 (value, vector2.y);
        }


        /// <summary>
        ///     change Y axis value only.
        /// </summary>
        public static Vector2 Y (this Vector2 vector2, float value)
        {
            return new Vector2 (vector2.x, value);
        }


        /// <summary>
        ///     change Vector3 to only one value.
        /// </summary>
        public static Vector3 SetOneValue (float value)
        {
            return new Vector3 (value, value, value);
        }


        /// <summary>
        ///     change X axis value only.
        /// </summary>
        public static Vector3 X (this Vector3 vector3, float value)
        {
            return new Vector3 (value, vector3.y, vector3.z);
        }

        /// <summary>
        ///     change Y axis value only.
        /// </summary>
        public static Vector3 Y (this Vector3 vector3, float value)
        {
            return new Vector3 (vector3.x, value, vector3.z);
        }

        /// <summary>
        ///     change Z axis value only.
        /// </summary>
        public static Vector3 Z (this Vector3 vector3, float value)
        {
            return new Vector3 (vector3.x, vector3.y, value);
        }

        #endregion


        #region Color

        /// <summary>
        ///     Change red value only color.
        /// </summary>
        public static Color Red (this Color color, float red)
        {
            return new Color (red, color.g, color.b, color.a);
        }

        /// <summary>
        ///     Change green value only color.
        /// </summary>
        public static Color Green (this Color color, float green)
        {
            return new Color (color.r, green, color.b, color.a);
        }

        /// <summary>
        ///     Change blue value only color.
        /// </summary>
        public static Color Blue (this Color color, float blue)
        {
            return new Color (color.r, color.g, blue, color.a);
        }

        /// <summary>
        ///     Change alpha value only color.
        /// </summary>
        public static Color Alpha (this Color color, float alpha)
        {
            return new Color (color.r, color.g, color.b, alpha);
        }


        /// <summary>
        ///     Convert color to color code.
        /// </summary>
        public static string ToRGBHex (this Color c)
        {
            return $"#{ToByte (c.r):X2}{ToByte (c.g):X2}{ToByte (c.b):X2}";

            byte ToByte (float f)
            {
                f = Mathf.Clamp01 (f);
                return (byte) (f * 255);
            }
        }

        /// <summary>
        ///     Convert color code to color.
        /// </summary>
        public static Color ToColor (string code)
        {
            var r = code.Substring (0, 2);
            var g = code.Substring (2, 2);
            var b = code.Substring (4, 2);
            var a = code.Length == 8 ? code.Substring (6, 2) : "FF";

            return new Color (Convert.ToInt32 (r, 16) / 255f, Convert.ToInt32 (g, 16) / 255f,
                Convert.ToInt32 (b, 16) / 255f, Convert.ToInt32 (a, 16) / 255f);
        }

        #endregion


        #region Transform

        /// <summary>
        ///     Give a targetTransform a look.
        /// </summary>
        public static void SetEulerAngle2D (this Transform transform, Transform targetTransform)
        {
            var tempV3 = (targetTransform.position - transform.position).normalized;
            transform.rotation = Quaternion.Euler (0, 0, Mathf.Atan2 (tempV3.y, tempV3.x) * Mathf.Rad2Deg + 90);
        }


        /// <summary>
        ///     Initializing transform state.
        /// </summary>
        public static void SetInstantiateTransform (this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }


        /// <summary>
        ///     Change position X axis value only.
        /// </summary>
        public static Transform SetPositionX (this Transform transform, float value)
        {
            transform.position = transform.position.X (value);
            return transform;
        }


        /// <summary>
        ///     Change position Y axis value only.
        /// </summary>
        public static Transform SetPositionY (this Transform transform, float value)
        {
            transform.position = transform.position.Y (value);
            return transform;
        }


        /// <summary>
        ///     Change position Z axis value only.
        /// </summary>
        public static Transform SetPositionZ (this Transform transform, float value)
        {
            transform.position = transform.position.Z (value);
            return transform;
        }


        /// <summary>
        ///     Change position to vector2 value.
        /// </summary>
        public static Transform SetPositionXy (this Transform transform, Vector2 pos)
        {
            transform.position = transform.position.X (pos.x).Y (pos.y);
            return transform;
        }


        /// <summary>
        ///     Change local position X axis value only.
        /// </summary>
        public static Transform SetLocalPositionX (this Transform transform, float value)
        {
            transform.localPosition = transform.localPosition.X (value);
            return transform;
        }


        /// <summary>
        ///     Change local position Y axis value only.
        /// </summary>
        public static Transform SetLocalPositionY (this Transform transform, float value)
        {
            transform.localPosition = transform.localPosition.Y (value);
            return transform;
        }


        /// <summary>
        ///     Change local position Z axis value only.
        /// </summary>
        public static Transform SetLocalPositionZ (this Transform transform, float value)
        {
            transform.localPosition = transform.localPosition.Z (value);
            return transform;
        }


        /// <summary>
        ///     Change local position to vector2 value.
        /// </summary>
        public static Transform SetLocalPositionXy (this Transform transform, Vector2 pos)
        {
            transform.localPosition = transform.localPosition.X (pos.x).Y (pos.y);
            return transform;
        }


        public static void MoveTowards (this Transform transform, Vector2 currentPos, Vector2 targetPos,
            float maxDistanceDelta)
        {
            var towardPos = Vector2.MoveTowards (currentPos, targetPos, maxDistanceDelta);
            transform.SetPositionXy (towardPos);
            transform.SetLocalPositionZ (0);
        }


        /// <summary>
        ///     Return children Transform List of transform.
        /// </summary>
        public static IEnumerable<Transform> GetChildTransforms (this Transform transform)
        {
            var children = new List<Transform> ();

            for (var iChild = 0; iChild < transform.childCount; iChild++) children.Add (transform.GetChild (iChild));

            return children;
        }


        /// <summary>
        ///     Return index number.
        /// </summary>
        public static int GetChildIndex (this Transform transform, Transform child)
        {
            var index = child.parent == transform
                ? GetChildTransforms (transform)
                    .Where (t => t.Equals (child))
                    .Select ((x, i) => i)
                    .FirstOrDefault ()
                : -1;

            return index;
        }

        #endregion


        #region RectTransform

        /// <summary>
        ///     Initializing Rect Transform state.
        /// </summary>
        public static void SetInstantiateTransform (this RectTransform transform)
        {
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.sizeDelta = Vector2.zero;
            transform.anchoredPosition = Vector2.zero;
        }


        /// <summary>
        ///     change anchored position X axis value only.
        /// </summary>
        public static RectTransform SetAnchoredPositionX (this RectTransform rectTransform, float x)
        {
            rectTransform.anchoredPosition = rectTransform.anchoredPosition.X (x);
            return rectTransform;
        }


        /// <summary>
        ///     change anchored position Y axis value only.
        /// </summary>
        public static RectTransform SetAnchoredPositionY (this RectTransform rectTransform, float y)
        {
            rectTransform.anchoredPosition = rectTransform.anchoredPosition.Y (y);
            return rectTransform;
        }


        /// <summary>
        ///     change size delta X axis value only.
        /// </summary>
        public static RectTransform SetSizeDeltaX (this RectTransform rectTransform, float x)
        {
            rectTransform.sizeDelta = rectTransform.sizeDelta.X (x);
            return rectTransform;
        }


        /// <summary>
        ///     change size delta Y axis value only.
        /// </summary>
        public static RectTransform SetSizeDeltaY (this RectTransform rectTransform, float y)
        {
            rectTransform.sizeDelta = rectTransform.sizeDelta.X (y);
            return rectTransform;
        }

        #endregion


        #region UI

        /// <summary>
        ///     Add trigger action to eventTrigger.
        /// </summary>
        public static void AddEventTrigger (this EventTrigger eventTrigger, EventTriggerType eventTriggerType,
            UnityAction<BaseEventData> triggerAction)
        {
            var entry = new EventTrigger.Entry
            {
                eventID = eventTriggerType
            };
            entry.callback.AddListener (triggerAction);
            eventTrigger.triggers.Add (entry);
        }


        /// <summary>
        ///     change graphic color.
        /// </summary>
        public static void SetColor (this Graphic graphic, Color color)
        {
            graphic.SetColorByOption (StatusColorOption.Color, color);
        }

        /// <summary>
        ///     Change graphic color only to alpha values.
        /// </summary>
        public static void SetAlphaColor (this Graphic graphic, Color color)
        {
            graphic.SetColorByOption (StatusColorOption.AlphaOnly, color);
        }

        /// <summary>
        ///     Change graphic color only to RGB values.
        /// </summary>
        public static void SetOnlyColor (this Graphic graphic, Color color)
        {
            graphic.SetColorByOption (StatusColorOption.ColorOnly, color);
        }

        /// <summary>
        ///     Change graphic color.
        /// </summary>
        private static void SetColorByOption (this Graphic graphic, StatusColorOption option, Color color)
        {
            Color graphicColor;
            switch (option)
            {
                case StatusColorOption.Color:
                    graphic.color = color;
                    break;

                case StatusColorOption.ColorOnly:
                    graphicColor = graphic.color;
                    graphicColor = new Color (color.r, color.g, color.b, graphicColor.a);
                    graphic.color = graphicColor;
                    break;

                case StatusColorOption.AlphaOnly:
                    graphicColor = graphic.color;
                    graphicColor = new Color (graphicColor.r, graphicColor.g, graphicColor.b, color.a);
                    graphic.color = graphicColor;
                    break;

                case StatusColorOption.None:
                    break;

                default:
                    throw new ArgumentOutOfRangeException (nameof (option), option, null);
            }
        }

        #endregion


        #region Collection

        /// <summary>
        ///     collection's foreach action.
        /// </summary>
        public static void Foreach<T> (this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var var in enumerable) action (var);
        }


        /// <summary>
        ///     collection's foreach action.
        /// </summary>
        public static void Foreach<T> (this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var index = 0;
            foreach (var item in enumerable)
            {
                action (item, index);
                index++;
            }
        }


        public static TSource RandomSource<TSource> (this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentException (nameof (source));

            var sourceArray = source.ToArray ();
            var randValue = Random.Range (0, sourceArray.Length);
            var result = sourceArray[randValue];
            return result;
        }


        public static TSource RandomSource<TSource> (this IEnumerable<TSource> source, Func<TSource, bool> selector)
        {
            return source.Where (selector).RandomSource ();
        }


        public static int RandomIndex<TSource> (this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentException (nameof (source));

            var random = new System.Random ();
            var sourceArray = source.ToArray ();
            return random.Next (sourceArray.Length);
        }


        public static TSource Overlap<TSource> (this IEnumerable<TSource> source, IEnumerable<TSource> target)
        {
            if (source == null || target == null || !source.Any () || !target.Any ())
                throw new ArgumentException (nameof (source), nameof (target));

            using (var enumerator = target.GetEnumerator ())
            {
                while (enumerator.MoveNext ())
                {
                    var cur = enumerator.Current;

                    if (source.Contains (cur)) return cur;

                    enumerator.MoveNext ();
                }
            }

            return default;
        }


        public static IEnumerable<TSource> MinSources<TSource> (this IEnumerable<TSource> sources,
            Func<TSource, int> selector)
        {
            var minValue = sources.Min (selector);
            return sources.Where (x => selector.Invoke (x) == minValue);
        }


        public static IEnumerable<TSource> MinSources<TSource> (this IEnumerable<TSource> sources,
            Func<TSource, float> selector)
        {
            var minValue = sources.Min (selector);
            return sources.Where (x => Math.Abs (selector.Invoke (x) - minValue) < float.Epsilon);
        }


        public static bool FirstOrLast<TSource> (this IEnumerable<TSource> source, TSource element)
        {
            return element.Equals (source.FirstOrDefault ()) || element.Equals (source.LastOrDefault ());
        }


        public static bool ContainIndex<TSource> (this IEnumerable<TSource> source, int index)
        {
            return Enumerable.Range (0, source.Count ()).Contains (index);
        }


        public static int RandomIndex<TSource> (this IEnumerable<TSource> source, Func<TSource, bool> selector)
        {
            if (source == null)
                throw new ArgumentException (nameof (source));

            var random = new System.Random ();
            var sourceArray = source.ToArray ();
            return random.Next (sourceArray.Length);
        }


        public static TSource MaxSource<TSource> (this IEnumerable<TSource> source, Func<TSource, IComparable> selector)
        {
            if (source == null)
                throw new ArgumentException (nameof (source));
            if (selector == null)
                throw new ArgumentException (nameof (selector));
            TSource collection;
            var comparer = Comparer<IComparable>.Default;

            using (var enumerator = source.GetEnumerator ())
            {
                if (!enumerator.MoveNext ())
                    throw new NullReferenceException ();
                var num1 = selector (enumerator.Current);
                collection = enumerator.Current;

                while (enumerator.MoveNext ())
                {
                    var num2 = selector (enumerator.Current);
                    if (comparer.Compare (num1, num2) > 0) continue;
                    num1 = num2;
                    collection = enumerator.Current;
                }
            }

            return collection;
        }


        /// <summary>
        ///     zipping other collections foreach action.
        /// </summary>
        public static void ZipForEach<T, V> (this IEnumerable<T> enumeration, IEnumerable<V> otherEnumerable,
            Action<T, V> action)
        {
            var enumerator = otherEnumerable.GetEnumerator ();
            foreach (var item in enumeration)
            {
                enumerator.MoveNext ();
                action (item, enumerator.Current);
            }

            enumerator.Dispose ();
        }


        /// <summary>
        ///     zipping other collections foreach action.
        /// </summary>
        public static void ZipForEach<T, TV> (this IEnumerable<T> enumeration, IEnumerable<TV> otherEnumerable,
            Action<T, TV, int> action)
        {
            var list = otherEnumerable.ToList ();
            var enumerator = list.GetEnumerator ();
            var index = 0;
            foreach (var item in enumeration)
            {
                enumerator.MoveNext ();
                action (item, enumerator.Current, index++);
            }

            enumerator.Dispose ();
        }


        /// <summary>
        ///     shuffle collection.
        /// </summary>
        public static IEnumerable<T> Shuffle<T> (this IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy (x => NewGuid ());
        }


        /// <summary>
        ///     enqueue many times.
        /// </summary>
        public static void Enqueues<T> (this Queue<T> queue, IEnumerable<T> enumerable)
        {
            enumerable.Foreach (queue.Enqueue);
        }


        /// <summary>
        ///     dequeue specific times.
        /// </summary>
        public static IEnumerable<T> Dequeues<T> (this Queue<T> queue, int count)
        {
            var newCount = Mathf.Min (count, queue.Count);
            if (newCount <= 0)
            {
                Debug.Log ("is Zero Count");
                return null;
            }

            var newQueue = new Queue<T> ();
            while (newCount > 0)
            {
                newQueue.Enqueue (queue.Dequeue ());
                newCount--;
            }

            return newQueue;
        }


        /// <summary>
        ///     If a Key exists, set the value, and if not, add the Key Value to Dictionary.
        /// </summary>
        public static void SetOrAdd<TK, TV> (this Dictionary<TK, TV> dict, TK key, TV value)
        {
            if (dict.ContainsKey (key) == false)
                dict.Add (key, value);
            else
                dict[key] = value;
        }


        /// <summary>
        ///     If not a Key exists, add the Key Value to Dictionary.
        /// </summary>
        public static void ContainAndAdd<TK, TV> (this Dictionary<TK, TV> dict, TK key, TV value)
        {
            if (!dict.ContainsKey (key))
                dict.Add (key, value);
        }


        public static void AddRange<TK, TV> (this Dictionary<TK, TV> dict, Dictionary<TK, TV> targetDict)
        {
            targetDict.Foreach (target => { dict.ContainAndAdd (target.Key, target.Value); });
        }

        #endregion
    }
}