using JetBrains.Annotations;

namespace KKSFramework.Management
{
    public class ManagerBase<T> : Singleton<T>, IManagerBase where T : new()
    {
        /// <summary>
        /// 컴포넌트 베이스 클래스.
        /// </summary>
        [CanBeNull] protected ComponentBase ComponentBase;

        /// <summary>
        /// 매니저 클래스 초기화 컴포넌트 베이스 클래스 세팅.
        /// </summary>
        public virtual void InitManager()
        {
        }

        public virtual void AddComponentBase(ComponentBase componentBase)
        {
            ComponentBase = componentBase;
        }
    }

    public interface IManagerBase
    {
        void InitManager();

        void AddComponentBase(ComponentBase componentBase);
    }
}