using Blogger.Application.Common.Exceptions;
using EShop.Application.Common.Exceptions;
using EShop.Application.Constants;
using EShop.Application.Contracts;
using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Tag.Handlers.Queries;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;


namespace UnitTests.Handlers.AdminPanel.Tag;

public class GetTagQueryHandlerTests
{
    private readonly GetTagQueryHandler _sut;
    private readonly Mock<IMongoTagRepository> _tagRepositoryMock = new();
    private GetTagQueryRequest _request = new();

    public GetTagQueryHandlerTests()
    {
        _sut = new GetTagQueryHandler(_tagRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnGetTagQueryResponse_WhenTagIsFound()
    {
        //Arrange
        const long tagId = 1;
        var tagTitle = "TestTag";
        var tag = new EShop.Domain.Entities.Tag()
        {
            Id = tagId,
            Title = tagTitle
        };
        _request = new() { Id = tagId };
        _tagRepositoryMock.Setup(x =>
                x.FindByIdAsync(tagId))
            .ReturnsAsync(tag);

        //Act
        var result = await _sut.Handle(_request, default);

        //Assert
        Assert.Equal(tagId, result.Id);
        Assert.Equal(tagTitle, result.Title);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenTagIsNotFound()
    {
        //Arrange
        _request = new GetTagQueryRequest { Id = It.IsAny<long>()};
        //Act
        var act = () => _sut.Handle(_request, default);

        //Assert
        var exception= await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal($"{NameToReplaceInException.Tag} یافت نشد", exception.Message);
    }
}