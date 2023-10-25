// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalActivePlayerIntroAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.Playables;

public class MultiplayerLocalActivePlayerIntroAnimator : MonoBehaviour
{
  [SerializeField]
  protected PlayableDirector _introPlayableDirector;

  public virtual void SetBeforeIntroValues() => this._introPlayableDirector.Evaluate();

  public virtual void SetAfterIntroValues()
  {
    this._introPlayableDirector.time = this._introPlayableDirector.duration;
    this._introPlayableDirector.Evaluate();
  }
}
