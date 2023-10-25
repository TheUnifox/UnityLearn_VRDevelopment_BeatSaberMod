// Decompiled with JetBrains decompiler
// Type: DisableWhenMirrorIsEnabled
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class DisableWhenMirrorIsEnabled : MonoBehaviour
{
  [SerializeField]
  protected Mirror _mirror;

  public Mirror mirror
  {
    get => this._mirror;
    set => this._mirror = value;
  }

  public virtual void Start()
  {
    this._mirror.mirrorDidChangeEnabledStateEvent += new System.Action<bool>(this.HandleMirrorDidChangeEnabledState);
    this.HandleMirrorDidChangeEnabledState(this._mirror.isEnabled);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._mirror != (UnityEngine.Object) null))
      return;
    this._mirror.mirrorDidChangeEnabledStateEvent -= new System.Action<bool>(this.HandleMirrorDidChangeEnabledState);
  }

  public virtual void HandleMirrorDidChangeEnabledState(bool isEnabled) => this.gameObject.SetActive(!isEnabled);
}
