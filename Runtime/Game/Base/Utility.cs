public static class Utility
{
    public static bool IsNullOrWhiteSpaceEx(this string _value)
    {
        if (string.IsNullOrWhiteSpace(_value))
            return true;

        if (_value == "null")
            return true;

        return false;
    }
}