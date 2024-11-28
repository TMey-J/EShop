using EShop.Application.DTOs;
using EShop.Application.Features.AdminPanel.Category.Handlers.Queries;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;

namespace UnitTests.Handlers.AdminPanel.Category;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly GetAllCategoryQueryHandler _sut;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
    private readonly Mock<SearchCategoryDto> _searchCategoryDtoMock = new();
    private GetAllCategoryQueryRequest _request;

    public GetAllCategoriesQueryHandlerTests()
    {
        _sut = new GetAllCategoryQueryHandler(_categoryRepositoryMock.Object);
        _request = new GetAllCategoryQueryRequest(_searchCategoryDtoMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnGetAllCategoryQueryResponse_WhenCalled()
    {
        //Arrange
        const long categoryId = 1;
        var categoryTitle = "Test Category";
        long categoryParentId = 2;
        var categories = new List<ShowCategoryDto>()
        {
           new(categoryId,categoryTitle,categoryParentId,null)
        };
        
        var search = new SearchCategoryDto()
        {
            Title = categoryTitle,
            Pagination = new()
            {
                CurrentPage = 1
            }
        };
        
        _categoryRepositoryMock.Setup(x =>
                x.GetAllAsync(search))
            .ReturnsAsync(new GetAllCategoryQueryResponse(categories, search,1));

        //Act
        var result = await _sut.Handle(new (search), default);

        //Assert
        Assert.IsType<GetAllCategoryQueryResponse>(result);
        Assert.Contains(categories.First(), result.categories);
        Assert.True(result.pageCount>0);
    }
}