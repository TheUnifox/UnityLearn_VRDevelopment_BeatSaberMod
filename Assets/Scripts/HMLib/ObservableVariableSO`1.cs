// Decompiled with JetBrains decompiler
// Type: ObservableVariableSO`1
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

public class ObservableVariableSO<T> : PersistentScriptableObject, IValue<T>, IObservableChange
{
  protected T _value;

  public event System.Action didChangeEvent;

  public T value
  {
    set
    {
      if ((object) this._value != null && this._value.Equals((object) value))
        return;
      this._value = value;
      System.Action didChangeEvent = this.didChangeEvent;
      if (didChangeEvent == null)
        return;
      didChangeEvent();
    }
    get => this._value;
  }

  public static implicit operator T(ObservableVariableSO<T> obj) => obj.value;
}
