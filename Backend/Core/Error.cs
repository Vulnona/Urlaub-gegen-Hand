namespace UGH.Domain.Core;

public sealed class Error : ValueObject<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new(
        "Error.NulLValue",
        "The specified result value is null."
    );

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
