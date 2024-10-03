// MapsterConfig.cs
using Mapster;
using UGH.Domain.Entities;
using UGHApi.ViewModels;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<User, UserDTO>
            .NewConfig()
            .Map(dest => dest.Hobbies, src => SplitAndTrim(src.Hobbies))
            .Map(dest => dest.Skills, src => SplitAndTrim(src.Skills));
    }

    private static List<string?> SplitAndTrim(string? input)
    {
        return input?.Split(',').Select(h => h.Trim()).ToList() ?? new List<string?>();
    }
}
