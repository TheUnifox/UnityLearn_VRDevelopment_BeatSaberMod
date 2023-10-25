// Decompiled with JetBrains decompiler
// Type: MainCamera
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class MainCamera : MonoBehaviour
{
  protected Camera _camera;
  protected Transform _transform;

  public new Camera camera => this._camera;

  public bool enableCamera
  {
    set => this.gameObject.SetActive(value);
  }

  public Vector3 position => this._transform.position;

  public Quaternion rotation => this._transform.rotation;

  public virtual void Awake()
  {
    this._transform = this.transform;
    this._camera = this.GetComponent<Camera>();
    LivStaticWrapper.Init(this);
  }

  public virtual void OnEnable() => LivStaticWrapper.Update(this);
}
