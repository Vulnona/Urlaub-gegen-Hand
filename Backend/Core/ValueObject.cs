namespace UGH.Domain.Core;

public abstract class ValueObject<T>
where T : ValueObject<T>
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not ValueObject<T> valueObject)
        {
            return false;
        }

        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents().Select(x => x is not null ? x.GetHashCode() : 0).Aggregate((x, y) => x ^ y);
    }

    protected static bool EqualOperator(ValueObject<T> left, ValueObject<T> right)
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return ReferenceEquals(left, right) || left.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject<T> left, ValueObject<T> right)
    {
        return !EqualOperator(left, right);
    }
}
