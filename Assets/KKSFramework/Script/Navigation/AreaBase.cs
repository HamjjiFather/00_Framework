using UnityEngine;

namespace KKSFramework.Navigation
{
    public abstract class AreaBase<T> : MonoBehaviour
    {
        public abstract void SetArea (T areaData);
    }
}