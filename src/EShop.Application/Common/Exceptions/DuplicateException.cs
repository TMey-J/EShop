namespace Blogger.Application.Common.Exceptions
{
    public class DuplicateException(string nameToReplace):ApplicationException($"{nameToReplace} از قبل موجود است")
    {
    }
}