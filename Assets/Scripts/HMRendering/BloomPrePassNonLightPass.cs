// Decompiled with JetBrains decompiler
// Type: BloomPrePassNonLightPass
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public abstract class BloomPrePassNonLightPass : MonoBehaviour
{
  [SerializeField]
  private BloomPrePassNonLightPass.ExecutionTimeType _executionTimeType;
  private static List<BloomPrePassNonLightPass> _bloomPrePassAfterBlurList = new List<BloomPrePassNonLightPass>();
  private static List<BloomPrePassNonLightPass> _bloomPrePassBeforeBlurList = new List<BloomPrePassNonLightPass>();
  private BloomPrePassNonLightPass.ExecutionTimeType _registeredExecutionTimeType;

  public BloomPrePassNonLightPass.ExecutionTimeType executionTimeType => this._executionTimeType;

  public static List<BloomPrePassNonLightPass> bloomPrePassAfterBlurList => BloomPrePassNonLightPass._bloomPrePassAfterBlurList;

  public static List<BloomPrePassNonLightPass> bloomPrePassBeforeBlurList => BloomPrePassNonLightPass._bloomPrePassBeforeBlurList;

  protected virtual void OnEnable() => this.Register();

  protected virtual void OnDisable() => this.Unregister();

  protected void Register()
  {
    if (this._registeredExecutionTimeType == this._executionTimeType)
      return;
    if (this._registeredExecutionTimeType != BloomPrePassNonLightPass.ExecutionTimeType.None)
      this.Unregister();
    switch (this._executionTimeType)
    {
      case BloomPrePassNonLightPass.ExecutionTimeType.BeforeBlur:
        BloomPrePassNonLightPass._bloomPrePassBeforeBlurList.Add(this);
        break;
      case BloomPrePassNonLightPass.ExecutionTimeType.AfterBlur:
        BloomPrePassNonLightPass._bloomPrePassAfterBlurList.Add(this);
        break;
    }
    this._registeredExecutionTimeType = this._executionTimeType;
  }

  protected void Unregister()
  {
    switch (this._registeredExecutionTimeType)
    {
      case BloomPrePassNonLightPass.ExecutionTimeType.BeforeBlur:
        BloomPrePassNonLightPass._bloomPrePassBeforeBlurList.Remove(this);
        break;
      case BloomPrePassNonLightPass.ExecutionTimeType.AfterBlur:
        BloomPrePassNonLightPass._bloomPrePassAfterBlurList.Remove(this);
        break;
    }
    this._registeredExecutionTimeType = BloomPrePassNonLightPass.ExecutionTimeType.None;
  }

  protected virtual void OnValidate()
  {
    if (this.isActiveAndEnabled)
      this.Register();
    else
      this.Unregister();
  }

  public abstract void Render(RenderTexture dest, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix);

  public enum ExecutionTimeType
  {
    None,
    BeforeBlur,
    AfterBlur,
  }
}
