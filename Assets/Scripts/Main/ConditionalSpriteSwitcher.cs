// Decompiled with JetBrains decompiler
// Type: ConditionalSpriteSwitcher
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ConditionalSpriteSwitcher : MonoBehaviour
{
  [Header("False")]
  [SerializeField]
  protected Sprite _sprite0;
  [SerializeField]
  protected Material _material0;
  [Header("True")]
  [SerializeField]
  protected Sprite _sprite1;
  [SerializeField]
  protected Material _material1;
  [Space]
  [SerializeField]
  protected BoolSO _value;
  [SerializeField]
  protected SpriteRenderer _spriteRenderer;

  public Sprite falseSprite
  {
    get => this._sprite0;
    set => this._sprite0 = value;
  }

  public Sprite trueSprite
  {
    get => this._sprite1;
    set => this._sprite1 = value;
  }

  public virtual void Awake() => this.Apply();

  public virtual void Apply()
  {
    if ((bool) (ObservableVariableSO<bool>) this._value)
    {
      this._spriteRenderer.sprite = this._sprite1;
      this._spriteRenderer.sharedMaterial = this._material1;
    }
    else
    {
      this._spriteRenderer.sprite = this._sprite0;
      this._spriteRenderer.sharedMaterial = this._material0;
    }
  }
}
