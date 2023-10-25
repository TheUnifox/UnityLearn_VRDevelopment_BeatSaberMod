// Decompiled with JetBrains decompiler
// Type: MarkableUIButton
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class MarkableUIButton : MonoBehaviour
{
  [SerializeField]
  protected Animator _animator;
  protected bool _marked;
  protected int _markedTriggerId;

  public bool marked
  {
    get => this._marked;
    set
    {
      this._marked = value;
      this._animator.SetBool(this._markedTriggerId, value);
    }
  }

  public virtual void Awake() => this._markedTriggerId = Animator.StringToHash("Marked");

  public virtual void ToggleMarked() => this.marked = !this.marked;
}
