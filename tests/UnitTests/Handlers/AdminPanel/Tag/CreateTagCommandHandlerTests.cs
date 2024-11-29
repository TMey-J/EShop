using EShop.Application.Features.AdminPanel.Tag.Handlers.Commands;
using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;


namespace UnitTests.Handlers.AdminPanel.Tag;

public class CreateTagCommandHandlerTests
{
    private readonly CreateTagCommandHandler _sut;
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();
    private readonly Mock<IRabbitmqPublisherService> _rabbitmqPublisher = new();
    private  CreateTagCommandRequest _request=new();

    public CreateTagCommandHandlerTests()
    {
        _sut = new CreateTagCommandHandler(_tagRepositoryMock.Object,_rabbitmqPublisher.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnThrowDuplicateException_WhenTagIsFound()
    {
        //Arrange
        long tagId=1;
        var tagTitle="test";
        var propertyToSearch = nameof(EShop.Domain.Entities.Tag.Title);
        var tag = new EShop.Domain.Entities.Tag()
        {
            Title = tagTitle,
            Id = tagId
        };
        _tagRepositoryMock.Setup(x =>
               x.FindByAsync(propertyToSearch,tagTitle))
             .ReturnsAsync(tag);

        //Act
        _request = new() { Title = tagTitle };
        var act = () => _sut.Handle(_request, default);
        
        //Assert
        var exception= await Assert.ThrowsAsync<DuplicateException>(act);
        Assert.Equal($"این {NameToReplaceInException.Tag} از قبل موجود است", exception.Message);
        
        _tagRepositoryMock.Verify(x=>
            x.CreateAsync(It.IsAny<EShop.Domain.Entities.Tag>()), Times.Never);
        
        _tagRepositoryMock.Verify(x=>
            x.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnCreateTagCommandResponse_WhenTagIsNotFound()
    {
        //Arrange
        var propertyToSearch = nameof(EShop.Domain.Entities.Tag.Title);
        _tagRepositoryMock.Setup(x =>
                x.FindByAsync(propertyToSearch,It.IsAny<string>()))
            .ReturnsAsync(()=>null);

        //Act
        var result=await _sut.Handle(_request, default);
        
        //Assert
        _tagRepositoryMock.Verify(x=>
            x.CreateAsync(It.IsAny<EShop.Domain.Entities.Tag>()), Times.Once);
        
        _tagRepositoryMock.Verify(x=>
            x.SaveChangesAsync(), Times.Once);

        Assert.IsType<CreateTagCommandResponse>(result);
    }
}