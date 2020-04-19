using System.Collections;
using KKSFramework.Navigation;
using KKSFramework.SceneLoad;
using UnityEngine;

namespace KKSFramework
{
    public class EntryPageView : PageViewBase
    {
        private const float WaitSeconds = 2f;

        protected override void Showed()
        {
            base.Showed();
            StartCoroutine(WaitEntryPage());
        }

        private IEnumerator WaitEntryPage()
        {
            yield return SceneLoadManager.Instance.LoadSceneAsync(SceneType.Bootstrap, true);
            yield return new WaitForSeconds(WaitSeconds);
            yield return SceneLoadManager.Instance.ChangeSceneAsync(SceneType.Title);
        }
    }
}