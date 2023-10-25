// Decompiled with JetBrains decompiler
// Type: ReflectionProbeBakingEffect
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteAlways]
[ImageEffectAllowedInSceneView]
public class ReflectionProbeBakingEffect : MonoBehaviour
{
  [SerializeField]
  protected Material _material;

  public virtual void OnRenderImage(RenderTexture src, RenderTexture dest) => Graphics.Blit((Texture) src, dest, this._material);
}
