using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

// Token: 0x02000032 RID: 50
public interface ICertificateValidator
{
    // Token: 0x06000194 RID: 404
    void ValidateCertificateChain(DnsEndPoint endPoint, X509Certificate2 certificate, byte[][] certificateChain);
}
