// Decompiled with JetBrains decompiler
// Type: BeatSaberAPI.DataTransferObjects.ResultProof
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Runtime.CompilerServices;

namespace BeatSaberAPI.DataTransferObjects
{
  [Serializable]
  public class ResultProof
  {
    [CompilerGenerated]
    protected string m_Cproof;

    public string proof
    {
      get => this.m_Cproof;
      set => this.m_Cproof = value;
    }
  }
}
