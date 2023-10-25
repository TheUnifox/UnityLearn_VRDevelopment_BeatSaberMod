// Decompiled with JetBrains decompiler
// Type: SimpleShadowController
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class SimpleShadowController : MonoBehaviour
{
  [SerializeField]
  protected Transform _followTransform;
  [SerializeField]
  protected SpriteRenderer _spriteRenderer;
  [Space]
  [SerializeField]
  protected float _distanceScale = 1f;
  [SerializeField]
  protected float _scale;
  [Space]
  [SerializeField]
  protected float _alpha;
  [Space]
  [SerializeField]
  protected float _floorYPos;
  [SerializeField]
  protected bool _floorYPosLocal;
  protected Transform _transform;

  public virtual void Start() => this._transform = this.transform;

  public virtual void LateUpdate()
  {
    Vector3 position = this._followTransform.position;
    float num1 = this._floorYPosLocal ? this._floorYPos + this.transform.parent.position.y : this._floorYPos;
    float num2 = Mathf.Abs(position.y - num1) * this._distanceScale;
    float num3 = this._scale * (num2 + 1f);
    this._spriteRenderer.color = this._spriteRenderer.color.ColorWithAlpha(this._alpha / (num2 + 1f));
    position.y = num1;
    this._transform.position = position;
    this._transform.localScale = new Vector3(num3, num3, 1f);
  }
}
