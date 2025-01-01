// MapsterConfig.cs
using UGH.Domain.Entities;
using UGHApi.ViewModels;
using Mapster;

public static class MapsterConfig
{
#pragma warning disable CS8632
    public static void RegisterMappings()
    {
        TypeAdapterConfig<User, UserDTO>
            .NewConfig()
            .Map(dest => dest.Hobbies, src => SplitAndTrim(src.Hobbies))
            .Map(dest => dest.Skills, src => SplitAndTrim(src.Skills));
    }

    private static List<string?> SplitAndTrim(string? input)
    {
#pragma warning disable CS8632
        return input?.Split(',').Select(h => h.Trim()).ToList() ?? new List<string?>();
    }
}
