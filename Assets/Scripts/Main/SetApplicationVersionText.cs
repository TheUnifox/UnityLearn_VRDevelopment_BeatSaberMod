// Decompiled with JetBrains decompiler
// Type: SetApplicationVersionText
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class SetApplicationVersionText : MonoBehaviour
{
  [SerializeField]
  protected TextMeshPro _versionText;

  public virtual void Start() => this._versionText.text = Application.version.Replace('_', '\n');
}
