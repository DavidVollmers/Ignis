using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Ignis.Tests.E2E.Website.Shared;

public static class InputExtensions
{
    public static bool TryParseSelectableValueFromString<T>(this InputBase<T> input, string? value, out T result,
        out string? validationErrorMessage)
    {
        try
        {
            // We special-case bool values because BindConverter reserves bool conversion for conditional attributes.
            if (typeof(T) == typeof(bool))
            {
                if (TryConvertToBool(value, out result))
                {
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (typeof(T) == typeof(bool?))
            {
                if (TryConvertToNullableBool(value, out result))
                {
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (typeof(T) == typeof(Type))
            {
                if (value != null && Type.GetType(value) is { } type)
                {
                    result = (T)(object)type;
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (BindConverter.TryConvertTo<T>(value, CultureInfo.CurrentCulture, out var parsedValue))
            {
                result = parsedValue;
                validationErrorMessage = null;
                return true;
            }

            result = default!;
            validationErrorMessage = $"The {input.DisplayName} field is not valid.";
            return false;
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException($"{input.GetType()} does not support the type '{typeof(T)}'.", ex);
        }
    }

    private static bool TryConvertToBool<T>(string? value, out T result)
    {
        if (bool.TryParse(value, out var @bool))
        {
            result = (T)(object)@bool;
            return true;
        }

        result = default!;
        return false;
    }

    private static bool TryConvertToNullableBool<T>(string? value, out T result)
    {
        if (!string.IsNullOrEmpty(value))
        {
            return TryConvertToBool(value, out result);
        }

        result = default!;
        return true;
    }
}
