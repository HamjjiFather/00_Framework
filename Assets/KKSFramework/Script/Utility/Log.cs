using System;
using UnityEngine;

namespace KKSFramework
{
    public enum LogCategory
    {
        Service,
        System
    }


    public static class Log
    {
        public static void Verbose(LogCategory category, object message)
        {
            Verbose(category.ToString(), message);
        }


        public static void Warning(LogCategory category, object message)
        {
            Warning(category.ToString(), message);
        }


        public static void Error(LogCategory category, object message)
        {
            Error(category.ToString(), message);
        }


        private static string GetHeader(string category)
        {
            return $"[{category}] ";
        }


        public static void Verbose(string category, object message)
        {
            Debug.Log($"{GetHeader(category)}{message}");
        }


        public static void Warning(string category, object message)
        {
            Debug.LogWarning($"{GetHeader(category)}{message}");
        }


        public static void Error(string category, object message)
        {
            Debug.LogError($"{GetHeader(category)}{message}");
        }


        public static void ErrorException(string category, object message, Exception ex)
        {
            Debug.LogError($"{GetHeader(category)}{message}");
            Debug.LogException(ex);
        }
    }
}