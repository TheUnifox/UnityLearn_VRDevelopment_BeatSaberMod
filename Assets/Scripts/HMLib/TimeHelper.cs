// Decompiled with JetBrains decompiler
// Type: TimeHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class TimeHelper : MonoBehaviour
{
  [CompilerGenerated]
  protected static float m_time;
  [CompilerGenerated]
  protected static float m_deltaTime;
  [CompilerGenerated]
  protected static float m_fixedDeltaTime;
  [CompilerGenerated]
  protected static float m_interpolationFactor;
  protected float _accumulator;

  public static float time
  {
    get => TimeHelper.m_time;
    private set => TimeHelper.m_time = value;
  }

  public static float deltaTime
  {
    get => TimeHelper.m_deltaTime;
    private set => TimeHelper.m_deltaTime = value;
  }

  public static float fixedDeltaTime
  {
    get => TimeHelper.m_fixedDeltaTime;
    private set => TimeHelper.m_fixedDeltaTime = value;
  }

  public static float interpolationFactor
  {
    get => TimeHelper.m_interpolationFactor;
    private set => TimeHelper.m_interpolationFactor = value;
  }

  public virtual void Awake()
  {
    TimeHelper.fixedDeltaTime = Time.fixedDeltaTime;
    this._accumulator += TimeHelper.fixedDeltaTime;
  }

  public virtual void FixedUpdate()
  {
    TimeHelper.fixedDeltaTime = Time.fixedDeltaTime;
    this._accumulator -= TimeHelper.fixedDeltaTime;
  }

  public virtual void Update()
  {
    TimeHelper.deltaTime = Time.deltaTime;
    this._accumulator += TimeHelper.deltaTime;
    TimeHelper.time += TimeHelper.deltaTime;
    TimeHelper.interpolationFactor = this._accumulator / TimeHelper.fixedDeltaTime;
  }

  public static void __SetTime(float time) => TimeHelper.time = time;
}
