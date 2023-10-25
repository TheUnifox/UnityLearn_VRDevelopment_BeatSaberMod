using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BGNet.Logging;

// Token: 0x0200002D RID: 45
public class HealthCheckService : IHealthCheckService, IPollable, IDisposable
{
    // Token: 0x06000189 RID: 393 RVA: 0x00006AFB File Offset: 0x00004CFB
    public HealthCheckService(int port)
    {
        this._port = port;
        this._runThread = new Thread(new ThreadStart(this.Run));
        this._runThread.Start();
    }

    // Token: 0x0600018A RID: 394 RVA: 0x00006B38 File Offset: 0x00004D38
    public void Dispose()
    {
        this._disposed = true;
        this.PollUpdate();
        try
        {
            HttpListener listener = this._listener;
            if (listener != null)
            {
                listener.Close();
            }
        }
        catch (Exception)
        {
        }
    }

    // Token: 0x0600018B RID: 395 RVA: 0x00006B7C File Offset: 0x00004D7C
    public void PollUpdate()
    {
        try
        {
            this._manualResetEvent.Set();
        }
        catch (Exception)
        {
        }
    }

    // Token: 0x0600018C RID: 396 RVA: 0x00006BAC File Offset: 0x00004DAC
    private void Run()
    {
        while (!this._disposed)
        {
            try
            {
                string uriPrefix = new UriBuilder
                {
                    Scheme = "http",
                    Host = "+",
                    Port = this._port,
                    Path = "/"
                }.ToString();
                this._listener = new HttpListener();
                this._listener.Prefixes.Add(uriPrefix);
                this._listener.Start();
                while (!this._disposed)
                {
                    HttpListenerContext context = this._listener.GetContext();
                    bool flag = this._manualResetEvent.WaitOne(1000);
                    HttpListenerResponse response = context.Response;
                    response.StatusCode = (flag ? 200 : 500);
                    response.OutputStream.Close();
                }
            }
            catch (SocketException)
            {
            }
            catch (ThreadAbortException)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                this._listener = null;
            }
            catch (Exception exception)
            {
                Debug.LogException(exception, "Unexpected exception in health check");
                throw;
            }
            finally
            {
                HttpListener listener = this._listener;
                if (listener != null)
                {
                    listener.Stop();
                }
            }
        }
    }

    // Token: 0x040000F9 RID: 249
    private const int kTimeoutLengthMs = 1000;

    // Token: 0x040000FA RID: 250
    private readonly int _port;

    // Token: 0x040000FB RID: 251
    private readonly Thread _runThread;

    // Token: 0x040000FC RID: 252
    private volatile bool _disposed;

    // Token: 0x040000FD RID: 253
    private ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

    // Token: 0x040000FE RID: 254
    private HttpListener _listener;
}
