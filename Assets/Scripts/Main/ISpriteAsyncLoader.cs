// Decompiled with JetBrains decompiler
// Type: ISpriteAsyncLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface ISpriteAsyncLoader
{
  Task<Sprite> LoadSpriteAsync(string path, CancellationToken cancellationToken);
}
