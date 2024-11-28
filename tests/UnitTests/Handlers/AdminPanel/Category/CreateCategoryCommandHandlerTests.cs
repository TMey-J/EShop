using EShop.Application.Common.Exceptions;
using EShop.Application.Contracts.Services;
using EShop.Application.Features.AdminPanel.Category.Handlers.Commands;
using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using EShop.Application.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace UnitTests.Handlers.AdminPanel.Category;

public class CreateCategoryCommandHandlerTests
{
    private readonly CreateCategoryCommandHandler _sut;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
    private readonly Mock<IFileServices> _fileServiceMock = new();
    private readonly Mock<IOptionsSnapshot<SiteSettings>> _siteSettingsMock = new();
    private CreateCategoryCommandRequest _request = new();

    public CreateCategoryCommandHandlerTests()
    {
        _siteSettingsMock.Setup(x => x.Value).Returns(new SiteSettings());
        _sut = new CreateCategoryCommandHandler(_categoryRepositoryMock.Object,_fileServiceMock.Object, _siteSettingsMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowDuplicateException_WhenCategoryIsFound()
    {
        //Arrange
        var categoryTitle = "test";
        var propertyToSearch = nameof(EShop.Domain.Entities.Category.Title);
        var category = new EShop.Domain.Entities.Category()
        {
            Title = categoryTitle
        };
        _categoryRepositoryMock.Setup(x =>
                x.FindByAsync(propertyToSearch, categoryTitle))
            .ReturnsAsync(category);

        //Act
        _request = new() { Title = categoryTitle };
        var act = () => _sut.Handle(_request, default);

        //Assert
        var exception = await Assert.ThrowsAsync<DuplicateException>(act);
        Assert.Equal($"این {NameToReplaceInException.Category} از قبل موجود است", exception.Message);

        _categoryRepositoryMock.Verify(x =>
            x.CreateAsync(It.IsAny<EShop.Domain.Entities.Category>()), Times.Never);

        _categoryRepositoryMock.Verify(x =>
            x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCategoryParentIsNotFound()
    {
        //Arrange
        var categoryTitle = It.IsAny<string>();
        var categoryParentId = It.IsAny<long>();
        var propertyToSearch = nameof(EShop.Domain.Entities.Category.Title);

        _categoryRepositoryMock.Setup(x =>
                x.FindByAsync(propertyToSearch, categoryTitle))
            .ReturnsAsync(() => null);

        _categoryRepositoryMock.Setup(x =>
                x.FindByIdAsync(categoryParentId))
            .ReturnsAsync(() => null);

        //Act
        _request = new() { Title = categoryTitle, Parent = categoryParentId };
        var act = () => _sut.Handle(_request, default);

        //Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal($"{NameToReplaceInException.ParentCategory} یافت نشد", exception.Message);

        _categoryRepositoryMock.Verify(x =>
            x.CreateAsync(It.IsAny<EShop.Domain.Entities.Category>()), Times.Never);

        _categoryRepositoryMock.Verify(x =>
            x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnCreateCategoryCommandResponse_WhenEverythingIsOk()
    {
        //Arrange
        var categoryTitle = "test";
        var categoryParentIdHierarchyId = HierarchyId.GetRoot();
        var categoryParentId = 1;
        var category = new EShop.Domain.Entities.Category()
        {
            Title = categoryTitle, 
            Parent = categoryParentIdHierarchyId,
            Id = categoryParentId
        };
        var propertyToSearch = nameof(EShop.Domain.Entities.Category.Title);

        _categoryRepositoryMock.Setup(x =>
                x.FindByAsync(propertyToSearch, categoryTitle))
            .ReturnsAsync(() => null);

        _categoryRepositoryMock.Setup(x =>
                x.FindByIdAsync(categoryParentId))
            .ReturnsAsync(category);

        //Act
        _request = new() { Title = categoryTitle, Parent = categoryParentId };
        var result = await _sut.Handle(_request, default);

        //Assert
        Assert.IsType<CreateCategoryCommandResponse>(result);

        _categoryRepositoryMock.Verify(x =>
            x.CreateAsync(It.IsAny<EShop.Domain.Entities.Category>()), Times.Once);

        _categoryRepositoryMock.Verify(x =>
            x.SaveChangesAsync(), Times.Once);
    }
}