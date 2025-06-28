namespace UGH.Domain.Core;

public class UGH_Enums
{
    public enum VerificationState
    {
        IsNew,
        VerificationPending,
        VerificationFailed,
        Verified,
    }

    public enum CouponDuration
    {
        Keiner = 0,
        EinJahr = 365,
        ZweiJahre = 730,
        DreiJahre = 1095,     
        Lebenslang    
    }

    public enum TransactionStatus
    {
        Pending = 0,
        Complete = 1,
        Failed = 2,
    }
}
