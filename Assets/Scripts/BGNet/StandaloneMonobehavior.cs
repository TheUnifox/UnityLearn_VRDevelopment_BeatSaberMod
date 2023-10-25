using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class StandaloneMonobehavior : MonoBehaviour, IStandaloneMonobehavior
{
    // Token: 0x170000D7 RID: 215
    // (get) Token: 0x06000550 RID: 1360 RVA: 0x0000EAFC File Offset: 0x0000CCFC
    public float objectLifeTime
    {
        get
        {
            return Time.realtimeSinceStartup - this._startTime;
        }
    }

    // Token: 0x170000D8 RID: 216
    // (get) Token: 0x06000551 RID: 1361 RVA: 0x0000EB0A File Offset: 0x0000CD0A
    public float deltaTime
    {
        get
        {
            return Time.unscaledDeltaTime;
        }
    }

    // Token: 0x170000D9 RID: 217
    // (get) Token: 0x06000552 RID: 1362 RVA: 0x0000EB11 File Offset: 0x0000CD11
    public float lastFrameTime
    {
        get
        {
            return this.GetLastFrameTime();
        }
    }

    // Token: 0x170000DA RID: 218
    // (get) Token: 0x06000553 RID: 1363 RVA: 0x0000EB19 File Offset: 0x0000CD19
    public int frameCount
    {
        get
        {
            return Time.frameCount;
        }
    }

    // Token: 0x170000DB RID: 219
    // (get) Token: 0x06000554 RID: 1364 RVA: 0x0000EB20 File Offset: 0x0000CD20
    public bool isReady
    {
        get
        {
            return this._readyTcs.Task.IsCompleted;
        }
    }

    // Token: 0x06000555 RID: 1365 RVA: 0x0000EB32 File Offset: 0x0000CD32
    protected void Awake()
    {
        this._startTime = Time.realtimeSinceStartup;
    }

    // Token: 0x06000556 RID: 1366 RVA: 0x00002273 File Offset: 0x00000473
    protected virtual void Start()
    {
    }

    // Token: 0x06000557 RID: 1367 RVA: 0x00002273 File Offset: 0x00000473
    protected virtual void Update()
    {
    }

    // Token: 0x06000558 RID: 1368 RVA: 0x00002273 File Offset: 0x00000473
    protected virtual void OnDestroy()
    {
    }

    // Token: 0x06000559 RID: 1369 RVA: 0x00002273 File Offset: 0x00000473
    protected virtual void OnApplicationPause(bool pauseStatus)
    {
    }

    // Token: 0x0600055A RID: 1370 RVA: 0x0000EB40 File Offset: 0x0000CD40
    public async void Dispatch(Action action)
    {
        if (!this.isReady)
        {
            await this._readyTcs.Task;
        }
        action();
    }

    // Token: 0x0600055B RID: 1371 RVA: 0x0000EB84 File Offset: 0x0000CD84
    public async Task DispatchAsync(Func<Task> action)
    {
        if (!this.isReady)
        {
            await this._readyTcs.Task;
        }
        await action();
    }

    // Token: 0x0600055C RID: 1372 RVA: 0x0000EBD1 File Offset: 0x0000CDD1
    public static T Create<T>() where T : StandaloneMonobehavior
    {
        GameObject gameObject = new GameObject();
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        gameObject.name = "T";
        UnityEngine.Object.DontDestroyOnLoad(gameObject);
        T t = gameObject.AddComponent<T>();
        t.enabled = false;
        return t;
    }

    // Token: 0x0600055D RID: 1373 RVA: 0x0000EC04 File Offset: 0x0000CE04
    public Task RunAsync(IStandaloneThreadRunner runner, CancellationToken cancellationToken)
    {
        if (this._isRunning)
        {
            return Task.FromException(new Exception("{this} is already running"));
        }
        this._isRunning = true;
        base.enabled = true;
        TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
        base.StartCoroutine(this.RunAsyncCoroutine(taskCompletionSource, cancellationToken));
        return taskCompletionSource.Task;
    }

    // Token: 0x0600055E RID: 1374 RVA: 0x0000EC52 File Offset: 0x0000CE52
    public void Stop()
    {
        this._isRunning = false;
    }

    // Token: 0x0600055F RID: 1375 RVA: 0x0000EC5B File Offset: 0x0000CE5B
    private IEnumerator RunAsyncCoroutine(TaskCompletionSource<bool> tcs, CancellationToken cancellationToken)
    {
        yield return null;
        TaskCompletionSource<bool> readyTcs = this._readyTcs;
        if (readyTcs != null)
        {
            readyTcs.TrySetResult(true);
        }
        while (!cancellationToken.IsCancellationRequested && this._isRunning)
        {
            yield return null;
        }
        UnityEngine.Object.Destroy(base.gameObject);
        if (cancellationToken.IsCancellationRequested)
        {
            tcs.SetCanceled();
        }
        else
        {
            tcs.TrySetResult(true);
        }
        yield break;
    }

    // Token: 0x06000560 RID: 1376 RVA: 0x0000EC78 File Offset: 0x0000CE78
    private float GetLastFrameTime()
    {
        if (this._lastFrameTimeCount != Time.frameCount)
        {
            FrameTimingManager.CaptureFrameTimings();
            FrameTimingManager.GetLatestTimings(1U, this._lastFrameTimings);
            this._lastFrameTimeCount = Time.frameCount;
        }
        return (float)this._lastFrameTimings[0].cpuFrameTime;
    }

    // Token: 0x0400021A RID: 538
    private readonly TaskCompletionSource<bool> _readyTcs = new TaskCompletionSource<bool>();

    // Token: 0x0400021B RID: 539
    private bool _isRunning;

    // Token: 0x0400021C RID: 540
    private float _startTime;

    // Token: 0x0400021D RID: 541
    private readonly FrameTiming[] _lastFrameTimings = new FrameTiming[1];

    // Token: 0x0400021E RID: 542
    private int _lastFrameTimeCount;
}
