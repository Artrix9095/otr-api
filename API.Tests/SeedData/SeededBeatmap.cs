using Database.Entities;

namespace APITests.SeedData;

public static class SeededBeatmap
{
    public static Beatmap Get() =>
        new()
        {
            Id = 1369,
            Artist = "POLKADOT STINGRAY",
            OsuId = 2699135,
            Bpm = 165,
            MapperId = 2233878,
            MapperName = "moph",
            Sr = 6.5007,
            Cs = 4,
            Ar = 9.5,
            Hp = 6,
            Od = 9,
            Length = 188,
            Title = "Otoshimae",
            DiffName = "Extreme",
            Ruleset = 0,
            CircleCount = 674,
            SliderCount = 430,
            SpinnerCount = 0,
            MaxCombo = 1551,
            Created = new DateTime(2023, 09, 03),
        };
}
