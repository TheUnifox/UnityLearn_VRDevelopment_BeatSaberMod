// Decompiled with JetBrains decompiler
// Type: DirectionalLightWithIds
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class DirectionalLightWithIds : RuntimeLightWithIds
{
  [SerializeField]
  protected DirectionalLight _directionalLight;
  [SerializeField]
  protected bool _setIntensityOnly;
  [SerializeField]
  [DrawIf("_setIntensityOnly", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected Color _defaultColor = Color.black;

  protected override void ColorWasSet(Color color)
  {
    if (this._setIntensityOnly)
      color = this._defaultColor.ColorWithValue(color.a);
    this._directionalLight.color = color;
  }
}
