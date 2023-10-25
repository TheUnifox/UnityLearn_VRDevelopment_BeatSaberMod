// Decompiled with JetBrains decompiler
// Type: DataItemConvertor`2
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public abstract class DataItemConvertor<TOut, TParam>
{
  public abstract TOut Convert(object item, TParam param);

  public abstract System.Type inputDataType { get; }
}
