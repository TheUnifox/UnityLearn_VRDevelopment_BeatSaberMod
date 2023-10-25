// Decompiled with JetBrains decompiler
// Type: KeyboardShortcutsManager
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections.Generic;
using UnityEngine;

public class KeyboardShortcutsManager : MonoBehaviour
{
  protected Dictionary<KeyboardShortcutsManager.KeyboardShortcut, List<System.Action>> _keyboardShortcutActions = new Dictionary<KeyboardShortcutsManager.KeyboardShortcut, List<System.Action>>();

  public virtual void Update()
  {
    if (!Input.anyKeyDown)
      return;
    foreach (KeyValuePair<KeyboardShortcutsManager.KeyboardShortcut, List<System.Action>> keyboardShortcutAction in this._keyboardShortcutActions)
    {
      KeyboardShortcutsManager.KeyboardShortcut key = keyboardShortcutAction.Key;
      List<System.Action> actionList = keyboardShortcutAction.Value;
      if (Input.GetKeyDown(key.mainKey) && (key.combinationKey1 == KeyCode.None || Input.GetKey(key.combinationKey1)) && (key.combinationKey2 == KeyCode.None || Input.GetKey(key.combinationKey2)))
      {
        foreach (System.Action action in actionList)
          action();
      }
    }
  }

  public virtual void RemoveKeyboardShortcuts(Dictionary<KeyCode, System.Action> shortcutActions)
  {
    foreach (KeyValuePair<KeyCode, System.Action> shortcutAction in shortcutActions)
      this.RemoveKeyboardShortcut(shortcutAction.Key, shortcutAction.Value);
  }

  public virtual void RemoveKeyboardShortcuts(
    Dictionary<KeyboardShortcutsManager.KeyboardShortcut, System.Action> shortcutActions)
  {
    foreach (KeyValuePair<KeyboardShortcutsManager.KeyboardShortcut, System.Action> shortcutAction in shortcutActions)
      this.RemoveKeyboardShortcut(shortcutAction.Key, shortcutAction.Value);
  }

  public virtual void RemoveKeyboardShortcut(
    KeyboardShortcutsManager.KeyboardShortcut keyboardShortcut,
    System.Action callback)
  {
    List<System.Action> actionList = (List<System.Action>) null;
    if (!this._keyboardShortcutActions.TryGetValue(keyboardShortcut, out actionList))
      return;
    actionList.Remove(callback);
  }

  public virtual void RemoveKeyboardShortcut(
    KeyCode mainKey,
    KeyCode combinationKey1,
    KeyCode combinationKey2,
    System.Action callback)
  {
    this.RemoveKeyboardShortcut(new KeyboardShortcutsManager.KeyboardShortcut(mainKey, combinationKey1, combinationKey2), callback);
  }

  public virtual void RemoveKeyboardShortcut(
    KeyCode mainKey,
    KeyCode combinationKey1,
    System.Action callback)
  {
    this.RemoveKeyboardShortcut(mainKey, combinationKey1, KeyCode.None, callback);
  }

  public virtual void RemoveKeyboardShortcut(KeyCode mainKey, System.Action callback) => this.RemoveKeyboardShortcut(mainKey, KeyCode.None, KeyCode.None, callback);

  public virtual void AddKeyboardShortcuts(Dictionary<KeyCode, System.Action> shortcutActions)
  {
    foreach (KeyValuePair<KeyCode, System.Action> shortcutAction in shortcutActions)
      this.AddKeyboardShortcut(shortcutAction.Key, shortcutAction.Value);
  }

  public virtual void AddKeyboardShortcuts(
    Dictionary<KeyboardShortcutsManager.KeyboardShortcut, System.Action> shortcutActions)
  {
    foreach (KeyValuePair<KeyboardShortcutsManager.KeyboardShortcut, System.Action> shortcutAction in shortcutActions)
      this.AddKeyboardShortcut(shortcutAction.Key, shortcutAction.Value);
  }

  public virtual void AddKeyboardShortcut(
    KeyboardShortcutsManager.KeyboardShortcut keyboardShortcut,
    System.Action callback)
  {
    List<System.Action> actionList = (List<System.Action>) null;
    if (!this._keyboardShortcutActions.TryGetValue(keyboardShortcut, out actionList))
    {
      actionList = new List<System.Action>();
      this._keyboardShortcutActions[keyboardShortcut] = actionList;
    }
    actionList.Add(callback);
  }

  public virtual void AddKeyboardShortcut(
    KeyCode mainKey,
    KeyCode combinationKey1,
    KeyCode combinationKey2,
    System.Action callback)
  {
    this.AddKeyboardShortcut(new KeyboardShortcutsManager.KeyboardShortcut(mainKey, combinationKey1, combinationKey2), callback);
  }

  public virtual void AddKeyboardShortcut(
    KeyCode mainKey,
    KeyCode combinationKey1,
    System.Action callback)
  {
    this.AddKeyboardShortcut(mainKey, combinationKey1, KeyCode.None, callback);
  }

  public virtual void AddKeyboardShortcut(KeyCode mainKey, System.Action callback) => this.AddKeyboardShortcut(mainKey, KeyCode.None, KeyCode.None, callback);

  public class KeyboardShortcut
  {
    public KeyCode mainKey;
    public KeyCode combinationKey1;
    public KeyCode combinationKey2;

    public KeyboardShortcut(KeyCode mainKey, KeyCode combinationKey1, KeyCode combinationKey2)
    {
      this.mainKey = mainKey;
      this.combinationKey1 = combinationKey1;
      this.combinationKey2 = combinationKey2;
    }

    public override int GetHashCode() => (int) (this.mainKey | this.combinationKey1 | this.combinationKey2);

    public override bool Equals(object obj) => obj != null && obj is KeyboardShortcutsManager.KeyboardShortcut keyboardShortcut && keyboardShortcut.mainKey == this.mainKey && keyboardShortcut.combinationKey1 == this.combinationKey1 && keyboardShortcut.combinationKey2 == this.combinationKey2;
  }
}
