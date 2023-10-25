// Decompiled with JetBrains decompiler
// Type: NoteCutHapticEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Libraries.HM.HMLib.VR;
using UnityEngine;
using Zenject;

public class NoteCutHapticEffect : MonoBehaviour
{
  [SerializeField]
  protected HapticPresetSO _normalPreset;
  [SerializeField]
  protected HapticPresetSO _shortNormalPreset;
  [SerializeField]
  protected HapticPresetSO _shortWeakPreset;
  [Inject]
  protected readonly HapticFeedbackController _hapticFeedbackController;

  public virtual void HitNote(SaberType saberType, NoteCutHapticEffect.Type type)
  {
    HapticPresetSO hapticPreset;
    switch (type)
    {
      case NoteCutHapticEffect.Type.Normal:
        hapticPreset = this._normalPreset;
        break;
      case NoteCutHapticEffect.Type.ShortNormal:
        hapticPreset = this._shortNormalPreset;
        break;
      case NoteCutHapticEffect.Type.ShortWeak:
        hapticPreset = this._shortWeakPreset;
        break;
      default:
        hapticPreset = this._normalPreset;
        break;
    }
    this._hapticFeedbackController.PlayHapticFeedback(saberType.Node(), hapticPreset);
  }

  public enum Type
  {
    Normal,
    ShortNormal,
    ShortWeak,
  }
}
