using Microsoft.EntityFrameworkCore;
using Watchdog.Bot.Models;

namespace Watchdog.Bot.Tests.Repositories.Helpers;

[PrimaryKey(nameof(Id), nameof(Name))]
public sealed class SampleModel : IEntity
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public object[] GetIdentity()
    {
        return new object[] { Id, Name };
    }
}