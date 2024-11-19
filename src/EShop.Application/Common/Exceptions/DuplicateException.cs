namespace Blogger.Application.Common.Exceptions
{
    public class DuplicateException(string nameToReplace):ApplicationException($"این{nameToReplace}از قبل موجود است")
    {
    }
}