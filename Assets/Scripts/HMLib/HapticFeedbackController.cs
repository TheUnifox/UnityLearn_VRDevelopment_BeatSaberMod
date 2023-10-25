// Decompiled with JetBrains decompiler
// Type: HapticFeedbackController
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using Libraries.HM.HMLib.VR;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Zenject;

public class HapticFeedbackController : MonoBehaviour
{
  [SerializeField]
  protected BoolSO _controllersRumbleEnabled;
  [SerializeField]
  protected HapticPresetSO _continuousRumblePreset;
  [Inject]
  protected readonly IVRPlatformHelper _vrPlatformHelper;
  protected const float kContinuousRumbleFrameDuration = 0.0166666675f;
  protected readonly Dictionary<XRNode, Dictionary<object, HapticFeedbackController.RumbleData>> _rumblesByNode = new Dictionary<XRNode, Dictionary<object, HapticFeedbackController.RumbleData>>();

  public HapticPresetSO continuousRumblePreset => this._continuousRumblePreset;

  public virtual void Awake()
  {
    this._rumblesByNode[XRNode.LeftHand] = new Dictionary<object, HapticFeedbackController.RumbleData>();
    this._rumblesByNode[XRNode.RightHand] = new Dictionary<object, HapticFeedbackController.RumbleData>();
  }

  public virtual void PlayHapticFeedback(XRNode node, HapticPresetSO hapticPreset)
  {
    if (!this._controllersRumbleEnabled.value)
      return;
    HapticFeedbackController.RumbleData rumble = this.GetRumble(node, (object) hapticPreset);
    if (rumble == null || (Object) hapticPreset == (Object) null)
      return;
    rumble.active = true;
    rumble.continuous = hapticPreset._continuous;
    rumble.strength = hapticPreset._strength;
    rumble.endTime = Time.time + hapticPreset._duration;
    rumble.frequency = hapticPreset._frequency;
  }

  public virtual void LateUpdate() => this.UpdateRumbles();

  public virtual void UpdateRumbles()
  {
    foreach (KeyValuePair<XRNode, Dictionary<object, HapticFeedbackController.RumbleData>> keyValuePair in this._rumblesByNode)
    {
      XRNode key = keyValuePair.Key;
      bool flag = false;
      float strength = 0.0f;
      float duration = 0.0f;
      float frequency = 0.0f;
      foreach (HapticFeedbackController.RumbleData rumbleData in keyValuePair.Value.Values)
      {
        if (rumbleData.active && (double) rumbleData.strength >= (double) strength)
        {
          if (rumbleData.continuous)
          {
            rumbleData.active = false;
            duration = 0.0166666675f;
          }
          else
          {
            if ((double) rumbleData.endTime < (double) Time.time)
            {
              rumbleData.active = false;
              continue;
            }
            duration = rumbleData.endTime - Time.time;
          }
          flag = true;
          strength = rumbleData.strength;
          frequency = rumbleData.frequency;
        }
      }
      if (flag)
        this._vrPlatformHelper.TriggerHapticPulse(key, duration, strength, frequency);
      else
        this._vrPlatformHelper.StopHaptics(key);
    }
  }

  public virtual HapticFeedbackController.RumbleData GetRumble(XRNode node, object preset)
  {
    Dictionary<object, HapticFeedbackController.RumbleData> dictionary;
    if (!this._rumblesByNode.TryGetValue(node, out dictionary))
      return (HapticFeedbackController.RumbleData) null;
    HapticFeedbackController.RumbleData rumble;
    dictionary.TryGetValue(preset, out rumble);
    if (rumble == null)
      dictionary[preset] = rumble = new HapticFeedbackController.RumbleData();
    return rumble;
  }

  public class RumbleData
  {
    public bool active;
    public bool continuous;
    public float strength;
    public float endTime;
    public float frequency;
  }
}
