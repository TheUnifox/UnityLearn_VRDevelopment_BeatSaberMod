// Decompiled with JetBrains decompiler
// Type: MockAudioTimeSource
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class MockAudioTimeSource : MonoBehaviour, IAudioTimeSource
{
  [CompilerGenerated]
  protected float m_CsongTime;
  [CompilerGenerated]
  protected float m_ClastFrameDeltaSongTime;

  public float songTime
  {
    get => this.m_CsongTime;
    private set => this.m_CsongTime = value;
  }

  public float lastFrameDeltaSongTime
  {
    get => this.m_ClastFrameDeltaSongTime;
    private set => this.m_ClastFrameDeltaSongTime = value;
  }

  public float songEndTime => float.MaxValue;

  public float songLength => float.MaxValue;

  public bool isReady => true;

  public virtual void Update()
  {
    this.songTime += Time.deltaTime;
    this.lastFrameDeltaSongTime = Time.deltaTime;
  }
}
