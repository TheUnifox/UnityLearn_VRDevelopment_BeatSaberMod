using System;
using System.Collections;
using System.Collections.Generic;
using BGNet.Core;

// Token: 0x0200001B RID: 27
public class ExpiringDictionary<TKey, TValue> : IDisposable, IEnumerable<TValue>, IEnumerable where TValue : IDisposable
{
    // Token: 0x1700002B RID: 43
    // (get) Token: 0x060000D6 RID: 214 RVA: 0x00004E49 File Offset: 0x00003049
    public int Count
    {
        get
        {
            return this._entryLookup.Count;
        }
    }

    // Token: 0x060000D7 RID: 215 RVA: 0x00004E58 File Offset: 0x00003058
    public ExpiringDictionary(BGNet.Core.ITimeProvider timeProvider, long expirationLengthMs)
    {
        this._timeProvider = timeProvider;
        this._expirationLengthMs = expirationLengthMs;
    }

    // Token: 0x060000D8 RID: 216 RVA: 0x00004EAC File Offset: 0x000030AC
    private static int CompareEntries(ExpiringDictionary<TKey, TValue>.Entry a, ExpiringDictionary<TKey, TValue>.Entry b)
    {
        return a.expireTime.CompareTo(b.expireTime);
    }

    // Token: 0x060000D9 RID: 217 RVA: 0x00004EC0 File Offset: 0x000030C0
    private void RemoveExpiredEntries()
    {
        long timeMs = this._timeProvider.GetTimeMs();
        ExpiringDictionary<TKey, TValue>.Entry entry;
        while (this._expirationQueue.TryGetFirst(out entry) && entry.expireTime < timeMs)
        {
            this._expirationQueue.Remove(entry);
            this._entryLookup.Remove(entry.key);
            entry.Dispose();
            this.ReleaseEntry(entry);
        }
    }

    // Token: 0x060000DA RID: 218 RVA: 0x00004F1F File Offset: 0x0000311F
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.Enumerate();
    }

    // Token: 0x060000DB RID: 219 RVA: 0x00004F1F File Offset: 0x0000311F
    IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()

    {
        return this.Enumerate();
    }

    // Token: 0x060000DC RID: 220 RVA: 0x00004F27 File Offset: 0x00003127
    private IEnumerator<TValue> Enumerate()
    {
        this.RemoveExpiredEntries();
        foreach (ExpiringDictionary<TKey, TValue>.Entry entry in this._expirationQueue)
        {
            yield return entry.value;
        }
        yield break;
    }

    // Token: 0x060000DD RID: 221 RVA: 0x00004F36 File Offset: 0x00003136
    public bool ContainsKey(TKey key)
    {
        this.RemoveExpiredEntries();
        return this._entryLookup.ContainsKey(key);
    }

    // Token: 0x060000DE RID: 222 RVA: 0x00004F4C File Offset: 0x0000314C
    public bool TryGetValue(TKey key, out TValue value)
    {
        this.RemoveExpiredEntries();
        ExpiringDictionary<TKey, TValue>.Entry entry;
        if (!this._entryLookup.TryGetValue(key, out entry))
        {
            value = default(TValue);
            return false;
        }
        value = entry.value;
        return true;
    }

    // Token: 0x060000DF RID: 223 RVA: 0x00004F88 File Offset: 0x00003188
    public bool TryGetValueAndResetExpiration(TKey key, out TValue value)
    {
        this.RemoveExpiredEntries();
        ExpiringDictionary<TKey, TValue>.Entry entry;
        if (!this._entryLookup.TryGetValue(key, out entry))
        {
            value = default(TValue);
            return false;
        }
        entry.expireTime = this._timeProvider.GetTimeMs() + this._expirationLengthMs;
        this._expirationQueue.UpdateSortedPosition(entry);
        value = entry.value;
        return true;
    }

    // Token: 0x060000E0 RID: 224 RVA: 0x00004FE8 File Offset: 0x000031E8
    public bool Remove(TKey key)
    {
        ExpiringDictionary<TKey, TValue>.Entry item;
        if (!this._entryLookup.TryGetValue(key, out item))
        {
            return false;
        }
        this._entryLookup.Remove(key);
        this._expirationQueue.Remove(item);
        return true;
    }

    // Token: 0x060000E1 RID: 225 RVA: 0x00005024 File Offset: 0x00003224
    public void ResetExpiration(TKey key)
    {
        ExpiringDictionary<TKey, TValue>.Entry entry;
        if (!this._entryLookup.TryGetValue(key, out entry))
        {
            return;
        }
        entry.expireTime = this._timeProvider.GetTimeMs() + this._expirationLengthMs;
        this._expirationQueue.UpdateSortedPosition(entry);
    }

    // Token: 0x060000E2 RID: 226 RVA: 0x00005068 File Offset: 0x00003268
    public bool Add(TKey key, TValue value)
    {
        if (this.ContainsKey(key))
        {
            return false;
        }
        ExpiringDictionary<TKey, TValue>.Entry entry = this.GetEntry(key, value);
        this._entryLookup[key] = entry;
        this._expirationQueue.Add(entry);
        return true;
    }

    // Token: 0x060000E3 RID: 227 RVA: 0x000050A4 File Offset: 0x000032A4
    private TValue Get(TKey key)
    {
        this.RemoveExpiredEntries();
        ExpiringDictionary<TKey, TValue>.Entry entry;
        if (!this._entryLookup.TryGetValue(key, out entry))
        {
            return default(TValue);
        }
        return entry.value;
    }

    // Token: 0x060000E4 RID: 228 RVA: 0x000050D8 File Offset: 0x000032D8
    private void Set(TKey key, TValue value)
    {
        ExpiringDictionary<TKey, TValue>.Entry entry;
        if (!this._entryLookup.TryGetValue(key, out entry))
        {
            entry = this.GetEntry(key, value);
            this._entryLookup[key] = entry;
            this._expirationQueue.Add(entry);
            return;
        }
        entry.value = value;
        entry.expireTime = this._timeProvider.GetTimeMs() + this._expirationLengthMs;
        this._expirationQueue.UpdateSortedPosition(entry);
    }

    // Token: 0x1700002C RID: 44
    public TValue this[TKey key]
    {
        get
        {
            return this.Get(key);
        }
        set
        {
            this.Set(key, value);
        }
    }

    // Token: 0x060000E7 RID: 231 RVA: 0x00005156 File Offset: 0x00003356
    public void PollUpdate()
    {
        this.RemoveExpiredEntries();
    }

    // Token: 0x060000E8 RID: 232 RVA: 0x00005160 File Offset: 0x00003360
    public void Dispose()
    {
        this._entryLookup.Clear();
        foreach (ExpiringDictionary<TKey, TValue>.Entry entry in this._expirationQueue)
        {
            entry.Dispose();
        }
        this._expirationQueue.Clear();
        this._reusableEntries.Clear();
    }

    // Token: 0x060000E9 RID: 233 RVA: 0x000051CC File Offset: 0x000033CC
    private ExpiringDictionary<TKey, TValue>.Entry GetEntry(TKey key, TValue value)
    {
        ExpiringDictionary<TKey, TValue>.Entry entry;
        if (this._reusableEntries.Count > 0)
        {
            entry = this._reusableEntries[this._reusableEntries.Count - 1];
            this._reusableEntries.RemoveAt(this._reusableEntries.Count - 1);
        }
        else
        {
            entry = new ExpiringDictionary<TKey, TValue>.Entry();
        }
        entry.key = key;
        entry.value = value;
        entry.expireTime = this._timeProvider.GetTimeMs() + this._expirationLengthMs;
        return entry;
    }

    // Token: 0x060000EA RID: 234 RVA: 0x00005247 File Offset: 0x00003447
    private void ReleaseEntry(ExpiringDictionary<TKey, TValue>.Entry entry)
    {
        if (this._reusableEntries.Count < 1024)
        {
            this._reusableEntries.Add(entry);
        }
    }

    // Token: 0x04000098 RID: 152
    private const int kMaxReusableEntries = 1024;

    // Token: 0x04000099 RID: 153
    private readonly BGNet.Core.ITimeProvider _timeProvider;

    // Token: 0x0400009A RID: 154
    private readonly long _expirationLengthMs;

    // Token: 0x0400009B RID: 155
    private readonly OrderedSet<ExpiringDictionary<TKey, TValue>.Entry> _expirationQueue = new OrderedSet<ExpiringDictionary<TKey, TValue>.Entry>(new Comparison<ExpiringDictionary<TKey, TValue>.Entry>(ExpiringDictionary<TKey, TValue>.CompareEntries), OrderedSet<ExpiringDictionary<TKey, TValue>.Entry>.ProcessOrder.DontCare);

    // Token: 0x0400009C RID: 156
    private readonly Dictionary<TKey, ExpiringDictionary<TKey, TValue>.Entry> _entryLookup = new Dictionary<TKey, ExpiringDictionary<TKey, TValue>.Entry>();

    // Token: 0x0400009D RID: 157
    private readonly List<ExpiringDictionary<TKey, TValue>.Entry> _reusableEntries = new List<ExpiringDictionary<TKey, TValue>.Entry>(1024);

    // Token: 0x020000E0 RID: 224
    private class Entry : IDisposable
    {
        public TKey key;
        public TValue value;
        public long expireTime;

        public void Dispose()
        {
            ref TValue local1 = ref this.value;
            ref TValue obj = ref local1;
            ref TValue local2 = ref obj;
            if ((object)default(TValue) == null)
            {
                if ((object)obj != null)
                    local1 = ref local2;
                else
                    goto label_3;
            }
            local1.Dispose();
        label_3:
            this.key = default(TKey);
            this.value = default(TValue);
        }
    }
}
