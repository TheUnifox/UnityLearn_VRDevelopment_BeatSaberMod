// Decompiled with JetBrains decompiler
// Type: ReflectionProbeBakingOverride
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ReflectionProbeBakingOverride : MonoBehaviour
{
  [SerializeField]
  protected ReflectionProbeBakingOverride.ActiveStateHandling _stateHandling;
  [SerializeField]
  protected bool _setPosition;
  [SerializeField]
  [DrawIf("_setPosition", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected Vector3 _localPosition = Vector3.zero;
  [SerializeField]
  protected bool _setRotation;
  [SerializeField]
  [DrawIf("_setRotation", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected Vector3 _localRotation = Vector3.zero;
  [SerializeField]
  protected bool _setScale;
  [SerializeField]
  [DrawIf("_setScale", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected Vector3 _localScale = Vector3.one;

  public virtual void UpdateForProbeBaking()
  {
    if (this._stateHandling == ReflectionProbeBakingOverride.ActiveStateHandling.Enable)
      this.gameObject.SetActive(true);
    if (this._stateHandling == ReflectionProbeBakingOverride.ActiveStateHandling.Disable)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      Transform transform = this.gameObject.transform;
      if (this._setPosition)
        transform.localPosition = this._localPosition;
      if (this._setRotation)
        transform.localEulerAngles = this._localRotation;
      if (!this._setScale)
        return;
      transform.localScale = this._localScale;
    }
  }

  public enum ActiveStateHandling
  {
    LeaveAsIs,
    Enable,
    Disable,
  }
}
