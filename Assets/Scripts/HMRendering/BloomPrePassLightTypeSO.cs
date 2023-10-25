// Decompiled with JetBrains decompiler
// Type: BloomPrePassLightTypeSO
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class BloomPrePassLightTypeSO : PersistentScriptableObject
{
  [SerializeField]
  protected int _renderingPriority;
  [SerializeField]
  protected Material _material;

  public int renderingPriority => this._renderingPriority;

  public Material material => this._material;
}
