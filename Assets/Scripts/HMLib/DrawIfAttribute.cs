// Decompiled with JetBrains decompiler
// Type: DrawIfAttribute
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class DrawIfAttribute : PropertyAttribute
{
  public readonly string propertyName;
  public readonly object value;
  public readonly object orValue;
  public readonly DrawIfAttribute.DisablingType disablingType;

  public DrawIfAttribute(
    string propertyName,
    object value,
    DrawIfAttribute.DisablingType disablingType = DrawIfAttribute.DisablingType.DontDraw)
  {
    this.propertyName = propertyName;
    this.value = value;
    this.disablingType = disablingType;
  }

  public DrawIfAttribute(
    string propertyName,
    object value,
    object orValue,
    DrawIfAttribute.DisablingType disablingType = DrawIfAttribute.DisablingType.DontDraw)
  {
    this.propertyName = propertyName;
    this.value = value;
    this.orValue = orValue;
    this.disablingType = disablingType;
  }

  public enum DisablingType
  {
    ReadOnly,
    DontDraw,
  }
}
