// Decompiled with JetBrains decompiler
// Type: MultiplayerSpectatingSpotManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;

public class MultiplayerSpectatingSpotManager
{
  protected readonly List<IMultiplayerSpectatingSpot> _spectatingSpots = new List<IMultiplayerSpectatingSpot>();
  protected readonly Dictionary<IMultiplayerSpectatingSpot, int> _spotIndexBySpot = new Dictionary<IMultiplayerSpectatingSpot, int>();

  public IReadOnlyList<IMultiplayerSpectatingSpot> spectatingSpots => (IReadOnlyList<IMultiplayerSpectatingSpot>) this._spectatingSpots;

  public IMultiplayerSpectatingSpot defaultSpot
  {
    get
    {
      IMultiplayerSpectatingSpot defaultSpot = this._spectatingSpots.FirstOrDefault<IMultiplayerSpectatingSpot>((Func<IMultiplayerSpectatingSpot, bool>) (s => s.isMain));
      if (defaultSpot != null)
        return defaultSpot;
      return this._spectatingSpots.Count <= 0 ? (IMultiplayerSpectatingSpot) null : this._spectatingSpots[0];
    }
  }

  public virtual void RegisterSpectatingSpot(IMultiplayerSpectatingSpot spectatingSpot)
  {
    this._spectatingSpots.Add(spectatingSpot);
    this.UpdateIndexBySpotDictionary();
    spectatingSpot.hasBeenRemovedEvent += new System.Action<IMultiplayerSpectatingSpot>(this.SpotOnHasBeenRemoved);
  }

  public virtual IMultiplayerSpectatingSpot GetAdjacentSpot(
    IMultiplayerSpectatingSpot spectatingSpot,
    int offset)
  {
    return this._spectatingSpots.Count == 0 ? (IMultiplayerSpectatingSpot) null : this._spectatingSpots[(this.GetIndexBySpot(spectatingSpot) + this._spectatingSpots.Count + offset) % this._spectatingSpots.Count];
  }

  public virtual int GetIndexBySpot(IMultiplayerSpectatingSpot spectatingSpot)
  {
    int num;
    return spectatingSpot != null && this._spotIndexBySpot.TryGetValue(spectatingSpot, out num) ? num : 0;
  }

  public virtual void UpdateIndexBySpotDictionary()
  {
    for (int index = 0; index < this.spectatingSpots.Count; ++index)
      this._spotIndexBySpot[this.spectatingSpots[index]] = index;
  }

  public virtual void SpotOnHasBeenRemoved(IMultiplayerSpectatingSpot spectatingSpot)
  {
    this._spectatingSpots.Remove(spectatingSpot);
    this.UpdateIndexBySpotDictionary();
    spectatingSpot.hasBeenRemovedEvent -= new System.Action<IMultiplayerSpectatingSpot>(this.SpotOnHasBeenRemoved);
  }
}
