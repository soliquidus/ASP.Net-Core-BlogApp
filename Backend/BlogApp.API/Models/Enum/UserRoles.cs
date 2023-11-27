using System.ComponentModel;

namespace BlogApp.API.Models.Enum;

public enum UserRoles
{
    [Description("Writer")]
    Writer,
    [Description("Reader")]
    Reader
}

public static class EnumExtensions
{
    public static string GetDescription(this System.Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        var attributes = fieldInfo.GetCustomAttributes(
            typeof(DescriptionAttribute), false
        ) as DescriptionAttribute[];

        return attributes != null && attributes.Length > 0
            ? attributes[0].Description
            : value.ToString();
    }
}