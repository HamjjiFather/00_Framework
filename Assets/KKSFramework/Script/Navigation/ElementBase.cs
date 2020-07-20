using UnityEngine;

namespace KKSFramework.Navigation
{
    public abstract class ElementBase : IElementBase
    {
        public abstract void SetElement ();
    }

    public abstract class ElementBase<T> : MonoBehaviour, IElementBase<T>
    {
        public abstract T ElementData { get; set; }

        public abstract void SetElement (T elementData);
    }
    
    public interface IElementBase
    {
        void SetElement ();
    }
    
    public interface IElementBase<T>
    {
        T ElementData { get; set; }
        
        void SetElement (T elementData);
    }
}