// Decompiled with JetBrains decompiler
// Type: DataItemConvertor`4
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public abstract class DataItemConvertor<TBase, TIn, TOut, TParam> : DataItemConvertor<TBase, TParam> where TOut : TBase
{
  public override System.Type inputDataType => typeof (TIn);

  public override TBase Convert(object item, TParam param) => (TBase) this.Convert((TIn) item, param);

  protected abstract TOut Convert(TIn item, TParam param);
}
