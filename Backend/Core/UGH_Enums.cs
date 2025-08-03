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

    public enum TransactionStatus
    {
        Pending = 0,
        Complete = 1,
        Failed = 2,
    }
}
