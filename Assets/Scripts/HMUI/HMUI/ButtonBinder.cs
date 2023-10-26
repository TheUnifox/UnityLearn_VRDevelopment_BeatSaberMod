// Decompiled with JetBrains decompiler
// Type: HMUI.ButtonBinder
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HMUI
{
  public class ButtonBinder
  {
    protected List<Tuple<Button, UnityAction>> _bindings;

    public ButtonBinder() => this.Init();

    public ButtonBinder(Button button, Action action)
    {
      this.Init();
      this.AddBinding(button, action);
    }

    public ButtonBinder(List<Tuple<Button, Action>> bindingData)
    {
      this.Init();
      this.AddBindings(bindingData);
    }

    public virtual void Init() => this._bindings = new List<Tuple<Button, UnityAction>>();

    public virtual void AddBindings(List<Tuple<Button, Action>> bindingData)
    {
      foreach (Tuple<Button, Action> tuple in bindingData)
        this.AddBinding(tuple.Item1, tuple.Item2);
    }

    public virtual void AddBinding(Button button, Action action)
    {
      UnityAction call = new UnityAction(action.Invoke);
      button.onClick.AddListener(call);
      this._bindings.Add<Button, UnityAction>(button, call);
    }

    public virtual void ClearBindings()
    {
      if (this._bindings == null)
        return;
      foreach (Tuple<Button, UnityAction> binding in this._bindings)
      {
        Button button = binding.Item1;
        if ((UnityEngine.Object) button != (UnityEngine.Object) null)
          button.onClick.RemoveListener(binding.Item2);
      }
      this._bindings.Clear();
    }
  }
}
