using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

// Token: 0x0200000C RID: 12
public class ClientCertificateValidator : ICertificateValidator
{
    // Token: 0x06000035 RID: 53 RVA: 0x0000290D File Offset: 0x00000B0D
    public void ValidateCertificateChain(DnsEndPoint endPoint, X509Certificate2 certificate, byte[][] certificateChain)
    {
        this.ValidateCertificateChainInternal(endPoint, certificate, certificateChain);
    }

    // Token: 0x06000036 RID: 54 RVA: 0x00002918 File Offset: 0x00000B18
    [Conditional("VALIDATE_CERTIFICATE")]
    private void ValidateCertificateChainInternal(DnsEndPoint endPoint, X509Certificate2 certificate, byte[][] certificateChain)
    {
        string nameInfo = certificate.GetNameInfo(X509NameType.DnsName, false);
        if (nameInfo == null)
        {
            throw new Exception("Certificate does not contain a url!");
        }
        if (nameInfo.StartsWith("*"))
        {
            if (!endPoint.hostName.EndsWith(nameInfo.Substring(1)))
            {
                throw new Exception("Certificate url " + nameInfo + " does not match our specified hostname " + endPoint.hostName);
            }
        }
        else if (endPoint.hostName != nameInfo)
        {
            throw new Exception("Certificate url " + nameInfo + " does not match our specified hostname " + endPoint.hostName);
        }
        X509CertificateUtility.ValidateCertificateChain(certificate, certificateChain);
    }
}
