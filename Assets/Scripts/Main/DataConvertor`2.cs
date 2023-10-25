// Decompiled with JetBrains decompiler
// Type: DataConvertor`2
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class DataConvertor<T, TParam>
{
  protected readonly Dictionary<System.Type, DataItemConvertor<T, TParam>> _convertors = new Dictionary<System.Type, DataItemConvertor<T, TParam>>();

  public virtual T ProcessItem(object item, TParam param)
  {
    DataItemConvertor<T, TParam> dataItemConvertor;
    return this._convertors.TryGetValue(item.GetType(), out dataItemConvertor) ? dataItemConvertor.Convert(item, param) : default (T);
  }

  public virtual void AddConvertor(DataItemConvertor<T, TParam> dataItemConvertor) => this._convertors[dataItemConvertor.inputDataType] = dataItemConvertor;
}
