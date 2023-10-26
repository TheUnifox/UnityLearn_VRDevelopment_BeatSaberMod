// Decompiled with JetBrains decompiler
// Type: HMUI.ScreenModeData
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;

namespace HMUI
{
  [Serializable]
  public class ScreenModeData
  {
    public Vector3 position;
    public Vector3 rotation;
    public float scale = 1f;
    public float radius = 150f;
    public bool offsetHeightByHeadPos;
    [DrawIf("offsetHeightByHeadPos", true, DrawIfAttribute.DisablingType.DontDraw)]
    public float yOffsetRelativeToHead;
    [DrawIf("offsetHeightByHeadPos", true, DrawIfAttribute.DisablingType.DontDraw)]
    public float minYPos;

    public ScreenModeData(
      Vector3 position,
      Vector3 rotation,
      float scale,
      float radius,
      bool offsetHeightByHeadPos,
      float yOffsetRelativeToHead,
      float minYPos)
    {
      this.position = position;
      this.rotation = rotation;
      this.scale = scale;
      this.radius = radius;
      this.offsetHeightByHeadPos = offsetHeightByHeadPos;
      this.yOffsetRelativeToHead = yOffsetRelativeToHead;
      this.minYPos = minYPos;
    }
  }
}
