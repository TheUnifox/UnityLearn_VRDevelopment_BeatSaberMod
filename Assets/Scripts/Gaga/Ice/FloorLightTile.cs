using System;
using Tweening;
using UnityEngine;
using Zenject;

namespace Ice
{
    // Token: 0x02000004 RID: 4
    public class FloorLightTile : MonoBehaviour
    {
        // Token: 0x0600000A RID: 10 RVA: 0x00002268 File Offset: 0x00000468
        protected void Awake()
        {
            this._fadeInTween = new ColorTween(Color.clear, Color.clear, new Action<Color>(this.SetColor), 0f, EaseType.Linear, 0f);
            this._fadeOutTween = new ColorTween(Color.clear, Color.clear, new Action<Color>(this.SetColor), 0f, EaseType.Linear, 0f);
            this._fadeInTween.onCompleted = new Action(this.HandleFadeInTweenOnCompleted);
            this._fadeOutTween.onCompleted = new Action(this.HandleFadeOutTweenOnCompleted);
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000022FB File Offset: 0x000004FB
        protected void OnDestroy()
        {
            this._fadeInTween.onCompleted = null;
            this._fadeOutTween.onCompleted = null;
            if (this._songTimeTweeningManager != null)
            {
                this._songTimeTweeningManager.KillAllTweens(this);
            }
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002330 File Offset: 0x00000530
        public void HighlightWithColor(Color color, float fadeInDuration, float fadeOutDuration)
        {
            this._songTimeTweeningManager.KillAllTweens(this);
            this._fadeInTween.duration = fadeInDuration;
            this._fadeInTween.fromValue = this._colorSetter.color;
            this._fadeInTween.toValue = color;
            this._fadeOutTween.duration = fadeOutDuration;
            this._fadeOutTween.fromValue = color;
            this._fadeOutTween.toValue = new Color(0f, 0f, 0f, 0f);
            this._songTimeTweeningManager.RestartTween(this._fadeInTween, this);
        }

        // Token: 0x0600000D RID: 13 RVA: 0x000023C6 File Offset: 0x000005C6
        private void HandleFadeInTweenOnCompleted()
        {
            this._songTimeTweeningManager.RestartTween(this._fadeOutTween, this);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000023DB File Offset: 0x000005DB
        private void HandleFadeOutTweenOnCompleted()
        {
            Action<FloorLightTile> action = this.didFinish;
            if (action == null)
            {
                return;
            }
            action(this);
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000023EE File Offset: 0x000005EE
        private void SetColor(Color color)
        {
            this._colorSetter.SetColor(color);
            this._tubeBloomPrePassLight.color = color;
        }

        // Token: 0x0400000C RID: 12
        [SerializeField]
        private MaterialPropertyBlockColorSetter _colorSetter;

        // Token: 0x0400000D RID: 13
        [SerializeField]
        private TubeBloomPrePassLight _tubeBloomPrePassLight;

        // Token: 0x0400000E RID: 14
        [Inject]
        private readonly SongTimeTweeningManager _songTimeTweeningManager;

        // Token: 0x0400000F RID: 15
        [NonSerialized]
        public Action<FloorLightTile> didFinish;

        // Token: 0x04000010 RID: 16
        private ColorTween _fadeInTween;

        // Token: 0x04000011 RID: 17
        private ColorTween _fadeOutTween;

        // Token: 0x0200000A RID: 10
        public class Pool : MonoMemoryPool<FloorLightTile>
        {
        }
    }
}
