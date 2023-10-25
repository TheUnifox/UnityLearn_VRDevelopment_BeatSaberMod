// Decompiled with JetBrains decompiler
// Type: AsyncHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Threading.Tasks;

public abstract class AsyncHelper
{
  public static T RunSync<T>(Func<Task<T>> asyncTask)
  {
    T result = default (T);
    Task.Run((Func<Task>) (async () => result = await asyncTask())).GetAwaiter().GetResult();
    return result;
  }

  public static void RunSync(Func<Task> asyncTask) => Task.Run((Func<Task>) (async () => await asyncTask())).GetAwaiter().GetResult();
}
