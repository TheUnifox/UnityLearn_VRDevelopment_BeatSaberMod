// Decompiled with JetBrains decompiler
// Type: DisableSpatializerOnOldWindows
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class DisableSpatializerOnOldWindows : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;

  public virtual void Awake()
  {
    if (SystemInfo.operatingSystem.IndexOf("Windows 10", StringComparison.OrdinalIgnoreCase) >= 0)
      return;
    this._audioSource.spatialize = false;
  }
}
