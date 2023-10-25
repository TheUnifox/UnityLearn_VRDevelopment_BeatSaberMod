// Decompiled with JetBrains decompiler
// Type: DataConvertor`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class DataConvertor<T>
{
  protected readonly Dictionary<System.Type, DataItemConvertor<T>> _convertors = new Dictionary<System.Type, DataItemConvertor<T>>();

  public virtual T ProcessItem(object item)
  {
    DataItemConvertor<T> dataItemConvertor;
    return this._convertors.TryGetValue(item.GetType(), out dataItemConvertor) ? dataItemConvertor.Convert(item) : default (T);
  }

  public virtual void AddConvertor(DataItemConvertor<T> dataItemConvertor) => this._convertors[dataItemConvertor.inputDataType] = dataItemConvertor;
}
