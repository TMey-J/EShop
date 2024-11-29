using Blogger.Application.Common.Exceptions;
using EShop.Application.Common.Exceptions;
using EShop.Application.Constants;
using EShop.Application.Contracts;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.DTOs;
using EShop.Application.Features.AdminPanel.Tag.Handlers.Queries;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;


namespace UnitTests.Handlers.AdminPanel.Tag;

public class GetAllTagsQueryHandlerTests
{
    private readonly GetAllTagsQueryHandler _sut;
    private readonly Mock<IMongoTagRepository> _tagRepositoryMock = new();

    public GetAllTagsQueryHandlerTests()
    {
        _sut = new GetAllTagsQueryHandler(_tagRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnGetAllTagsQueryResponse_WhenCalled()
    {
        //Arrange
        var tagId = 1;
        var tagTitle = "test";
        var tags = new List<ShowTagDto>()
        {
           new(tagId, tagTitle)
        };
        
        var search = new SearchTagDto()
        {
            Title = tagTitle,
            Pagination = new()
            {
                CurrentPage = 1
            }
        };
        
        _tagRepositoryMock.Setup(x =>
                x.GetAllAsync(search))
            .ReturnsAsync(new GetAllTagsQueryResponse(tags, search,1));

        //Act
        var result = await _sut.Handle(new (search), default);

        //Assert
        Assert.IsType<GetAllTagsQueryResponse>(result);
        Assert.Contains(tags.First(), result.Tags);
        Assert.True(result.PageCount>0);
    }
}