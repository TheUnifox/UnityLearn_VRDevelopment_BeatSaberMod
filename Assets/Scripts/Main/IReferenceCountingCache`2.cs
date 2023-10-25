// Decompiled with JetBrains decompiler
// Type: IReferenceCountingCache`2
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public interface IReferenceCountingCache<in TKey, TValue>
{
  int Insert(TKey key, TValue item);

  int AddReference(TKey key);

  int RemoveReference(TKey key);

  int GetReferenceCount(TKey key);

  bool TryGet(TKey key, out TValue result);
}
