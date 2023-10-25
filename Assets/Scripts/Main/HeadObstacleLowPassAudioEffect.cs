// Decompiled with JetBrains decompiler
// Type: HeadObstacleLowPassAudioEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class HeadObstacleLowPassAudioEffect : MonoBehaviour
{
  [Inject]
  protected PlayerHeadAndObstacleInteraction _playerHeadAndObstacleInteraction;
  [Inject]
  protected MainAudioEffects _mainAudioEffects;
  protected bool _headWasInObstacle;

  public virtual void Update()
  {
    bool headIsInObstacle = this._playerHeadAndObstacleInteraction.playerHeadIsInObstacle;
    if (headIsInObstacle == this._headWasInObstacle)
      return;
    if (headIsInObstacle)
      this._mainAudioEffects.TriggerLowPass();
    else
      this._mainAudioEffects.ResumeNormalSound();
    this._headWasInObstacle = headIsInObstacle;
  }
}
