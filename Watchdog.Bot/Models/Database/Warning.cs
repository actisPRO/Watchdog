using Watchdog.Bot.Utils;

namespace Watchdog.Bot.Models.Database;

public sealed class Warning : IEntity
{
    public string Id { get; set; } = IdGenerator.GenerateId();

    public required ulong GuildId { get; set; }

    public Guild Guild { get; set; } = default!;

    public required ulong UserId { get; set; }

    public required ulong ModeratorId { get; set; }

    public required string Reason { get; set; }

    public required DateTime CreatedAt { get; set; }

    public object[] GetIdentity() => new object[] { Id };
}