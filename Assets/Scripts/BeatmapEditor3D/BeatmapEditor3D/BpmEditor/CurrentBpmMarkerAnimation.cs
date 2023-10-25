// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.CurrentBpmMarkerAnimation
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D.BpmEditor
{
  public class CurrentBpmMarkerAnimation : MonoBehaviour
  {
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _baseTickLength;
    [DoesNotRequireDomainReloadInit]
    private static readonly int _beatTriggerId = Animator.StringToHash("_BeatTrigger");

    public void Play(float tickLength)
    {
      this._animator.speed = 1f / Mathf.Min(this._baseTickLength, tickLength);
      this._animator.SetTrigger(CurrentBpmMarkerAnimation._beatTriggerId);
    }
  }
}
