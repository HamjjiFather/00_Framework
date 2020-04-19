using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KKSFramework.Animation
{
    /// <summary>
    /// 스프라이트 애니메이션 방향 타입.
    /// </summary>
    public enum StatusDirection
    {
        Front,
        Side,
        Back,
        Default
    }

    /// <summary>
    /// 스프라이트 애니메이션 동작 타입.
    /// </summary>
    public enum StatusBehaviour
    {
        Idle,
        Walk,
        Attack,
        FX,
        Default
    }

    /// <summary>
    /// 스프라이트 애니메이션 데이터 클래스.
    /// </summary>
    [Serializable]
    public class SpriteAnimationData
    {
        /// <summary>
        /// 반복 여부.
        /// </summary>
        public bool is_looping;

        /// <summary>
        /// 반복 후 스프라이트 종료 여부.
        /// </summary>
        public bool is_off;

        /// <summary>
        /// 스프라이트 리스트.
        /// </summary>
        public List<Sprite> list_spr_animation = new List<Sprite>();

        /// <summary>
        /// 애니메이션 속도 보정치.
        /// </summary>
        public float speed_rate;

        /// <summary>
        /// 이 데이터의 스프라이트 길이.
        /// </summary>
        /// <returns></returns>
        public int ReturnAnimationLength()
        {
            return list_spr_animation.Count;
        }
    }

    /// <summary>
    /// 애니메이션 관리 클래스.
    /// </summary>
    public class SpriteAnimationComponent : MonoBehaviour
    {
        /// <summary>
        /// 애니메이션 지속시간.
        /// </summary>
        private readonly float animation_duration = 1f;

        /// <summary>
        /// 현재 애니메이션 카운트.
        /// </summary>
        private int ani_count;

        /// <summary>
        /// 현재 실행중인 애니메이션 키.
        /// </summary>
        private string animation_key = string.Empty;

        /// <summary>
        /// 애니메이션 동작 타입.
        /// </summary>
        private StatusBehaviour e_StatusBehaviour = StatusBehaviour.Default;

        /// <summary>
        /// 애니메이션 방향 타입.
        /// </summary>
        private StatusDirection e_StatusDirection = StatusDirection.Default;

        /// <summary>
        /// 이벤트 프레임.
        /// </summary>
        private int event_frame = -1;

        /// <summary>
        /// 스프라이트 렌더러.
        /// </summary>
        private SpriteRenderer spren_this;

        /// <summary>
        /// 스프라이트 종료 이벤트.
        /// </summary>
        public UnityAction ua_end_animation;

        /// <summary>
        /// 스프라이트 특정 프레임 이벤트.
        /// </summary>
        public UnityAction ua_frame_event;

        public SpriteRenderer Spren_this
        {
            get
            {
                if (spren_this == null) spren_this = GetComponent<SpriteRenderer>();
                return spren_this;
            }

            set => spren_this = value;
        }

        /// <summary>
        /// 스프라이트 애니메이션 데이터.
        /// </summary>
        public Dictionary<string, SpriteAnimationData> Dic_sprites_data { get; set; } =
            new Dictionary<string, SpriteAnimationData>();

        /// <summary>
        /// 현재 애니메이션 길이를 리턴.
        /// </summary>
        /// <returns></returns>
        public int ReturnAnimationLength()
        {
            return Dic_sprites_data[animation_key].ReturnAnimationLength();
        }

        /// <summary>
        /// 해당 키에 해당하는 애니메이션 데이터 존재 여부.
        /// </summary>
        /// <param name="p_key"></param>
        /// <returns></returns>
        public bool IsSetAnimation(string p_key)
        {
            return Dic_sprites_data.ContainsKey(p_key);
        }

        /// <summary>
        /// 이동 애니메이션 세팅.
        /// </summary>
        /// <param name="p_arr_spr"></param>
        public void SetSprites(StatusDirection p_e_StatusDirection, StatusBehaviour p_e_StatusBehaviour,
            SpriteAnimationData p_SpriteAnimationData)
        {
            var temp_key_name = string.Format(p_e_StatusDirection + p_e_StatusBehaviour.ToString());
            if (Dic_sprites_data.ContainsKey(temp_key_name) == false)
                Dic_sprites_data.Add(temp_key_name, p_SpriteAnimationData);
        }

        /// <summary>
        /// 스프라이트 즉시 변경.
        /// </summary>
        public void ChangeSpriteImmediately(StatusDirection p_e_StatusDirection = StatusDirection.Default,
            StatusBehaviour p_e_StatusBehaviour = StatusBehaviour.Default, int p_count = 0)
        {
            ani_count = 0;
            e_StatusDirection = p_e_StatusDirection;
            e_StatusBehaviour = p_e_StatusBehaviour;
            animation_key = string.Format("{0}{1}", e_StatusDirection, e_StatusBehaviour);
            Spren_this.sprite = Dic_sprites_data[animation_key].list_spr_animation[p_count];
        }

        /// <summary>
        /// 스프라이트 데이터 초기화.
        /// </summary>
        public void ClearAllSprite()
        {
            Dic_sprites_data.Clear();
        }

        /// <summary>
        /// 이벤트 데이터 초기화.
        /// </summary>
        public void ClearAllEvent()
        {
            ua_end_animation = null;
            ua_frame_event = null;
        }

        /// <summary>
        /// 애니메이션 실행.
        /// </summary>
        /// <param name="p_is_attack"></param>
        public void Play(SpriteAnimationData p_SpriteAnimationData,
            StatusDirection p_e_StatusDirection = StatusDirection.Default,
            StatusBehaviour p_e_StatusBehaviour = StatusBehaviour.Default, bool p_is_force = false)
        {
            if (p_is_force == false &&
                animation_key.Equals(p_e_StatusDirection + p_e_StatusBehaviour.ToString()) == false || p_is_force)
            {
                animation_key = string.Format("{0}{1}", p_e_StatusDirection, p_e_StatusBehaviour);
                e_StatusDirection = p_e_StatusDirection;
                e_StatusBehaviour = p_e_StatusBehaviour;

                if (Dic_sprites_data.ContainsKey(animation_key))
                {
                    ani_count = 0;
                    CancelInvoke();
                    PlayAnimation();
                }
                else
                {
                    Debug.Log("Not Exist Animation : " + animation_key);
                }
            }
        }

        /// <summary>
        /// 프레임 이벤트 설정.
        /// </summary>
        /// <param name="p_event_frame"></param>
        /// <param name="p_ua_frame"></param>
        public void SetFrameEvent(int p_event_frame, UnityAction p_ua_frame)
        {
            event_frame = p_event_frame;
            ua_frame_event = p_ua_frame;
        }

        /// <summary>
        /// 종료 프레임 이벤트 설정.
        /// </summary>
        /// <param name="p_ua_end"></param>
        public void SetEndEvent(UnityAction p_ua_end)
        {
            ua_end_animation = p_ua_end;
        }

        /// <summary>
        /// 애니메이션 정지.
        /// </summary>
        public void Stop()
        {
            ani_count = 0;
            Spren_this.sprite = Dic_sprites_data[animation_key].list_spr_animation[ani_count];
            CancelInvoke();
        }

        /// <summary>
        /// 애니메이션 재실행.
        /// </summary>
        public void Resume()
        {
            ani_count = 0;
            CancelInvoke();
            PlayAnimation();
        }

        /// <summary>
        /// 애니메이션 실행.
        /// </summary>
        public void PlayAnimation()
        {
            Spren_this.sprite = Dic_sprites_data[animation_key].list_spr_animation[ani_count];
            Invoke("ChangeAnimation", animation_duration * Dic_sprites_data[animation_key].speed_rate);
        }

        /// <summary>
        /// 애니메이션 변경.
        /// </summary>
        private void ChangeAnimation()
        {
            ani_count++;

            if (event_frame.Equals(-1) == false)
                if (ani_count.Equals(event_frame))
                    if (ua_frame_event != null)
                    {
                        event_frame = -1;
                        ua_frame_event.Invoke();
                        ua_frame_event = null;
                    }

            if (ani_count == ReturnAnimationLength())
            {
                ani_count = 0;

                if (Dic_sprites_data[animation_key].is_looping)
                {
                    PlayAnimation();
                }
                else
                {
                    if (Dic_sprites_data[animation_key].is_off)
                    {
                        Spren_this.sprite = null;
                    }
                    else
                    {
                        if (ua_end_animation != null)
                        {
                            ua_end_animation.Invoke();
                            ua_end_animation = null;
                        }
                    }
                }
            }
            else
            {
                PlayAnimation();
            }
        }
    }
}