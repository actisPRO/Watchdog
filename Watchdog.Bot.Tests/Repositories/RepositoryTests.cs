using AutoFixture;
using FluentAssertions;
using Watchdog.Bot.Exceptions;
using Watchdog.Bot.Repositories.Interfaces;
using Watchdog.Bot.Tests.Helpers;

namespace Watchdog.Bot.Tests.Repositories;

public sealed class RepositoryTests
{
    private IRepository<SampleModel> _sut = default!;
    private InMemoryContext _context = default!;
    
    [SetUp]
    public void Setup()
    {
        _context = new InMemoryContext();
        _sut = new SampleRepository(_context);
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
    
    [Test]
    public async Task AddAndGetByIdTest()
    {
        // Arrange
        var fixture = new Fixture().Create<SampleModel>();
        
        // Act
        await _sut.AddAsync(fixture);
        var result = await _sut.GetByIdAsync(fixture.Id, fixture.Name);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fixture);
    }

    [Test]
    public async Task GetCountTest()
    {
        // Arrange
        var models = await PopulateDbAsync(10);
        
        // Act
        var count = await _sut.GetCountAsync();
        
        // Assert
        count.Should().Be(10);
    }

    [Test]
    public async Task GetAllTest()
    {
        // Arrange
        var models = await PopulateDbAsync(10);
        
        // Act
        var result = await _sut.GetAllAsync();
        
        // Assert
        result.Should().BeEquivalentTo(models);
    }
    
    [Test]
    public async Task FindTest()
    {
        // Arrange
        var models = await PopulateDbAsync(10);
        var expected = models.ToArray()[5];
        
        // Act
        var result = await _sut.FindAsync(x => x.Description == expected.Description);
        var actual = result.FirstOrDefault();
        
        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task UpdateTest()
    {
        // Arrange
        var models = await PopulateDbAsync(10);
        var modelToEdit = models.First();
        
        // Act
        modelToEdit.Description = Guid.NewGuid().ToString();
        await _sut.UpdateAsync(modelToEdit);
        var result = await _sut.GetByIdAsync(modelToEdit.Id, modelToEdit.Name);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(modelToEdit);
    }
    
    [Test]
    public async Task DeleteTest()
    {
        // Arrange
        var models = await PopulateDbAsync(10);
        var modelToDelete = models.First();
        
        // Act
        await _sut.DeleteAsync(modelToDelete.Id, modelToDelete.Name);
        var result = await _sut.GetByIdAsync(modelToDelete.Id, modelToDelete.Name);
        
        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void UpdateNonExistingEntityTest()
    {
        // Arrange
        var model = new Fixture().Create<SampleModel>();
        
        // Act
        var act = async Task () => await _sut.UpdateAsync(model);
        
        // Assert
        act.Should().ThrowAsync<ObjectNotFoundException>();
    }
    
    [Test]
    public void DeleteNonExistingEntityTest()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        var act = async Task () => await _sut.DeleteAsync(id);
        
        // Assert
        act.Should().ThrowAsync<ObjectNotFoundException>();
    }

    private async Task<IEnumerable<SampleModel>> PopulateDbAsync(int count)
    {
        var models = new List<SampleModel>();
        for (int i = 0; i < count; ++i)
        {
            var model = new Fixture().Create<SampleModel>();
            models.Add(await _sut.AddAsync(model));;
        }

        return models;
    }
}