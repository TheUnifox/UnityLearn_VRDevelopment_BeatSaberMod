// Decompiled with JetBrains decompiler
// Type: CutoutEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class CutoutEffect : MonoBehaviour
{
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [SerializeField]
  [NullAllowed]
  protected BoolSO _useRandomCutoutOffset;
  [SerializeField]
  protected Vector3 _cutoutOffset;
  protected Vector3 _randomNoiseTexOffset;
  protected float _cutout;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _cutoutPropertyID = Shader.PropertyToID("_Cutout");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _cutoutTexOffsetPropertyID = Shader.PropertyToID("_CutoutTexOffset");

  public bool useRandomCutoutOffset => (bool) (ObservableVariableSO<bool>) this._useRandomCutoutOffset;

  public virtual void Start()
  {
    this._randomNoiseTexOffset = Random.onUnitSphere * 10f;
    this.SetCutout(this._cutout);
  }

  public virtual void SetCutout(float cutout)
  {
    Vector3 cutoutOffset = !((Object) this._useRandomCutoutOffset != (Object) null) || !this._useRandomCutoutOffset.value ? this._cutoutOffset : this._randomNoiseTexOffset;
    this.SetCutout(cutout, cutoutOffset);
  }

  public virtual void SetCutout(float cutout, Vector3 cutoutOffset)
  {
    this._cutout = cutout;
    MaterialPropertyBlock materialPropertyBlock = this._materialPropertyBlockController.materialPropertyBlock;
    materialPropertyBlock.SetVector(CutoutEffect._cutoutTexOffsetPropertyID, (Vector4) cutoutOffset);
    materialPropertyBlock.SetFloat(CutoutEffect._cutoutPropertyID, cutout);
    this._materialPropertyBlockController.ApplyChanges();
  }
}
