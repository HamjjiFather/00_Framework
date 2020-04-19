using System;
using System.Collections.Generic;
using System.Linq;
using KKSFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using static System.Guid;

/// <summary>
/// c#, unity 기본 확장 메소드.
/// </summary>
public static class BaseExtension
{
    #region Object

    /// <summary>
    /// 오브젝트의 Z값을 변경하여 Target을 바라보도록 설정.
    /// </summary>
    public static void SetEulerAngle2D(this Transform p_t_transform, Transform p_t_target)
    {
        var temp_v3 = (p_t_target.position - p_t_transform.position).normalized;
        p_t_transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(temp_v3.y, temp_v3.x) * Mathf.Rad2Deg + 90);
    }

    /// <summary>
    /// 트랜스폼을 생성 시 초기화 상태로 설정.
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
    /// 트랜스폼을 생성 시 초기화 상태로 설정.
    /// </summary>
    public static void SetInstantiateTransform (this Transform transform)
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }


    public static void SetActive (this MonoBehaviour monoBehaviour, bool isActive)
    {
        monoBehaviour.gameObject.SetActive (isActive);
    }
    
    #endregion

    #region Disposable

    public static void DisposeSafe(this IDisposable disposable)
    {
        disposable?.Dispose();
    }

    #endregion

    #region Action

    public static void CallSafe(this Action action)
    {
        action?.Invoke();
    }


    public static void CallSafe<T1>(this Action<T1> action, T1 t1)
    {
        action?.Invoke(t1);
    }


    public static void CallSafe<T1, T2>(this Action<T1, T2> action, T1 t1, T2 t2)
    {
        action?.Invoke(t1, t2);
    }


    public static void CallSafe<T1, T2, T3>(this Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
        action?.Invoke(t1, t2, t3);
    }


    public static void CallSafe<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
        action?.Invoke(t1, t2, t3, t4);
    }

    #endregion

    #region Func

    public static T1 CallSafe<T1>(this Func<T1> func)
    {
        return func != null ? func.Invoke() : default;
    }


    public static T2 CallSafe<T1, T2>(this Func<T1, T2> func, T1 t1)
    {
        return func != null ? func.Invoke(t1) : default;
    }


    public static T3 CallSafe<T1, T2, T3>(this Func<T1, T2, T3> func, T1 t1, T2 t2)
    {
        return func != null ? func.Invoke(t1, t2) : default;
    }


    public static T4 CallSafe<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> func, T1 t1, T2 t2, T3 t3)
    {
        return func != null ? func.Invoke(t1, t2, t3) : default;
    }

    #endregion

    #region Vector

    /// <summary>
    /// (확장) 벡터2의 x, y 값을 1개의 값으로 통일하여 리턴.
    /// </summary>
    /// <param name="p_target"></param>
    /// <param name="p_one_value"> 변경 값. </param>
    /// <returns></returns>
    public static Vector2 SetOneValue(this Vector2 p_target, float p_one_value)
    {
        return new Vector2(p_one_value, p_one_value);
    }

    /// <summary>
    /// (확장) 벡터2의 X축만 변경함.
    /// </summary>
    /// <param name="p_target"></param>
    /// <param name="p_y_value"></param>
    /// <returns></returns>
    public static Vector2 X(this Vector2 p_target, float p_value)
    {
        return new Vector2(p_value, p_target.y);
    }

    /// <summary>
    /// (확장) 벡터2의 Y축만 변경함.
    /// </summary>
    /// <param name="p_target"></param>
    /// <param name="p_y_value"></param>
    /// <returns></returns>
    public static Vector2 Y(this Vector2 p_target, float p_value)
    {
        return new Vector2(p_target.x, p_value);
    }

    /// <summary>
    /// (확장) 벡터3의 x, y 값을 벡터2로, z값을 1개의 파라미터 값으로 리턴.
    /// </summary>
    /// <param name="p_target"></param>
    /// <param name="p_v2_value"> x, y 값을 조정할 벡터2 값. </param>
    /// <param name="p_z_value"> z 값. </param>
    /// <returns></returns>
    public static Vector3 SetVector2FloatValue(this Vector3 p_target, Vector2 p_v2_value, float p_z_value)
    {
        return new Vector3(p_v2_value.x, p_v2_value.y, p_z_value);
    }

    /// <summary>
    /// (확장) 벡터3의 x, y, z 값을 1개의 값으로 리턴.
    /// </summary>
    /// <param name="p_target"></param>
    /// <param name="p_one_value"> 변경 값. </param>
    /// <returns></returns>
    public static Vector3 SetOneValue(this Vector3 p_target, float p_one_value)
    {
        return new Vector3(p_one_value, p_one_value, p_one_value);
    }


    /// <summary>
    /// (확장) 벡터3의 X축만 변경함.
    /// </summary>
    /// <param name="p_target"></param>
    /// <param name="p_y_value"></param>
    /// <returns></returns>
    public static Vector3 X(this Vector3 p_target, float p_value)
    {
        return new Vector3(p_value, p_target.y, p_target.z);
    }

    /// <summary>
    /// (확장) 벡터3의 Y축만 변경함.
    /// </summary>
    /// <param name="p_target"></param>
    /// <param name="p_y_value"></param>
    /// <returns></returns>
    public static Vector3 Y(this Vector3 p_target, float p_value)
    {
        return new Vector3(p_target.x, p_value, p_target.z);
    }

    /// <summary>
    /// (확장) 벡터3의 Y축만 변경함.
    /// </summary>
    /// <param name="p_target"></param>
    /// <param name="p_y_value"></param>
    /// <returns></returns>
    public static Vector3 Z(this Vector3 p_target, float p_value)
    {
        return new Vector3(p_target.x, p_target.y, p_value);
    }

    /// <summary>
    /// (확장) 벡터4의 x, y, z, w값을 1개의 값으로 리턴.
    /// </summary>
    /// <param name="p_target"></param>
    /// <param name="p_one_value"> 변경 값. </param>
    /// <returns></returns>
    public static Vector4 SetOneValue(this Vector4 p_target, float p_one_value)
    {
        return new Vector4(p_one_value, p_one_value, p_one_value, p_one_value);
    }

    #endregion

    #region Color

    /// <summary>
    /// (확장) 컬러의 R값만 조정.
    /// </summary>
    public static Color Red(this Color p_target, float p_red)
    {
        return new Color(p_red, p_target.g, p_target.b, p_target.a);
    }

    /// <summary>
    /// (확장) 컬러의 G값만 조정.
    /// </summary>
    public static Color Green(this Color p_target, float p_green)
    {
        return new Color(p_target.r, p_green, p_target.b, p_target.a);
    }

    /// <summary>
    /// (확장) 컬러의 B값만 조정.
    /// </summary>
    public static Color Blue(this Color p_target, float p_blue)
    {
        return new Color(p_target.r, p_target.g, p_blue, p_target.a);
    }

    /// <summary>
    /// (확장) 컬러의 A값만 조정.
    /// </summary>
    public static Color Alpha(this Color p_target, float p_alpha)
    {
        return new Color(p_target.r, p_target.g, p_target.b, p_alpha);
    }

    #endregion

    #region UI

    /// <summary>
    /// (확장) UGUI의 알파값 조정.
    /// </summary>
    /// <param name="p_graphic"></param>
    /// <param name="p_alpha"></param>
    public static void SetAlphaColor(this Graphic p_graphic, float p_alpha)
    {
        p_graphic.color = new Color(p_graphic.color.r, p_graphic.color.g, p_graphic.color.b, p_alpha);
    }

    /// <summary>
    /// (확장) UGUI의 색상값 조정.
    /// </summary>
    public static void SetColor(this Graphic p_graphic, Color p_color)
    {
        p_graphic.color = p_color;
    }

    /// <summary>
    /// (확장) UGUI의 알파값을 포함하지 않은 색상값만 조정.
    /// </summary>
    /// <param name="p_graphic"></param>
    /// <param name="p_alpha"></param>
    public static void SetOnlyColor(this Graphic p_graphic, Color p_color)
    {
        p_graphic.color = new Color(p_color.r, p_color.g, p_color.b, p_graphic.color.a);
    }

    /// <summary>
    /// (확장) UGUI의 색상 조정.
    /// </summary>
    /// <param name="p_graphic"></param>
    /// <param name="p_alpha"></param>
    public static void SetColorByOption(this Graphic p_graphic, StatusColorOption p_option, Color p_color)
    {
        switch (p_option)
        {
            case StatusColorOption.Color:
                SetColor(p_graphic, p_color);
                break;

            case StatusColorOption.ColorOnly:
                SetOnlyColor(p_graphic, p_color);
                break;

            case StatusColorOption.AlphaOnly:
                SetAlphaColor(p_graphic, p_color.a);
                break;

            case StatusColorOption.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(p_option), p_option, null);
        }
    }

    #endregion

    #region Collection

    /// <summary>
    /// 열거형 Foreach. 타입 이벤트.
    /// </summary>
    public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var var in enumerable) action(var);
    }

    
    /// <summary>
    /// 열거형 Foreach, 타입, 인덱스 이벤트.
    /// </summary>
    public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
    {
        var index = 0;
        foreach (var item in enumerable)
        {
            action(item, index);
            index++;
        }
    }
    
    
    /// <summary>
    /// 열거형 랜덤 섞기.
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.OrderBy (x => NewGuid ());
    }
    
    
    /// <summary>
    /// 리스트값이 없을 경우 추가한다.
    /// </summary>
    public static void AddIfNotContains<T>(this List<T> list, T value)
    {
        if (list.Contains(value) == false) list.Add(value);
    }


    /// <summary>
    /// 여러번 Enqueue 처리.
    /// </summary>
    public static void Enqueues<T> (this Queue<T> queue, IEnumerable<T> enumerable)
    {
        enumerable.Foreach (queue.Enqueue);
    }


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
    /// 인덱스에 해당하는 딕셔너리 값을 세팅하거나, 없을 경우 추가.
    /// </summary>
    public static void SetOrAdd<K, V>(this Dictionary<K, V> dict, K key, V value)
    {
        if (dict.ContainsKey(key) == false)
            dict.Add(key, value);
        else
            dict[key] = value;
    }

    public static void SetOrAddForListValue<K, V>(this Dictionary<K, List<V>> dict, K key, V value)
    {
        if (dict.ContainsKey(key) == false)
            dict.Add(key, new List<V>());

        dict[key].Add(value);
    }

    #endregion
}