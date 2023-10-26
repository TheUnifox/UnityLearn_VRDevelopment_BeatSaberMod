// Decompiled with JetBrains decompiler
// Type: MainEffectContainerSO
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

public class MainEffectContainerSO : PersistentScriptableObject
{
  [SerializeField]
  private MainEffectSO _mainEffect;
  [SerializeField]
  private BoolSO _postProcessEnabled;

  public MainEffectSO mainEffect => this._mainEffect;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._postProcessEnabled.value = this._mainEffect.hasPostProcessEffect;
  }

  public void Init(MainEffectSO mainEffect)
  {
    this._mainEffect = mainEffect;
    this._postProcessEnabled.value = this._mainEffect.hasPostProcessEffect;
  }
}
