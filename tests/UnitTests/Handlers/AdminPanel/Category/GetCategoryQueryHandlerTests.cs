using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Category.Handlers.Queries;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Handlers.AdminPanel.Category;

public class GetCategoryQueryHandlerTests
{
    private readonly GetCategoryQueryHandler _sut;
    private readonly Mock<IMongoCategoryRepository> _mongoCategoryRepositoryMock = new();
    private GetCategoryQueryRequest _request = new();

    public GetCategoryQueryHandlerTests()
    {
        _sut = new GetCategoryQueryHandler(_mongoCategoryRepositoryMock.Object);
    }

    // [Fact]
    // public async Task Handle_ShouldReturnGetCategoryQueryResponse_WhenCategoryIsFound()
    // {
    //     //Arrange
    //     const long categoryId = 1;
    //     var categoryTitle = "Test Category";
    //     var categoryParent = 1;
    //     var category = new EShop.Domain.Entities.Category()
    //     {
    //         Id = categoryId,
    //         Title = categoryTitle,
    //         ParentId = categoryParent
    //     };
    //     _request = new() { Id = categoryId};
    //     _mongoCategoryRepositoryMock.Setup(x =>
    //             x.FindByIdAsync(categoryId))
    //         .ReturnsAsync(category);
    //
    //     //Act
    //     var result = await _sut.Handle(_request, default);
    //
    //     //Assert
    //     Assert.Equal(categoryId, result.Id);
    //     Assert.Equal(categoryTitle, result.Title);
    // }

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