// Decompiled with JetBrains decompiler
// Type: LightTransformGroup`3
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class LightTransformGroup<TX, TY, TZ> : LightGroupSubsystem
  where TX : MonoBehaviour
  where TY : MonoBehaviour
  where TZ : MonoBehaviour
{
  [Space]
  [SerializeField]
  private bool _mirrorX;
  [SerializeField]
  private bool _mirrorY;
  [SerializeField]
  private bool _mirrorZ;
  [SerializeField]
  private bool _disableAutomaticTransformGathering;
  [SerializeField]
  private List<Transform> _xTransforms;
  [SerializeField]
  private List<Transform> _yTransforms;
  [SerializeField]
  private List<Transform> _zTransforms;

  public bool mirrorX => this._mirrorX;

  public bool mirrorY => this._mirrorY;

  public bool mirrorZ => this._mirrorZ;

  public List<Transform> xTransforms => this._xTransforms;

  public List<Transform> yTransforms => this._yTransforms;

  public List<Transform> zTransforms => this._zTransforms;

  public int count
  {
    get
    {
      if (this._xTransforms.Count > 0)
        return this._xTransforms.Count;
      if (this._yTransforms.Count > 0)
        return this._yTransforms.Count;
      return this._zTransforms.Count <= 0 ? 0 : this._zTransforms.Count;
    }
  }
}
