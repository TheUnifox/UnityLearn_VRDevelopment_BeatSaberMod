// Decompiled with JetBrains decompiler
// Type: BloomPrePassRenderDataSO
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using UnityEngine;

public class BloomPrePassRenderDataSO : PersistentScriptableObject
{
  public readonly BloomPrePassRenderDataSO.Data data = new BloomPrePassRenderDataSO.Data();

  public class Data
  {
    [NonSerialized]
    public RenderTexture bloomPrePassRenderTexture;
    [NonSerialized]
    public Vector2 textureToScreenRatio;
    [NonSerialized]
    public Matrix4x4 viewMatrix;
    [NonSerialized]
    public Matrix4x4 projectionMatrix;
    [NonSerialized]
    public float stereoCameraEyeOffset;
    [NonSerialized]
    public ToneMapping toneMapping;
  }
}
