using System.Threading;
using UniRx.Async;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KKSFramework.Navigation
{
    public class PopupOption : ViewOption
    {
        public Button bgButton;

        public Button closeButton;
        
        public ViewEffector viewEffector;

        public void InitializePopupOption(UnityAction closeAction)
        {
            bgButton.onClick.AddListener(closeAction);
            closeButton.onClick.AddListener(closeAction);
        }
        
        public async UniTask ShowAsync(CancellationToken ct = default)
        {
            await viewEffector.ShowAsync(ct);
        }

        public async UniTask HideAsync(CancellationToken ct = default)
        {
            await viewEffector.HideAsync(ct);
        }
    }
}