namespace UGH.Domain.Core;

public static class Errors
{
    public static class General
    {
        public static Error InvalidOperation(string operation) =>
            new("General.InvalidOperation", $"The operation '{operation}' is invalid.");

        public static Error NullOrEmpty(string fieldName) =>
            new("General.NullOrEmpty", $"The field '{fieldName}' cannot be null or empty.");

        public static Error AlreadyExists(string entityName, object key) =>
            new("General.AlreadyExists", $"The {entityName} with key '{key}' already exists.");

        public static Error NotFound(string entityName, object key) =>
            new("General.NotFound", $"The {entityName} with key '{key}' was not found.");

        public static Error NotAuthorized(string action = "perform this action") =>
            new("General.NotAuthorized", $"You are not authorized to {action}.");

        public static Error InvalidField(string fieldName, string reason) =>
            new("General.InvalidField", $"The field '{fieldName}' is invalid: {reason}.");

        public static Error UnexpectedError(string message = "An unexpected error occurred.") =>
            new("General.UnexpectedError", message);
    }

    public static class Validation
    {
        public static Error MoneyAmountMustBeGreaterThanZero(decimal amount) =>
            new("Validation.MoneyAmount", $"The amount '{amount}' must be greater than zero.");

        public static Error QuantityGreaterThanZero(int quantity) =>
            new("Validation.Quantity", $"The quantity '{quantity}' must be greater than zero.");

        public static Error DateTimeCannotBeMinValue() =>
            new("Validation.DateTimeMinValue", "The DateTime value cannot be equal to DateTime.MinValue.");

        public static Error EmptyGuid() =>
            new("Validation.EmptyGuid", "The GUID value cannot be empty.");
    }
}