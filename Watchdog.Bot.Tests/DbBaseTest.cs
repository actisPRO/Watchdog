using Microsoft.EntityFrameworkCore.Storage;
using Watchdog.Bot.Extensions;
using Watchdog.Bot.Options;
using Watchdog.Bot.Tests.Helpers.Attributes;

namespace Watchdog.Bot.Tests;

public abstract class DbBaseTest : BaseTest
{
    private DatabaseContext _context = default!;
    private IDbContextTransaction? _currentTransaction;
    
    protected DatabaseContext Context => _context.ThrowIfNull();
    
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
    }
    
    [OneTimeTearDown]
    public void DatabaseOneTimeTearDown()
    {
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