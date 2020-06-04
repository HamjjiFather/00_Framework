using System.Collections.Generic;
using System.Linq;
using BaseFrame;
using KKSFramework.Management;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KKSFramework.Event
{
    public struct InputTouch
    {
        public int pointerId;
        public TouchPhase phase;
        public Vector2 position;
    }

    public class InputManager : ManagerBase<InputManager>
    {
        public int GetTouches (ref InputTouch[] touches)
        {
#if UNITY_EDITOR || UNITY_FACEBOOK || UNITY_STANDALONE
            touches[0].position = Input.mousePosition;
            touches[0].pointerId = 0;

            if (Input.GetMouseButtonDown (0))
            {
                touches[0].phase = TouchPhase.Began;
                return 1;
            }

            if (Input.GetMouseButtonUp (0))
            {
                touches[0].phase = TouchPhase.Ended;
                return 1;
            }

            if (!Input.GetMouseButton (0)) return 0;
            touches[0].phase = TouchPhase.Stationary;
            return 1;
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            var count = Mathf.Min (touches.Length, Input.touchCount);
            for (var i = 0; i < count; i++)
            {
                var touch = Input.GetTouch (i);
                touches[i].phase = touch.phase;
                touches[i].position = touch.position;
                touches[i].pointerId = touch.fingerId;
            }

            return count;
#endif
        }


        /// <summary>
        /// Down 이벤트 유무
        /// </summary>
        public bool GetDown ()
        {
            var isDown = false;

#if UNITY_EDITOR || UNITY_FACEBOOK || UNITY_STANDALONE
            isDown = Input.GetMouseButtonDown (0);
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch (0);
                isDown = touch.phase == TouchPhase.Began;
            }
#endif
            return isDown;
        }


        /// <summary>
        /// Up 이벤트 유무
        /// </summary>
        /// <returns></returns>
        public bool GetUp ()
        {
            var isUp = false;

#if UNITY_EDITOR || UNITY_FACEBOOK || UNITY_STANDALONE
            isUp = Input.GetMouseButtonUp (0);
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch (0);
                isUp = touch.phase == TouchPhase.Ended;
            }
#endif
            return isUp;
        }


        /// <summary>
        /// Press 이벤트 유무
        /// </summary>
        public bool GetPress ()
        {
            bool isPress;

#if UNITY_EDITOR || UNITY_FACEBOOK || UNITY_STANDALONE
            isPress = Input.GetMouseButton (0);
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            isPress = Input.touchCount > 0;
#endif
            return isPress;
        }


        /// <summary>
        /// 위치
        /// </summary>
        public Vector2 GetPosition ()
        {
            var position = Vector2.zero;
#if UNITY_EDITOR || UNITY_FACEBOOK || UNITY_STANDALONE
            position = Input.mousePosition;
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch (0);
                position = touch.position;
            }
#endif
            return position;
        }


        /// <summary>
        /// 위치
        /// </summary>
        public int GetPositions (Vector2[] positions)
        {
            int count;
#if UNITY_EDITOR || UNITY_FACEBOOK || UNITY_STANDALONE
            positions[0] = Input.mousePosition;
            count = 1;
#elif (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            count = Input.touchCount;
            for (var i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch (i);
                positions[i] = touch.position;
            }
#else
            positions[0] = Vector2.zero;
            count = 1;
#endif
            return count;
        }


        public bool OnRaycastHit (GameObject gameObject)
        {
            var raycastResult = new List<RaycastResult> ();
            var pointerEvent = new PointerEventData (EventSystem.current)
            {
                position = GetPosition ()
            };

            EventSystem.current.RaycastAll (pointerEvent, raycastResult);

            return raycastResult.Select (item => item.gameObject).Any (obj => obj.Equals (gameObject));
        }


        public bool OnRaycastHit<T> () where T : Component
        {
            var raycastResult = new List<RaycastResult> ();
            var pointerEvent = new PointerEventData (EventSystem.current)
            {
                position = GetPosition ()
            };

            EventSystem.current.RaycastAll (pointerEvent, raycastResult);

            return raycastResult.Select (item => item.gameObject.GetComponent<T> ()).Any (getComp => getComp != null);
        }


        public bool OnRaycastHit<T> (out GameObject gameObject) where T : Component
        {
            var raycastResult = new List<RaycastResult> ();
            var pointerEvent = new PointerEventData (EventSystem.current)
            {
                position = GetPosition ()
            };

            EventSystem.current.RaycastAll (pointerEvent, raycastResult);
            foreach (var item in from item in raycastResult
                let getComp = item.gameObject.GetComponent<T> ()
                where getComp != null
                select item)
            {
                gameObject = item.gameObject;
                return true;
            }

            gameObject = null;
            return false;
        }
    }
}