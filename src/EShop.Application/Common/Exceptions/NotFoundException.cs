namespace EShop.Application.Common.Exceptions
{
    public class NotFoundException(string nameToReplace):ApplicationException($"{nameToReplace} یافت نشد")
    {
    }
}