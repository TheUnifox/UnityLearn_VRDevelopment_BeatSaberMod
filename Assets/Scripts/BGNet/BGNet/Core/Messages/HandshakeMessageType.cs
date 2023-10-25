using System;

namespace BGNet.Core.Messages
{
    // Token: 0x020000AC RID: 172
    public enum HandshakeMessageType
    {
        // Token: 0x04000290 RID: 656
        ClientHelloRequest,
        // Token: 0x04000291 RID: 657
        HelloVerifyRequest,
        // Token: 0x04000292 RID: 658
        ClientHelloWithCookieRequest,
        // Token: 0x04000293 RID: 659
        ServerHelloRequest,
        // Token: 0x04000294 RID: 660
        ServerCertificateRequest,
        // Token: 0x04000295 RID: 661
        ClientKeyExchangeRequest = 6,
        // Token: 0x04000296 RID: 662
        ChangeCipherSpecRequest,
        // Token: 0x04000297 RID: 663
        MessageReceivedAcknowledge,
        // Token: 0x04000298 RID: 664
        MultipartMessage
    }
}
