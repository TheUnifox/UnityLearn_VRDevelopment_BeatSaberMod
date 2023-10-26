// Decompiled with JetBrains decompiler
// Type: VRUIControls.VRLaserPointer
// Assembly: VRUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BA7334A-77F9-4425-988C-69CB2EE35CCF
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\VRUI.dll

using UnityEngine;

namespace VRUIControls
{
  public class VRLaserPointer : MonoBehaviour
  {
    [SerializeField]
    protected MeshRenderer _renderer;
    [DoesNotRequireDomainReloadInit]
    protected static readonly int _fadeStartNormalizedDistanceId = Shader.PropertyToID("_FadeStartNormalizedDistance");
    [DoesNotRequireDomainReloadInit]
    protected static MaterialPropertyBlock _materialPropertyBlock;

    public virtual void SetLocalPosition(Vector3 position) => this.transform.localPosition = position;

    public virtual void SetLocalScale(Vector3 scale) => this.transform.localScale = scale;

    public virtual void SetFadeDistance(float distance)
    {
      if (VRLaserPointer._materialPropertyBlock == null)
        VRLaserPointer._materialPropertyBlock = new MaterialPropertyBlock();
      VRLaserPointer._materialPropertyBlock.SetFloat(VRLaserPointer._fadeStartNormalizedDistanceId, distance);
      this._renderer.SetPropertyBlock(VRLaserPointer._materialPropertyBlock);
    }
  }
}
