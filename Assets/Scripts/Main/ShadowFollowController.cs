// Decompiled with JetBrains decompiler
// Type: ShadowFollowController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ShadowFollowController : MonoBehaviour
{
  [SerializeField]
  protected Transform _shadowTransform;
  [SerializeField]
  [NullAllowed]
  protected Transform _targetTransform;
  [SerializeField]
  protected SpriteRenderer _shadowSpriteRenderer;
  [Space]
  [SerializeField]
  protected Vector2 _shadowHeightRange;
  [SerializeField]
  protected Vector2 _shadowSizeRange;
  [SerializeField]
  protected Vector2 _shadowAlphaRange;

  public virtual void SetTargetTransform(Transform target) => this._targetTransform = target;

    public virtual void Update()
    {
        Vector3 position = this._targetTransform.position;
        float t = Mathf.InverseLerp(this._shadowHeightRange.x, this._shadowHeightRange.y, position.y);
        float num = this._shadowSizeRange.y - Mathf.LerpUnclamped(this._shadowSizeRange.x, this._shadowSizeRange.y, t);
        this._shadowTransform.localScale = new Vector3(num, num, num);
        this._shadowTransform.position = new Vector3(position.x, 0f, position.z);
        Color color = this._shadowSpriteRenderer.color;
        color.a = this._shadowAlphaRange.y - Mathf.LerpUnclamped(this._shadowAlphaRange.x, this._shadowAlphaRange.y, t);
        this._shadowSpriteRenderer.color = color;
    }
}
