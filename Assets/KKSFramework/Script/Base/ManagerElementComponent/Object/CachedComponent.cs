﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐시 컴포넌트 클래스.
/// 2018.06.03. 작성.
/// </summary>
public class CachedComponent : MonoBehaviour
{
    #region Fields & Property

    /// <summary>
    /// 이 오브젝트에 캐시된 컴포넌트 딕셔너리.
    /// </summary>
    private readonly Dictionary<Type, Component> _cachedComponentDict = new Dictionary<Type, Component>();

    #endregion

    #region Methods

    /// <summary>
    /// 캐시된 컴포넌트 리턴.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Component GetCachedComponent(Type type)
    {
        _cachedComponentDict.TryGetValue(type, out var tempComponent);
        return tempComponent ? tempComponent : _cachedComponentDict[type] = GetComponent(type);
    }

    /// <summary>
    /// 컴포넌트를 추가하고 캐시된 제네릭 컴포넌트 리턴.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Component AddCachedComponent(Type type)
    {
        var tempComponent = gameObject.AddComponent(type);
        _cachedComponentDict.Add(type, tempComponent);
        return tempComponent ? tempComponent : _cachedComponentDict[type] = GetComponent(type);
    }

    /// <summary>
    /// 캐시된 제네릭 컴포넌트 리턴.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetCachedComponent<T>() where T : Component
    {
        return (T) GetCachedComponent(typeof(T));
    }

    /// <summary>
    /// 컴포넌트를 추가하고 캐시된 제네릭 컴포넌트 리턴.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T AddCachedComponent<T>() where T : Component
    {
        return (T) AddCachedComponent(typeof(T));
    }

    #endregion
}