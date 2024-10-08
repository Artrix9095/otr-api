using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using OsuApiClient.Domain.Osu.Multiplayer;
using OsuApiClient.Net.JsonModels.Osu.Beatmaps;

namespace OsuApiClient.Domain.Osu.Beatmaps;

/// <summary>
/// Represents a beatmapset of a <see cref="Beatmap"/> played in a <see cref="MultiplayerGame"/>
/// </summary>
[AutoMap(typeof(BeatmapsetJsonModel))]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class Beatmapset : IModel
{
    /// <summary>
    /// Id of the beatmapset
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Song artist
    /// </summary>
    public string Artist { get; init; } = null!;

    /// <summary>
    /// Song artist including unicode characters
    /// </summary>
    public string ArtistUnicode { get; init; } = null!;

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    /// Title including unicode characters
    /// </summary>
    public string TitleUnicode { get; init; } = null!;

    /// <summary>
    /// Id of the <see cref="Creator"/>
    /// </summary>
    [SourceMember(nameof(BeatmapsetJsonModel.UserId))]
    public int? CreatorId { get; init; }

    /// <summary>
    /// Username of the creator
    /// </summary>
    public string? Creator { get; init; }

    /// <summary>
    /// Cover image urls
    /// </summary>
    public BeatmapsetCovers Covers { get; init; } = null!;

    /// <summary>
    /// Number of favorites
    /// </summary>
    public int FavouriteCount { get; init; }

    /// <summary>
    /// Hype information
    /// </summary>
    public BeatmapsetHype? Hype { get; init; }

    /// <summary>
    /// Denotes if the set contains nsfw content
    /// </summary>
    public bool Nsfw { get; init; }

    /// <summary>
    /// Included audio offset
    /// </summary>
    public double Offset { get; init; }

    /// <summary>
    /// Play count
    /// </summary>
    public long PlayCount { get; init; }

    /// <summary>
    /// Preview image url
    /// </summary>
    public string? PreviewUrl { get; init; }

    /// <summary>
    /// No description
    /// </summary>
    public string? Source { get; init; }

    /// <summary>
    /// Denotes if the set is part of a beatmap spotlight
    /// </summary>
    public bool Spotlight { get; init; }

    /// <summary>
    /// Ranked status
    /// </summary>
    public string Status { get; init; } = null!;

    /// <summary>
    /// No description
    /// </summary>
    public long? TrackId { get; init; }

    /// <summary>
    /// Denotes if the set includes video
    /// </summary>
    public bool Video { get; init; }
}
