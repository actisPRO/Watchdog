using Microsoft.EntityFrameworkCore.Storage;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Options;
using Watchdog.Bot.Tests.Helpers.Attributes;
using Watchdog.Bot.Tests.Helpers.Generators;

namespace Watchdog.Bot.Tests;

public abstract class DbBaseTest : BaseTest
{
    private DatabaseContext _context = default!;
    private IDbContextTransaction? _currentTransaction;
    
    protected DatabaseContext Context => _context.ThrowIfNull();

    protected GuildGenerator GuildGenerator { get; private set; } = default!;
    
    [OneTimeSetUp]
    public void DatabaseOneTimeSetUp()
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
                               ?? "Server=localhost;Database=watchdog-tests;User Id=postgres;Password=postgres;";
        
        var options = Microsoft.Extensions.Options.Options.Create<DatabaseOptions>(new ()
        {
            ConnectionString = connectionString
        });
        
        _context = new DatabaseContext(options);
        _context.Database.EnsureCreated();
        
        GuildGenerator = new GuildGenerator(_context);
    }
    
    [OneTimeTearDown]
    public void DatabaseOneTimeTearDown()
    {
        _context.Database.EnsureDeleted();
        Context.Dispose();
    }
    
    [SetUp]
    public void DatabaseSetUp()
    {
        var currentType = GetType();
        var currentMethod = currentType.GetMethod(TestContext.CurrentContext.Test.MethodName ?? string.Empty)!;

        if (!NonTransactionalAttribute.IsNonTransactional(currentType) 
            && !NonTransactionalAttribute.IsNonTransactional(currentMethod))
        {
            if (_currentTransaction != null)
                throw new InvalidOperationException("Unable to start tests because a transaction was not disposed.");
            _currentTransaction = Context.Database.BeginTransaction();
        }
    }
    
    [TearDown]
    public void DatabaseTearDown()
    {
        _currentTransaction?.Rollback();
        _currentTransaction?.Dispose();
        _currentTransaction = null;
    }
}