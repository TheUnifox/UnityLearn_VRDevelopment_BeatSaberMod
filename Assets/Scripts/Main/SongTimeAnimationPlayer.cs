// Decompiled with JetBrains decompiler
// Type: SongTimeAnimationPlayer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SongTimeAnimationPlayer : MonoBehaviour
{
  [SerializeField]
  protected AnimationClip _animationClip;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;

  public virtual void Update() => this._animationClip.SampleAnimation(this.gameObject, this._audioTimeSyncController.songTime);
}
