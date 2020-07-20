using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace KKSFramework.DesignPattern
{
    public abstract class ViewModelBase<T> : ViewModelBase where T : ModelBase
    {
        protected abstract T ModelBase { get; set; }
        protected readonly ReactiveCommand<T> ModelChangedCommand = new ReactiveCommand<T> ();
        
        
        public virtual void RegistReactiveCommand (Action<T> subscribeAction)
        {
            ModelChangedCommand.RegistModelReactiveCommand (subscribeAction);
            ModelChangedCommand.Execute (ModelBase);
        }
        
        
        public virtual void RegistReactiveCommand (Action<T> subscribeAction, MonoBehaviour target)
        {
            ModelChangedCommand.RegistModelReactiveCommand (subscribeAction, target);
            ModelChangedCommand.Execute (ModelBase);
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
        public virtual void InitAfterLoadTableData() {}
        
        /// <summary>
        /// 로컬 데이터를 로드하고 뷰모델 세팅.
        /// </summary>
        public virtual void InitAfterLoadLocalData() {}
    }
}