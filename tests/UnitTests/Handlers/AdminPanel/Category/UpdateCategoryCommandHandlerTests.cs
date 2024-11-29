using EShop.Application.Common.Exceptions;
using EShop.Application.Contracts.Services;
using EShop.Application.Features.AdminPanel.Category.Handlers.Commands;
using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using EShop.Application.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace UnitTests.Handlers.AdminPanel.Category;

public class UpdateCategoryCommandHandlerTests
{
    private readonly UpdateCategoryCommandHandler _sut;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
    private readonly Mock<IFileRepository> _fileServiceMock = new();
    private readonly Mock<IOptionsSnapshot<SiteSettings>> _siteSettingsMock = new();
    private UpdateCategoryCommandRequest _request = new();

    public UpdateCategoryCommandHandlerTests()
    {
        _siteSettingsMock.Setup(x => x.Value).Returns(new SiteSettings());
        _sut = new UpdateCategoryCommandHandler(_categoryRepositoryMock.Object,_fileServiceMock.Object, _siteSettingsMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCategoryIsNotFound()
    {
        //Arrange
        var propertyToSearch = nameof(EShop.Domain.Entities.Category.Title);
        _categoryRepositoryMock.Setup(x =>
                x.FindByAsync(propertyToSearch, It.IsAny<string>()))
            .ReturnsAsync(() => null);

        //Act
        var act = () => _sut.Handle(_request, default);

        //Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal($"{NameToReplaceInException.Category} یافت نشد", exception.Message);

        _categoryRepositoryMock.Verify(x =>
            x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCategoryParentIsNotFound()
    {
        //Arrange
        long categoryId = 1;
        string categoryTitle = "test";
        HierarchyId categoryParent = HierarchyId.GetRoot();
        var categoryParentId = 2;
        var category = new EShop.Domain.Entities.Category()
        {
            Title = categoryTitle,
            Parent = categoryParent,
            Id = categoryId,
        };
        _categoryRepositoryMock.Setup(x =>
                x.FindByIdAsync(categoryId))
            .ReturnsAsync(category);

        _categoryRepositoryMock.Setup(x =>
                x.FindByIdAsync(categoryParentId))
            .ReturnsAsync(() => null);


        //Act
        _request = new() { NewTitle = categoryTitle,NewParentId = categoryParentId,Id = categoryId };
        var act = () => _sut.Handle(_request, default);

        //Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal($"{NameToReplaceInException.ParentCategory} یافت نشد", exception.Message);

        _categoryRepositoryMock.Verify(x =>
            x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnUpdateCategoryCommandResponse_WhenEverythingIsOk()
    {
        const long categoryId = 1;
        const string categoryTitle = "test";
        var categoryParent = HierarchyId.GetRoot();
        const int categoryParentId = 2;
        var category = new EShop.Domain.Entities.Category()
        {
            Title = categoryTitle,
            Parent = categoryParent,
            Id = categoryId,
        };
        var parentCategory = new EShop.Domain.Entities.Category()
        {
            Title = categoryTitle,
            Parent = categoryParent,
            Id = categoryParentId,
        };
        _categoryRepositoryMock.Setup(x =>
                x.FindByIdAsync(categoryId))
            .ReturnsAsync(category);
        
        _categoryRepositoryMock.Setup(x =>
                x.FindByIdAsync(categoryParentId))
            .ReturnsAsync(parentCategory);
        
        _categoryRepositoryMock.Setup(x =>
                x.GetCategoryChildrenAsync(category))
            .ReturnsAsync(()=>[]);
    
        //Act
        _request = new() { NewTitle = categoryTitle,NewParentId = categoryParentId,Id = categoryId };
        var result = await _sut.Handle(_request, default);
    
        //Assert
        Assert.IsType<UpdateCategoryCommandResponse>(result);
    
        _categoryRepositoryMock.Verify(x =>
            x.SaveChangesAsync(), Times.Once);
    }
}