// Decompiled with JetBrains decompiler
// Type: EnvironmentColorManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class EnvironmentColorManager : MonoBehaviour
{
  [SerializeField]
  protected ColorSchemeSO _defaultColorScheme;
  [Space]
  [SerializeField]
  protected SimpleColorSO _environmentColor0;
  [SerializeField]
  protected SimpleColorSO _environmentColor1;
  [SerializeField]
  protected SimpleColorSO _environmentColor0Boost;
  [SerializeField]
  protected SimpleColorSO _environmentColor1Boost;
  [InjectOptional]
  protected ColorScheme _colorScheme;

  public Color environmentColor0 => this._colorScheme.environmentColor0;

  public Color environmentColor1 => this._colorScheme.environmentColor1;

  public Color environmentColor0Boost => !this._colorScheme.supportsEnvironmentColorBoost ? this._colorScheme.environmentColor0 : this._colorScheme.environmentColor0Boost;

  public Color environmentColor1Boost => !this._colorScheme.supportsEnvironmentColorBoost ? this._colorScheme.environmentColor1 : this._colorScheme.environmentColor1Boost;

  public virtual void Awake()
  {
    if (this._colorScheme != null)
      return;
    this._colorScheme = this._defaultColorScheme.colorScheme;
  }

  public virtual void Start()
  {
    this._environmentColor0.SetColor(this._colorScheme.environmentColor0);
    this._environmentColor1.SetColor(this._colorScheme.environmentColor1);
    if (this._colorScheme.supportsEnvironmentColorBoost)
    {
      this._environmentColor0Boost.SetColor(this._colorScheme.environmentColor0Boost);
      this._environmentColor1Boost.SetColor(this._colorScheme.environmentColor1Boost);
    }
    else
    {
      this._environmentColor0Boost.SetColor(this._colorScheme.environmentColor0);
      this._environmentColor1Boost.SetColor(this._colorScheme.environmentColor1);
    }
  }
}
