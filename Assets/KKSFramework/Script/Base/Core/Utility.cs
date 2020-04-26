using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Random = UnityEngine.Random;

namespace KKSFramework
{
    public static class Utility
    {
        /// <summary>
        /// clear all of console log.
        /// </summary>
        public static void ClearConsole()
        {
            var logEntries = Type.GetType("UnityEditorInternal.LogEntries, UnityEditor.dll");
            var mclear = logEntries?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            mclear?.Invoke(null, null);
        }

        /// <summary>
        /// Type에 해당하는 enum을 랜덤으로 1개 리턴함.
        /// </summary>
        public static Enum ReturnRandomEnum(Type p_type)
        {
            var temp_array = Enum.GetValues(p_type);
            return (Enum) temp_array.GetValue(Random.Range(0, temp_array.Length));
        }

        /// <summary>
        /// 문자열과 같은 이름을 가진 T 타입에 해당하는 enum 타입을 리턴.
        /// </summary>
        public static T ReturnEnumTypeValue<T>(string enumName) where T : Enum
        {
            var @enum = Enum.GetValues(typeof(T)).Cast<T>().ToList().Find(x => x.ToString().Equals(enumName));
            return @enum != null ? @enum : default;
        }

        /// <summary>
        /// 단계별 차등 FilledAmount 적용 값을 리턴.
        /// ex: 100, 200, 500의 단계 데이터, 값 150일 경우 0.33.. + 0.16..이 되어  0.5의 값을 리턴 함.
        /// ex: 5, 10, 15, 30의 단계 데이터, 값 17일 경우 0.25 + 0.25 + 0.25 + 0.033..이 되어 + 0.7833..의 값을 리턴 함.
        /// </summary>
        /// <returns></returns>
        public static float ReturnSteppedFilledAmount(float p_value, List<float> p_list_step_amount)
        {
            var temp_count = p_list_step_amount.Count;
            float temp_value = 0;
            for (var i = 0; i < temp_count; i++)
            {
                float temp_prev_value = 0;
                if (i == 0)
                    temp_prev_value = 0;
                else
                    temp_prev_value = p_list_step_amount[i - 1];

                if (p_value < p_list_step_amount[i])
                {
                    temp_value += 1f / temp_count *
                                  ((p_value - temp_prev_value) / (p_list_step_amount[i] - temp_prev_value));
                    break;
                }

                temp_value += 1f / temp_count;
            }

            return temp_value;
        }


        #region Compare

        public static bool Or<T> (T target, T arg1, T arg2) where T : IComparable
        {
            return target.CompareTo (arg1) == 0
                   || target.CompareTo (arg2) == 0;
        }


        public static bool Or<T> (T target, T arg1, T arg2, T arg3) where T : IComparable
        {
            return target.CompareTo (arg1) == 0
                   || target.CompareTo (arg2) == 0
                   || target.CompareTo (arg3) == 0;
        }

        
        public static bool Or<T> (T target, T arg1, T arg2, T arg3, T arg4) where T : IComparable
        {
            return target.CompareTo (arg1) == 0
                   || target.CompareTo (arg2) == 0
                   || target.CompareTo (arg3) == 0
                   || target.CompareTo (arg4) == 0;
        }
        
        public static bool Or<T> (T target, T arg1, T arg2, T arg3, T arg4, T arg5) where T : IComparable
        {
            return target.CompareTo (arg1) == 0
                   || target.CompareTo (arg2) == 0
                   || target.CompareTo (arg3) == 0
                   || target.CompareTo (arg4) == 0
                   || target.CompareTo (arg5) == 0;
        }


        public static bool And<T> (T target, T arg1, T arg2) where T : IComparable
        {
            return target.CompareTo (arg1) == 0
                   && target.CompareTo (arg2) == 0;
        }


        public static bool And<T> (T target, T arg1, T arg2, T arg3) where T : IComparable
        {
            return target.CompareTo (arg1) == 0
                   && target.CompareTo (arg2) == 0
                   && target.CompareTo (arg3) == 0;
        }

        
        public static bool And<T> (T target, T arg1, T arg2, T arg3, T arg4) where T : IComparable
        {
            return target.CompareTo (arg1) == 0
                   && target.CompareTo (arg2) == 0
                   && target.CompareTo (arg3) == 0
                   && target.CompareTo (arg4) == 0;
        }
        
        public static bool And<T> (T target, T arg1, T arg2, T arg3, T arg4, T arg5) where T : IComparable
        {
            return target.CompareTo (arg1) == 0
                   && target.CompareTo (arg2) == 0
                   && target.CompareTo (arg3) == 0
                   && target.CompareTo (arg4) == 0
                   && target.CompareTo (arg5) == 0;
        }
        

        #endregion
    }
}