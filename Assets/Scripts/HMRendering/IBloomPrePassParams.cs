// Decompiled with JetBrains decompiler
// Type: IBloomPrePassParams
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public interface IBloomPrePassParams
{
  TextureEffectSO textureEffect { get; }

  int textureWidth { get; }

  int textureHeight { get; }

  Vector2 fov { get; }

  float linesWidth { get; }

  ToneMapping toneMapping { get; }
}
