// Decompiled with JetBrains decompiler
// Type: Libraries.HM.HMLib.VR.HapticPresetSO
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

namespace Libraries.HM.HMLib.VR
{
  public class HapticPresetSO : ScriptableObject
  {
    [DrawIf("_continuous", false, DrawIfAttribute.DisablingType.DontDraw)]
    public float _duration = 0.05f;
    public float _strength = 1f;
    public float _frequency = 0.5f;
    public bool _continuous;

    public virtual void CopyFrom(HapticPresetSO hapticPreset)
    {
      this._duration = hapticPreset._duration;
      this._strength = hapticPreset._strength;
      this._frequency = hapticPreset._frequency;
      this._continuous = hapticPreset._continuous;
    }
  }
}
