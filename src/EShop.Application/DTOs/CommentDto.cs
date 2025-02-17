namespace EShop.Application.DTOs;

public record ShowCommentDto(
    long Id,
    string Body,
    long? ReplyId,
    byte Rating,
    DateTime CreatedDateTime,
    DateTime? ModifiedDateTime);