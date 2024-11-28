using EShop.Application.Common.Exceptions;
using EShop.Application.Features.AdminPanel.Category.Handlers.Queries;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Handlers.AdminPanel.Category;

public class GetCategoryQueryHandlerTests
{
    private readonly GetCategoryQueryHandler _sut;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
    private GetCategoryQueryRequest _request = new();

    public GetCategoryQueryHandlerTests()
    {
        _sut = new GetCategoryQueryHandler(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnGetTagQueryResponse_WhenCategoryIsFound()
    {
        //Arrange
        const long categoryId = 1;
        var categoryTitle = "Test Category";
        var categoryParent = HierarchyId.GetRoot();
        var category = new EShop.Domain.Entities.Category()
        {
            Id = categoryId,
            Title = categoryTitle,
            Parent = categoryParent
        };
        _request = new() { Id = categoryId};
        _categoryRepositoryMock.Setup(x =>
                x.FindByIdAsync(categoryId))
            .ReturnsAsync(category);

        //Act
        var result = await _sut.Handle(_request, default);

        //Assert
        Assert.Equal(categoryId, result.Id);
        Assert.Equal(categoryTitle, result.Title);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCategoryIsNotFound()
    {
        //Arrange
        _request = new() { Id = It.IsAny<long>()};
        //Act
        var act = () => _sut.Handle(_request, default);

        //Assert
        var exception= await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal($"{NameToReplaceInException.Category} یافت نشد", exception.Message);
    }
}