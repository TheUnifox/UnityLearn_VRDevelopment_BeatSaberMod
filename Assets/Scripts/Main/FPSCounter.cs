// Decompiled with JetBrains decompiler
// Type: FPSCounter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
  [CompilerGenerated]
  protected int m_CcurrentFPS;
  [CompilerGenerated]
  protected int m_ClowestFPS;
  [CompilerGenerated]
  protected int m_ChighestFPS;
  [CompilerGenerated]
  protected int m_CdroppedFrames;
  protected float _timeBuffer;
  protected int _frameCounter;
  protected float _minDeltaTime = float.MaxValue;

  public int currentFPS
  {
    get => this.m_CcurrentFPS;
    private set => this.m_CcurrentFPS = value;
  }

  public int lowestFPS
  {
    get => this.m_ClowestFPS;
    private set => this.m_ClowestFPS = value;
  }

  public int highestFPS
  {
    get => this.m_ChighestFPS;
    private set => this.m_ChighestFPS = value;
  }

  public int droppedFrames
  {
    get => this.m_CdroppedFrames;
    private set => this.m_CdroppedFrames = value;
  }

  public virtual void Awake()
  {
    this.lowestFPS = 999;
    this.highestFPS = 0;
  }

  public virtual void Update()
  {
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    this._timeBuffer += unscaledDeltaTime;
    ++this._frameCounter;
    if ((double) unscaledDeltaTime > 0.0066666668280959129)
      this._minDeltaTime = Mathf.Min(unscaledDeltaTime, this._minDeltaTime);
    if ((double) unscaledDeltaTime > (double) this._minDeltaTime * 1.5)
      ++this.droppedFrames;
    if ((double) this._timeBuffer < 1.0)
      return;
    this.currentFPS = this._frameCounter;
    this.lowestFPS = Mathf.Min(this.currentFPS, this.lowestFPS);
    this.highestFPS = Mathf.Max(this.currentFPS, this.highestFPS);
    this._timeBuffer -= Mathf.Floor(this._timeBuffer);
    this._frameCounter = 0;
  }
}
