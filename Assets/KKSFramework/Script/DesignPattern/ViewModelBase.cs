using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace KKSFramework.DesignPattern
{
    public abstract class ViewModelBase<T> : ViewModelBase where T : ModelBase
    {
        protected T ModelBase;
        protected readonly ReactiveCommand<T> TestReactiveCommand = new ReactiveCommand<T> ();
        
        public virtual void RegistReactiveCommand (Action<T> subscribeAction, MonoBehaviour target)
        {
            TestReactiveCommand.RegistModelReactiveCommand (subscribeAction, target);
            TestReactiveCommand.Execute (ModelBase);
        }
    }
    
    [UsedImplicitly]
    public abstract class ViewModelBase
    {
        /// <summary>
        /// 뷰모델 초기 세팅.
        /// </summary>
        public abstract void Initialize ();
        
        /// <summary>
        /// 테이블 데이터를 로드하고 뷰모델 세팅.
        /// </summary>
        public virtual void InitTableData() {}
        
        /// <summary>
        /// 로컬 데이터를 로드하고 뷰모델 세팅.
        /// </summary>
        public virtual void InitLocalData() {}
    }
}
