using System;
using UniRx;
using UnityEngine;

namespace KKSFramework.DesignPattern
{
    public static class DesignPatternExtension
    {
        public static void RegistModelReactiveCommand<T> (this ReactiveCommand<T> reactiveCommand, Action<T> subscribeAction, MonoBehaviour target) where T : ModelBase
        {
            reactiveCommand
                .TakeUntilDisable (target)
                .Subscribe (subscribeAction);
        }
    }
}