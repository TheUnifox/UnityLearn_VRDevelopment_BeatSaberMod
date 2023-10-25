// Decompiled with JetBrains decompiler
// Type: SliderHapticFeedbackInteractionEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Libraries.HM.HMLib.VR;
using UnityEngine;
using Zenject;

public class SliderHapticFeedbackInteractionEffect : SliderInteractionEffect
{
  [SerializeField]
  protected HapticPresetSO _hapticPreset;
  [Inject]
  protected readonly HapticFeedbackController _hapticFeedbackController;
  [Inject]
  protected readonly IGamePause _gamePause;
  protected const float kVibrationSaberInteractionParamThreshold = 0.2f;
  protected SaberType _saberType;

  protected override void Start()
  {
    base.Start();
    this.enabled = false;
    this._saberType = this.colorType.ToSaberType();
  }

  public virtual void Update()
  {
    if (this._gamePause.isPaused || (double) this.saberInteractionParam <= 0.20000000298023224)
      return;
    this.Vibrate();
  }

  protected override void StartEffect(float saberInteractionParam)
  {
    this.enabled = true;
    if ((double) saberInteractionParam <= 0.20000000298023224)
      return;
    this.Vibrate();
  }

  protected override void EndEffect() => this.enabled = false;

  public virtual void Vibrate() => this._hapticFeedbackController.PlayHapticFeedback(this._saberType.Node(), this._hapticPreset);
}
