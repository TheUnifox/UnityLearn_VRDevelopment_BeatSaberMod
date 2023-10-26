// Decompiled with JetBrains decompiler
// Type: NoPostProcessMainEffectSO
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

public class NoPostProcessMainEffectSO : MainEffectSO
{
  [SerializeField]
  private Shader _fadeShader;
  [Space]
  [SerializeField]
  [Range(0.0f, 3f)]
  private float _baseColorBoost = 1f;
  [SerializeField]
  private float _baseColorBoostThreshold;
  private Material _fadeMaterial;

  public override bool hasPostProcessEffect => false;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._fadeMaterial = new Material(this._fadeShader);
    this._fadeMaterial.hideFlags = HideFlags.HideAndDontSave;
  }

  protected void OnDisable() => EssentialHelpers.SafeDestroy((Object) this._fadeMaterial);

  public override void PreRender() => MainEffectCore.SetGlobalShaderValues(this._baseColorBoost, this._baseColorBoostThreshold);

  public override void PostRender(float fade) => this.DrawFadeQuad(1f - fade);

  public void DrawFadeQuad(float alpha)
  {
    if ((double) alpha < 0.0099999997764825821)
      return;
    GL.PushMatrix();
    GL.LoadOrtho();
    this._fadeMaterial.color = new Color(0.0f, 0.0f, 0.0f, alpha);
    this._fadeMaterial.SetPass(0);
    GL.Begin(7);
    GL.Vertex3(0.0f, 0.0f, 0.0f);
    GL.Vertex3(1f, 0.0f, 0.0f);
    GL.Vertex3(1f, 1f, 0.0f);
    GL.Vertex3(0.0f, 1f, 0.0f);
    GL.End();
    GL.PopMatrix();
  }
}
