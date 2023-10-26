// Decompiled with JetBrains decompiler
// Type: HMUI.Interactable
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;

namespace HMUI
{
  public class Interactable : MonoBehaviour
  {
    [SerializeField]
    protected bool _interactable = true;

    public event Action<Interactable, bool> interactableChangeEvent;

    public bool interactable
    {
      get => this._interactable;
      set
      {
        if (this._interactable == value)
          return;
        this._interactable = value;
        Action<Interactable, bool> interactableChangeEvent = this.interactableChangeEvent;
        if (interactableChangeEvent == null)
          return;
        interactableChangeEvent(this, this._interactable);
      }
    }
  }
}
