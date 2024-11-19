using EShop.Application.Common.Exceptions;
using EShop.Application.Features.AdminPanel.Tag.Handlers.Commands;
using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;


namespace UnitTests.Handlers.AdminPanel.Tag;

public class UpdateTagCommandHandlerTests
{
    private readonly UpdateTagCommandHandler _sut;
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();
    private  UpdateTagCommandRequest _request=new();

    public UpdateTagCommandHandlerTests()
    {
        _sut = new UpdateTagCommandHandler(_tagRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnThrowNotFoundException_WhenTagIsNotFound()
    {
        //Arrange
        var propertyToSearch = nameof(EShop.Domain.Entities.Tag.Title);
        _tagRepositoryMock.Setup(x =>
               x.FindByAsync(propertyToSearch,It.IsAny<string>()))
             .ReturnsAsync(()=>null);

        //Act
        var act = () => _sut.Handle(_request, default);
        
        //Assert
        var exception= await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal($"{NameToReplaceInException.Tag} یافت نشد", exception.Message);
        
        _tagRepositoryMock.Verify(x=>
            x.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowDuplicateException_WhenNewTitleIsExists()
    {
        //Arrange
        const long tagId = 1;
        const string tagTitle = "test";
        const string propertyToSearch = nameof(EShop.Domain.Entities.Tag.Title);
        var tag = new EShop.Domain.Entities.Tag()
        {
            Title = tagTitle,
            Id = tagId
        };
        _tagRepositoryMock.Setup(x =>
                x.FindByIdAsync(tagId))
            .ReturnsAsync(tag);

        _tagRepositoryMock.Setup(x =>
                x.IsExistsByAsync(propertyToSearch,It.IsAny<string>(),It.IsAny<long>()))
            .ReturnsAsync(true);
        
        _request = new() { Title = tagTitle,Id = tagId };

        //Act
        var act = () => _sut.Handle(_request, default);
        
        //Assert
        var exception= await Assert.ThrowsAsync<DuplicateException>(act);
        Assert.Equal($"این {NameToReplaceInException.Tag} از قبل موجود است", exception.Message);
        
        _tagRepositoryMock.Verify(x=>
            x.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnUpdateTagCommandResponse_WhenEverythingIsOk()
    {
        //Arrange
        const long tagId = 1;
        const string tagTitle = "test";
        const string propertyToSearch = nameof(EShop.Domain.Entities.Tag.Title);
        var tag = new EShop.Domain.Entities.Tag()
        {
            Title = tagTitle,
            Id = tagId
        };
        _tagRepositoryMock.Setup(x =>
                x.FindByIdAsync(tagId))
            .ReturnsAsync(tag);

        _tagRepositoryMock.Setup(x =>
                x.IsExistsByAsync(propertyToSearch,It.IsAny<string>(),It.IsAny<long>()))
            .ReturnsAsync(false);
        
        _request = new() { Title = tagTitle,Id = tagId };

        //Act
        var result = await _sut.Handle(_request, default);
        
        //Assert
        Assert.IsType<UpdateTagCommandResponse>(result);
        _tagRepositoryMock.Verify(x=>
            x.SaveChangesAsync(), Times.Once);
    }
}