// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorToggleGroupView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D
{
  public class BeatmapEditorToggleGroupView : MonoBehaviour
  {
    [SerializeField]
    private Toggle[] _uiStateToggles;
    private int _value;
    private List<UnityAction<bool>> _actions;

    public event Action<int> onValueChanged;

    public int value
    {
      get => this._value;
      set
      {
        this._value = value;
        this._uiStateToggles[value].isOn = true;
      }
    }

    protected void OnEnable()
    {
      this._actions = new List<UnityAction<bool>>(this._uiStateToggles.Length);
      for (int index = 0; index < this._uiStateToggles.Length; ++index)
      {
        int cachedIndex = index;
        Toggle uiStateToggle = this._uiStateToggles[index];
        uiStateToggle.isOn = false;
        this._actions.Add((UnityAction<bool>) (e => this.HandleToggleValueChanged(cachedIndex, e)));
        uiStateToggle.onValueChanged.AddListener(this._actions[index]);
      }
      this._uiStateToggles[this._value].isOn = true;
    }

    protected void OnDisable()
    {
      for (int index = 0; index < this._uiStateToggles.Length; ++index)
        this._uiStateToggles[index].onValueChanged.RemoveListener(this._actions[index]);
    }

    public void SetValueWithoutNotify(int value)
    {
      this._value = value;
      this._uiStateToggles[value].SetIsOnWithoutNotify(true);
    }

    public bool GetInteractable(int index) => this._uiStateToggles[index].interactable;

    public void SetInteractable(int index, bool value) => this._uiStateToggles[index].interactable = value;

    private void HandleToggleValueChanged(int toggleIndex, bool isOn)
    {
      if (!isOn)
        return;
      this._value = toggleIndex;
      Action<int> onValueChanged = this.onValueChanged;
      if (onValueChanged == null)
        return;
      onValueChanged(this._value);
    }
  }
}
