// Decompiled with JetBrains decompiler
// Type: CachedMediaAsyncLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CachedMediaAsyncLoader : MonoBehaviour, ISpriteAsyncLoader
{
  [SerializeField]
  protected int _maxNumberOfSpriteCachedElements = 100;
  protected AsyncCachedLoader<string, Sprite> _spriteAsyncCachedLoader;

  public virtual void ClearCache() => this._spriteAsyncCachedLoader?.ClearCache();

  public virtual async Task<Sprite> LoadSpriteAsync(
    string path,
    CancellationToken cancellationToken)
  {
    if (this._spriteAsyncCachedLoader == null)
      this._spriteAsyncCachedLoader = new AsyncCachedLoader<string, Sprite>(this._maxNumberOfSpriteCachedElements, new Func<string, CancellationToken, Task<Sprite>>(MediaAsyncLoader.LoadSpriteAsync));
    return await this._spriteAsyncCachedLoader.LoadAsync(path, cancellationToken);
  }
}
