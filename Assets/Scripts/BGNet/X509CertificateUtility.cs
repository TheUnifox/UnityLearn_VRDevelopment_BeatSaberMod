using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.X509;

// Token: 0x02000095 RID: 149
public static class X509CertificateUtility
{
    // Token: 0x060005E9 RID: 1513 RVA: 0x0001038D File Offset: 0x0000E58D
    private static X509Certificate2[] GetRootCertificates()
    {
        if (X509CertificateUtility._rootCertificates == null)
        {
            X509CertificateUtility._rootCertificates = X509CertificateUtility.GetCertificateList(X509CertificateUtility.kBeatSaberDotComRootCertificate, X509CertificateUtility.kAwsRootCertificates).ToArray<X509Certificate2>();
        }
        return X509CertificateUtility._rootCertificates;
    }

    // Token: 0x060005EA RID: 1514 RVA: 0x000103B4 File Offset: 0x0000E5B4
    public static ICertificateEncryptionProvider GetCertificateEncryptionProvider(string privateKeyPem, string password = null)
    {
        return X509CertificateUtility.GetRSACertificateEncryptionProvider(privateKeyPem, string.IsNullOrEmpty(password) ? null : new X509CertificateUtility.PasswordFinder(password));
    }

    // Token: 0x060005EB RID: 1515 RVA: 0x000103D0 File Offset: 0x0000E5D0
    private static X509CertificateUtility.RSACertificateEncryptionProvider GetRSACertificateEncryptionProvider(string privateKeyPem, X509CertificateUtility.PasswordFinder passwordFinder)
    {
        object obj = new PemReader(new StringReader(privateKeyPem), passwordFinder).ReadObject();
        AsymmetricCipherKeyPair asymmetricCipherKeyPair;
        if ((asymmetricCipherKeyPair = (obj as AsymmetricCipherKeyPair)) != null)
        {
            obj = asymmetricCipherKeyPair.Private;
        }
        RsaPrivateCrtKeyParameters privateKey;
        if ((privateKey = (obj as RsaPrivateCrtKeyParameters)) != null)
        {
            return new X509CertificateUtility.RSACertificateEncryptionProvider(privateKey);
        }
        throw new Exception(string.Format("Expecting RSA private key but found object of type {0}", (obj != null) ? obj.GetType() : null));
    }

    // Token: 0x060005EC RID: 1516 RVA: 0x0001042C File Offset: 0x0000E62C
    public static IEnumerable<X509Certificate2> GetCertificateList(string certificatePem, string certificateChainPem = null)
    {
        X509CertificateParser x509CertificateParser = new X509CertificateParser();
        PemReader pemReader = new PemReader(new StringReader(certificatePem));
        yield return new X509Certificate2(x509CertificateParser.ReadCertificate(pemReader.ReadPemObject().Content).GetEncoded());
        if (string.IsNullOrEmpty(certificateChainPem))
        {
            yield break;
        }
        PemReader pemReader2 = new PemReader(new StringReader(certificateChainPem));
        ICollection collection = x509CertificateParser.ReadCertificates(pemReader2.ReadPemObject().Content);
        foreach (object obj in collection)
        {
            Org.BouncyCastle.X509.X509Certificate x509Certificate = (Org.BouncyCastle.X509.X509Certificate)obj;
            yield return new X509Certificate2(x509Certificate.GetEncoded());
        }
        yield break;
    }

    // Token: 0x060005ED RID: 1517 RVA: 0x00010443 File Offset: 0x0000E643
    public static void ValidateCertificateChain(X509Certificate2 certificate, byte[][] certificateChain)
    {
        X509CertificateUtility.ValidateCertificateChainUnity(certificate, certificateChain);
    }

    // Token: 0x060005EE RID: 1518 RVA: 0x0001044C File Offset: 0x0000E64C
    private static void ValidateCertificateChainUnity(X509Certificate2 certificate, byte[][] certificateChain)
    {
        X509Chain x509Chain = new X509Chain();
        List<X509Certificate2> list = new List<X509Certificate2>();
        try
        {
            foreach (X509Certificate2 certificate2 in X509CertificateUtility.GetRootCertificates())
            {
                x509Chain.ChainPolicy.ExtraStore.Add(certificate2);
            }
            for (int j = 1; j < certificateChain.Length; j++)
            {
                X509Certificate2 x509Certificate = new X509Certificate2(certificateChain[j]);
                list.Add(x509Certificate);
                x509Chain.ChainPolicy.ExtraStore.Add(x509Certificate);
            }
            x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            x509Chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
            x509Chain.ChainPolicy.VerificationTime = DateTime.UtcNow;
            x509Chain.ChainPolicy.UrlRetrievalTimeout = TimeSpan.FromSeconds(10.0);
            if (!x509Chain.Build(certificate))
            {
                throw new CertificateException("Certificate chain is not valid! Errors: " + string.Join(", ", from s in x509Chain.ChainStatus
                                                                                                              select s.Status.ToString()));
            }
            if (x509Chain.ChainElements.Count != list.Count + 2)
            {
                throw new CertificateException(string.Format("Unexpected number of elements in certificate chain. Expected {0} but found {1}", list.Count + 2, x509Chain.ChainElements.Count));
            }
            for (int k = 1; k < x509Chain.ChainElements.Count - 1; k++)
            {
                if (x509Chain.ChainElements[k].Certificate.Thumbprint != list[k - 1].Thumbprint)
                {
                    throw new CertificateException("Thumbprint mismatch in certificate chain!");
                }
            }
            string thumbprint = x509Chain.ChainElements[x509Chain.ChainElements.Count - 1].Certificate.Thumbprint;
            bool flag = false;
            X509Certificate2[] rootCertificates = X509CertificateUtility.GetRootCertificates();
            for (int i = 0; i < rootCertificates.Length; i++)
            {
                if (rootCertificates[i].Thumbprint == thumbprint)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new CertificateException("Last Certificate in Chain was not a Root Certificate!");
            }
        }
        finally
        {
            for (int l = 0; l < list.Count; l++)
            {
                list[l].Dispose();
            }
            x509Chain.Dispose();
        }
    }

    // Token: 0x060005EF RID: 1519 RVA: 0x000106A0 File Offset: 0x0000E8A0
    private static void ValidateCertificateChainDotNet(X509Certificate2 certificate, byte[][] certificateChain)
    {
        X509Chain x509Chain = new X509Chain();
        try
        {
            for (int i = 1; i < certificateChain.Length; i++)
            {
                x509Chain.ChainPolicy.ExtraStore.Add(new X509Certificate2(certificateChain[i]));
            }
            x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
            x509Chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
            x509Chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
            x509Chain.ChainPolicy.VerificationTime = DateTime.UtcNow;
            x509Chain.ChainPolicy.UrlRetrievalTimeout = TimeSpan.FromSeconds(10.0);
            if (!x509Chain.Build(certificate))
            {
                throw new CertificateException("Certificate chain is not valid! Errors: " + string.Join(", ", from s in x509Chain.ChainStatus
                                                                                                              select s.Status.ToString()));
            }
        }
        finally
        {
            for (int j = 0; j < x509Chain.ChainPolicy.ExtraStore.Count; j++)
            {
                x509Chain.ChainPolicy.ExtraStore[j].Dispose();
            }
            x509Chain.Dispose();
        }
    }

    // Token: 0x0400025A RID: 602
    [DoesNotRequireDomainReloadInit]
    private static readonly string kBeatSaberDotComRootCertificate = "-----BEGIN CERTIFICATE-----\r\nMIIDxTCCAq2gAwIBAgIQAqxcJmoLQJuPC3nyrkYldzANBgkqhkiG9w0BAQUFADBs\r\nMQswCQYDVQQGEwJVUzEVMBMGA1UEChMMRGlnaUNlcnQgSW5jMRkwFwYDVQQLExB3\r\nd3cuZGlnaWNlcnQuY29tMSswKQYDVQQDEyJEaWdpQ2VydCBIaWdoIEFzc3VyYW5j\r\nZSBFViBSb290IENBMB4XDTA2MTExMDAwMDAwMFoXDTMxMTExMDAwMDAwMFowbDEL\r\nMAkGA1UEBhMCVVMxFTATBgNVBAoTDERpZ2lDZXJ0IEluYzEZMBcGA1UECxMQd3d3\r\nLmRpZ2ljZXJ0LmNvbTErMCkGA1UEAxMiRGlnaUNlcnQgSGlnaCBBc3N1cmFuY2Ug\r\nRVYgUm9vdCBDQTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAMbM5XPm\r\n+9S75S0tMqbf5YE/yc0lSbZxKsPVlDRnogocsF9ppkCxxLeyj9CYpKlBWTrT3JTW\r\nPNt0OKRKzE0lgvdKpVMSOO7zSW1xkX5jtqumX8OkhPhPYlG++MXs2ziS4wblCJEM\r\nxChBVfvLWokVfnHoNb9Ncgk9vjo4UFt3MRuNs8ckRZqnrG0AFFoEt7oT61EKmEFB\r\nIk5lYYeBQVCmeVyJ3hlKV9Uu5l0cUyx+mM0aBhakaHPQNAQTXKFx01p8VdteZOE3\r\nhzBWBOURtCmAEvF5OYiiAhF8J2a3iLd48soKqDirCmTCv2ZdlYTBoSUeh10aUAsg\r\nEsxBu24LUTi4S8sCAwEAAaNjMGEwDgYDVR0PAQH/BAQDAgGGMA8GA1UdEwEB/wQF\r\nMAMBAf8wHQYDVR0OBBYEFLE+w2kD+L9HAdSYJhoIAu9jZCvDMB8GA1UdIwQYMBaA\r\nFLE+w2kD+L9HAdSYJhoIAu9jZCvDMA0GCSqGSIb3DQEBBQUAA4IBAQAcGgaX3Nec\r\nnzyIZgYIVyHbIUf4KmeqvxgydkAQV8GK83rZEWWONfqe/EW1ntlMMUu4kehDLI6z\r\neM7b41N5cdblIZQB2lWHmiRk9opmzN6cN82oNLFpmyPInngiK3BD41VHMWEZ71jF\r\nhS9OMPagMRYjyOfiZRYzy78aG6A9+MpeizGLYAiJLQwGXFK3xPkKmNEVX58Svnw2\r\nYzi9RKR/5CYrCsSXaQ3pjOLAEFe4yHYSkVXySGnYvCoCWw9E1CAx2/S6cCZdkGCe\r\nvEsXCS+0yx5DaMkHJ8HSXPfqIbloEpw8nL+e/IBcm2PN7EeqJSdnoDfzAIJ9VNep\r\n+OkuE6N36B9K\r\n-----END CERTIFICATE-----";

    // Token: 0x0400025B RID: 603
    [DoesNotRequireDomainReloadInit]
    private static readonly string kAwsRootCertificates = "-----BEGIN CERTIFICATE-----\r\nMIIDQTCCAimgAwIBAgITBmyfz5m/jAo54vB4ikPmljZbyjANBgkqhkiG9w0BAQsF\r\nADA5MQswCQYDVQQGEwJVUzEPMA0GA1UEChMGQW1hem9uMRkwFwYDVQQDExBBbWF6\r\nb24gUm9vdCBDQSAxMB4XDTE1MDUyNjAwMDAwMFoXDTM4MDExNzAwMDAwMFowOTEL\r\nMAkGA1UEBhMCVVMxDzANBgNVBAoTBkFtYXpvbjEZMBcGA1UEAxMQQW1hem9uIFJv\r\nb3QgQ0EgMTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBALJ4gHHKeNXj\r\nca9HgFB0fW7Y14h29Jlo91ghYPl0hAEvrAIthtOgQ3pOsqTQNroBvo3bSMgHFzZM\r\n9O6II8c+6zf1tRn4SWiw3te5djgdYZ6k/oI2peVKVuRF4fn9tBb6dNqcmzU5L/qw\r\nIFAGbHrQgLKm+a/sRxmPUDgH3KKHOVj4utWp+UhnMJbulHheb4mjUcAwhmahRWa6\r\nVOujw5H5SNz/0egwLX0tdHA114gk957EWW67c4cX8jJGKLhD+rcdqsq08p8kDi1L\r\n93FcXmn/6pUCyziKrlA4b9v7LWIbxcceVOF34GfID5yHI9Y/QCB/IIDEgEw+OyQm\r\njgSubJrIqg0CAwEAAaNCMEAwDwYDVR0TAQH/BAUwAwEB/zAOBgNVHQ8BAf8EBAMC\r\nAYYwHQYDVR0OBBYEFIQYzIU07LwMlJQuCFmcx7IQTgoIMA0GCSqGSIb3DQEBCwUA\r\nA4IBAQCY8jdaQZChGsV2USggNiMOruYou6r4lK5IpDB/G/wkjUu0yKGX9rbxenDI\r\nU5PMCCjjmCXPI6T53iHTfIUJrU6adTrCC2qJeHZERxhlbI1Bjjt/msv0tadQ1wUs\r\nN+gDS63pYaACbvXy8MWy7Vu33PqUXHeeE6V/Uq2V8viTO96LXFvKWlJbYK8U90vv\r\no/ufQJVtMVT8QtPHRh8jrdkPSHCa2XV4cdFyQzR1bldZwgJcJmApzyMZFo6IQ6XU\r\n5MsI+yMRQ+hDKXJioaldXgjUkK642M4UwtBV8ob2xJNDd2ZhwLnoQdeXeGADbkpy\r\nrqXRfboQnoZsG4q5WTP468SQvvG5\r\n-----END CERTIFICATE-----\r\n-----BEGIN CERTIFICATE-----\r\nMIID7zCCAtegAwIBAgIBADANBgkqhkiG9w0BAQsFADCBmDELMAkGA1UEBhMCVVMx\r\nEDAOBgNVBAgTB0FyaXpvbmExEzARBgNVBAcTClNjb3R0c2RhbGUxJTAjBgNVBAoT\r\nHFN0YXJmaWVsZCBUZWNobm9sb2dpZXMsIEluYy4xOzA5BgNVBAMTMlN0YXJmaWVs\r\nZCBTZXJ2aWNlcyBSb290IENlcnRpZmljYXRlIEF1dGhvcml0eSAtIEcyMB4XDTA5\r\nMDkwMTAwMDAwMFoXDTM3MTIzMTIzNTk1OVowgZgxCzAJBgNVBAYTAlVTMRAwDgYD\r\nVQQIEwdBcml6b25hMRMwEQYDVQQHEwpTY290dHNkYWxlMSUwIwYDVQQKExxTdGFy\r\nZmllbGQgVGVjaG5vbG9naWVzLCBJbmMuMTswOQYDVQQDEzJTdGFyZmllbGQgU2Vy\r\ndmljZXMgUm9vdCBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkgLSBHMjCCASIwDQYJKoZI\r\nhvcNAQEBBQADggEPADCCAQoCggEBANUMOsQq+U7i9b4Zl1+OiFOxHz/Lz58gE20p\r\nOsgPfTz3a3Y4Y9k2YKibXlwAgLIvWX/2h/klQ4bnaRtSmpDhcePYLQ1Ob/bISdm2\r\n8xpWriu2dBTrz/sm4xq6HZYuajtYlIlHVv8loJNwU4PahHQUw2eeBGg6345AWh1K\r\nTs9DkTvnVtYAcMtS7nt9rjrnvDH5RfbCYM8TWQIrgMw0R9+53pBlbQLPLJGmpufe\r\nhRhJfGZOozptqbXuNC66DQO4M99H67FrjSXZm86B0UVGMpZwh94CDklDhbZsc7tk\r\n6mFBrMnUVN+HL8cisibMn1lUaJ/8viovxFUcdUBgF4UCVTmLfwUCAwEAAaNCMEAw\r\nDwYDVR0TAQH/BAUwAwEB/zAOBgNVHQ8BAf8EBAMCAQYwHQYDVR0OBBYEFJxfAN+q\r\nAdcwKziIorhtSpzyEZGDMA0GCSqGSIb3DQEBCwUAA4IBAQBLNqaEd2ndOxmfZyMI\r\nbw5hyf2E3F/YNoHN2BtBLZ9g3ccaaNnRbobhiCPPE95Dz+I0swSdHynVv/heyNXB\r\nve6SbzJ08pGCL72CQnqtKrcgfU28elUSwhXqvfdqlS5sdJ/PHLTyxQGjhdByPq1z\r\nqwubdQxtRbeOlKyWN7Wg0I8VRw7j6IPdj/3vQQF3zCepYoUz8jcI73HPdwbeyBkd\r\niEDPfUYd/x7H4c7/I9vG+o1VTqkC50cRRj70/b17KSa7qWFiNyi2LSr2EIZkyXCn\r\n0q23KXB56jzaYyWf/Wi3MOxw+3WKt21gZ7IeyLnp2KhvAotnDU0mV3HaIPzBSlCN\r\nsSi6\r\n-----END CERTIFICATE-----\r\n-----BEGIN CERTIFICATE-----\r\nMIIFQTCCAymgAwIBAgITBmyf0pY1hp8KD+WGePhbJruKNzANBgkqhkiG9w0BAQwF\r\nADA5MQswCQYDVQQGEwJVUzEPMA0GA1UEChMGQW1hem9uMRkwFwYDVQQDExBBbWF6\r\nb24gUm9vdCBDQSAyMB4XDTE1MDUyNjAwMDAwMFoXDTQwMDUyNjAwMDAwMFowOTEL\r\nMAkGA1UEBhMCVVMxDzANBgNVBAoTBkFtYXpvbjEZMBcGA1UEAxMQQW1hem9uIFJv\r\nb3QgQ0EgMjCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAK2Wny2cSkxK\r\ngXlRmeyKy2tgURO8TW0G/LAIjd0ZEGrHJgw12MBvIITplLGbhQPDW9tK6Mj4kHbZ\r\nW0/jTOgGNk3Mmqw9DJArktQGGWCsN0R5hYGCrVo34A3MnaZMUnbqQ523BNFQ9lXg\r\n1dKmSYXpN+nKfq5clU1Imj+uIFptiJXZNLhSGkOQsL9sBbm2eLfq0OQ6PBJTYv9K\r\n8nu+NQWpEjTj82R0Yiw9AElaKP4yRLuH3WUnAnE72kr3H9rN9yFVkE8P7K6C4Z9r\r\n2UXTu/Bfh+08LDmG2j/e7HJV63mjrdvdfLC6HM783k81ds8P+HgfajZRRidhW+me\r\nz/CiVX18JYpvL7TFz4QuK/0NURBs+18bvBt+xa47mAExkv8LV/SasrlX6avvDXbR\r\n8O70zoan4G7ptGmh32n2M8ZpLpcTnqWHsFcQgTfJU7O7f/aS0ZzQGPSSbtqDT6Zj\r\nmUyl+17vIWR6IF9sZIUVyzfpYgwLKhbcAS4y2j5L9Z469hdAlO+ekQiG+r5jqFoz\r\n7Mt0Q5X5bGlSNscpb/xVA1wf+5+9R+vnSUeVC06JIglJ4PVhHvG/LopyboBZ/1c6\r\n+XUyo05f7O0oYtlNc/LMgRdg7c3r3NunysV+Ar3yVAhU/bQtCSwXVEqY0VThUWcI\r\n0u1ufm8/0i2BWSlmy5A5lREedCf+3euvAgMBAAGjQjBAMA8GA1UdEwEB/wQFMAMB\r\nAf8wDgYDVR0PAQH/BAQDAgGGMB0GA1UdDgQWBBSwDPBMMPQFWAJI/TPlUq9LhONm\r\nUjANBgkqhkiG9w0BAQwFAAOCAgEAqqiAjw54o+Ci1M3m9Zh6O+oAA7CXDpO8Wqj2\r\nLIxyh6mx/H9z/WNxeKWHWc8w4Q0QshNabYL1auaAn6AFC2jkR2vHat+2/XcycuUY\r\n+gn0oJMsXdKMdYV2ZZAMA3m3MSNjrXiDCYZohMr/+c8mmpJ5581LxedhpxfL86kS\r\nk5Nrp+gvU5LEYFiwzAJRGFuFjWJZY7attN6a+yb3ACfAXVU3dJnJUH/jWS5E4ywl\r\n7uxMMne0nxrpS10gxdr9HIcWxkPo1LsmmkVwXqkLN1PiRnsn/eBG8om3zEK2yygm\r\nbtmlyTrIQRNg91CMFa6ybRoVGld45pIq2WWQgj9sAq+uEjonljYE1x2igGOpm/Hl\r\nurR8FLBOybEfdF849lHqm/osohHUqS0nGkWxr7JOcQ3AWEbWaQbLU8uz/mtBzUF+\r\nfUwPfHJ5elnNXkoOrJupmHN5fLT0zLm4BwyydFy4x2+IoZCn9Kr5v2c69BoVYh63\r\nn749sSmvZ6ES8lgQGVMDMBu4Gon2nL2XA46jCfMdiyHxtN/kHNGfZQIG6lzWE7OE\r\n76KlXIx3KadowGuuQNKotOrN8I1LOJwZmhsoVLiJkO/KdYE+HvJkJMcYr07/R54H\r\n9jVlpNMKVv/1F2Rs76giJUmTtt8AF9pYfl3uxRuw0dFfIRDH+fO6AgonB8Xx1sfT\r\n4PsJYGw=\r\n-----END CERTIFICATE-----\r\n-----BEGIN CERTIFICATE-----\r\nMIIBtjCCAVugAwIBAgITBmyf1XSXNmY/Owua2eiedgPySjAKBggqhkjOPQQDAjA5\r\nMQswCQYDVQQGEwJVUzEPMA0GA1UEChMGQW1hem9uMRkwFwYDVQQDExBBbWF6b24g\r\nUm9vdCBDQSAzMB4XDTE1MDUyNjAwMDAwMFoXDTQwMDUyNjAwMDAwMFowOTELMAkG\r\nA1UEBhMCVVMxDzANBgNVBAoTBkFtYXpvbjEZMBcGA1UEAxMQQW1hem9uIFJvb3Qg\r\nQ0EgMzBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABCmXp8ZBf8ANm+gBG1bG8lKl\r\nui2yEujSLtf6ycXYqm0fc4E7O5hrOXwzpcVOho6AF2hiRVd9RFgdszflZwjrZt6j\r\nQjBAMA8GA1UdEwEB/wQFMAMBAf8wDgYDVR0PAQH/BAQDAgGGMB0GA1UdDgQWBBSr\r\nttvXBp43rDCGB5Fwx5zEGbF4wDAKBggqhkjOPQQDAgNJADBGAiEA4IWSoxe3jfkr\r\nBqWTrBqYaGFy+uGh0PsceGCmQ5nFuMQCIQCcAu/xlJyzlvnrxir4tiz+OpAUFteM\r\nYyRIHN8wfdVoOw==\r\n-----END CERTIFICATE-----\r\n-----BEGIN CERTIFICATE-----\r\nMIIB8jCCAXigAwIBAgITBmyf18G7EEwpQ+Vxe3ssyBrBDjAKBggqhkjOPQQDAzA5\r\nMQswCQYDVQQGEwJVUzEPMA0GA1UEChMGQW1hem9uMRkwFwYDVQQDExBBbWF6b24g\r\nUm9vdCBDQSA0MB4XDTE1MDUyNjAwMDAwMFoXDTQwMDUyNjAwMDAwMFowOTELMAkG\r\nA1UEBhMCVVMxDzANBgNVBAoTBkFtYXpvbjEZMBcGA1UEAxMQQW1hem9uIFJvb3Qg\r\nQ0EgNDB2MBAGByqGSM49AgEGBSuBBAAiA2IABNKrijdPo1MN/sGKe0uoe0ZLY7Bi\r\n9i0b2whxIdIA6GO9mif78DluXeo9pcmBqqNbIJhFXRbb/egQbeOc4OO9X4Ri83Bk\r\nM6DLJC9wuoihKqB1+IGuYgbEgds5bimwHvouXKNCMEAwDwYDVR0TAQH/BAUwAwEB\r\n/zAOBgNVHQ8BAf8EBAMCAYYwHQYDVR0OBBYEFNPsxzplbszh2naaVvuc84ZtV+WB\r\nMAoGCCqGSM49BAMDA2gAMGUCMDqLIfG9fhGt0O9Yli/W651+kI0rz2ZVwyzjKKlw\r\nCkcO8DdZEv8tmZQoTipPNU0zWgIxAOp1AE47xDqUEpHJWEadIRNyp4iciuRMStuW\r\n1KyLa2tJElMzrdfkviT8tQp21KW8EA==\r\n-----END CERTIFICATE-----";

    // Token: 0x0400025C RID: 604
    private static X509Certificate2[] _rootCertificates;

    // Token: 0x02000164 RID: 356
    private class PasswordFinder : IPasswordFinder
    {
        // Token: 0x06000892 RID: 2194 RVA: 0x00016904 File Offset: 0x00014B04
        public PasswordFinder(byte[] password)
        {
            this._password = new char[password.Length];
            for (int i = 0; i < this._password.Length; i++)
            {
                this._password[i] = (char)password[i];
            }
        }

        // Token: 0x06000893 RID: 2195 RVA: 0x00016943 File Offset: 0x00014B43
        public PasswordFinder(string password)
        {
            this._password = password.ToCharArray();
        }

        // Token: 0x06000894 RID: 2196 RVA: 0x00016957 File Offset: 0x00014B57
        public char[] GetPassword()
        {
            return this._password;
        }

        // Token: 0x04000482 RID: 1154
        private readonly char[] _password;
    }

    // Token: 0x02000165 RID: 357
    private class RSACertificateEncryptionProvider : ICertificateEncryptionProvider, IDisposable
    {
        // Token: 0x06000895 RID: 2197 RVA: 0x0001695F File Offset: 0x00014B5F
        public RSACertificateEncryptionProvider(RsaPrivateCrtKeyParameters privateKey)
        {
            this._signer = SignerUtilities.GetSigner("SHA256WITHRSA");
            this._signer.Init(true, privateKey);
        }

        // Token: 0x06000896 RID: 2198 RVA: 0x00002273 File Offset: 0x00000473
        public void Dispose()
        {
        }

        // Token: 0x06000897 RID: 2199 RVA: 0x00016984 File Offset: 0x00014B84
        public byte[] SignData(byte[] data, int offset, int length)
        {
            this._signer.Reset();
            this._signer.BlockUpdate(data, offset, length);
            return this._signer.GenerateSignature();
        }

        // Token: 0x04000483 RID: 1155
        private readonly ISigner _signer;
    }
}
