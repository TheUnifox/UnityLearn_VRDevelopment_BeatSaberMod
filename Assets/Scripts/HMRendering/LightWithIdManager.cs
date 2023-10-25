// Decompiled with JetBrains decompiler
// Type: LightWithIdManager
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightWithIdManager : MonoBehaviour
{
  public const int kMaxLightId = 500;
  protected readonly List<ILightWithId>[] _lights = new List<ILightWithId>[501];
  protected readonly Color?[] _colors = new Color?[501];
  protected readonly List<ILightWithId> _lightsToUnregister = new List<ILightWithId>(100);
  protected bool _didChangeSomeColorsThisFrame;

  public event Action didChangeSomeColorsThisFrameEvent;

  public virtual void LateUpdate()
  {
    foreach (ILightWithId lightWithId in this._lightsToUnregister)
      this._lights[lightWithId.lightId].Remove(lightWithId);
    this._lightsToUnregister.Clear();
    if (this._didChangeSomeColorsThisFrame)
    {
      Action colorsThisFrameEvent = this.didChangeSomeColorsThisFrameEvent;
      if (colorsThisFrameEvent != null)
        colorsThisFrameEvent();
    }
    this._didChangeSomeColorsThisFrame = false;
  }

  public virtual void RegisterLight(ILightWithId lightWithId)
  {
    if (lightWithId.isRegistered)
      return;
    int lightId = lightWithId.lightId;
    if (lightId == -1)
      return;
    if (this._lights[lightId] == null)
      this._lights[lightId] = new List<ILightWithId>(10);
    this._lights[lightId].Add(lightWithId);
    this._lightsToUnregister.Remove(lightWithId);
    if (this._colors[lightId].HasValue)
      lightWithId.ColorWasSet(this._colors[lightId].Value);
    else
      lightWithId.ColorWasSet(Color.clear);
    lightWithId.__SetIsRegistered();
  }

  public virtual void UnregisterLight(ILightWithId lightWithId)
  {
    if (!lightWithId.isRegistered)
      return;
    int lightId = lightWithId.lightId;
    if (lightId == -1 || this._lights[lightId] == null)
      return;
    this._lightsToUnregister.Add(lightWithId);
    lightWithId.__SetIsUnRegistered();
  }

  public virtual void SetColorForId(int lightId, Color color)
  {
    this._colors[lightId] = new Color?(color);
    this._didChangeSomeColorsThisFrame = true;
    List<ILightWithId> light = this._lights[lightId];
    if (light == null)
      return;
    for (int index = 0; index < light.Count; ++index)
    {
      ILightWithId lightWithId = light[index];
      if (lightWithId.isRegistered)
        lightWithId.ColorWasSet(color);
    }
  }

  public virtual Color GetColorForId(int lightId, bool initializeIfNull = false)
  {
    if (this._colors[lightId].HasValue)
      return this._colors[lightId].Value;
    if (initializeIfNull)
      this.SetColorForId(lightId, Color.clear);
    return Color.clear;
  }
}
