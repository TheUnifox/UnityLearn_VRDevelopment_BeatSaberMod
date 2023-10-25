// Decompiled with JetBrains decompiler
// Type: Tweening.TweeningManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tweening
{
  public abstract class TweeningManager : MonoBehaviour
  {
    private readonly List<Tween> _activeTweens = new List<Tween>();
    private readonly HashSet<Tween> _activeTweensSet = new HashSet<Tween>();
    private readonly Dictionary<object, HashSet<Tween>> _tweensByOwner = new Dictionary<object, HashSet<Tween>>();
    private readonly Dictionary<Tween, object> _ownerByTween = new Dictionary<Tween, object>();
    private readonly Queue<HashSet<Tween>> _reusableTweenHashSets = new Queue<HashSet<Tween>>();

    protected void Start()
    {
      if (this._activeTweens.Count != 0)
        return;
      this.enabled = false;
    }

    protected void LateUpdate()
    {
      float time = this.GetTime();
      for (int index = this._activeTweens.Count - 1; index >= 0; --index)
      {
        Tween activeTween = this._activeTweens[index];
        if (!activeTween.isKilled)
          activeTween.Update(time);
        if (activeTween.isKilled || activeTween.isComplete)
        {
          if (activeTween.isKilled)
          {
            Action onKilled = activeTween.onKilled;
            if (onKilled != null)
              onKilled();
          }
          else if (activeTween.isComplete)
          {
            activeTween.Sample(1f);
            Action onCompleted = activeTween.onCompleted;
            if (onCompleted != null)
              onCompleted();
          }
          this._activeTweens.RemoveAt(index);
          this._activeTweensSet.Remove(activeTween);
          this.RemoveTweenFromOwnerDictionary(activeTween);
        }
      }
      if (this._activeTweens.Count != 0)
        return;
      this.enabled = false;
    }

    protected abstract float GetTime();

    public Tween AddTween(Tween tween, object owner)
    {
      if (this.AddTweenToDataStructures(tween, owner))
        tween.Restart(this.GetTime());
      return tween;
    }

    public Tween RestartTween(Tween tween, object owner)
    {
      tween.Restart(this.GetTime());
      this.AddTweenToDataStructures(tween, owner);
      return tween;
    }

    public Tween ResumeTween(Tween tween, object owner)
    {
      tween.Resume();
      this.AddTweenToDataStructures(tween, owner);
      return tween;
    }

    public void KillAllTweens(object owner)
    {
      HashSet<Tween> tweenSet;
      if (!this._tweensByOwner.TryGetValue(owner, out tweenSet))
        return;
      foreach (Tween tween in tweenSet)
        tween.Kill();
    }

    private void RemoveTweenFromOwnerDictionary(Tween tween)
    {
      object key;
      if (!this._ownerByTween.TryGetValue(tween, out key))
      {
        Debug.LogError((object) "Missing owner for tween while removing tween");
      }
      else
      {
        HashSet<Tween> tweenSet;
        if (!this._tweensByOwner.TryGetValue(key, out tweenSet))
        {
          Debug.LogError((object) "Missing tweens for an owner while removing tween");
        }
        else
        {
          tweenSet.Remove(tween);
          if (tweenSet.Count == 0)
          {
            this._reusableTweenHashSets.Enqueue(tweenSet);
            this._tweensByOwner.Remove(key);
          }
          this._ownerByTween.Remove(tween);
        }
      }
    }

    private bool AddTweenToDataStructures(Tween tween, object owner)
    {
      if (this._activeTweensSet.Contains(tween))
        return false;
      this._activeTweensSet.Add(tween);
      this._activeTweens.Add(tween);
      this.enabled = true;
      this.AddTweenToOwnerDictionary(tween, owner);
      return true;
    }

    private void AddTweenToOwnerDictionary(Tween tween, object owner)
    {
      HashSet<Tween> tweenSet1;
      if (this._tweensByOwner.TryGetValue(owner, out tweenSet1))
      {
        tweenSet1.Add(tween);
      }
      else
      {
        HashSet<Tween> tweenSet2 = this._reusableTweenHashSets.Count <= 0 ? new HashSet<Tween>() : this._reusableTweenHashSets.Dequeue();
        tweenSet2.Add(tween);
        this._tweensByOwner[owner] = tweenSet2;
      }
      this._ownerByTween[tween] = owner;
    }
  }
}
