namespace EShop.Application.Constants.Common
{
    public static class Messages
    {
        public const string Successful = "عملیات با موفقیت انجام شد";
        public static class Validations
        {
            public const string Required = "وارد کردن {DisplayName} الزامی است";
            public const string MaxLength = "{DisplayName} نباید بیشتر از {MaxLength} کاراکتر باشد";
            public const string RegularExpression = "{DisplayName} را به درستی وارد کنید";
            public const string Length = "{DisplayName} باید از {MinLength} تا {MaxLength} کاراکتر باشد";
            public const string GreaterThanZero = "{DisplayName} باید بزرگ تر از 0 باشد";


        }
        public static class Subjects
        {
            public const string VeryfyCodeMailSubject = "کد ثبت حساب کاربری";
        }
        public static class Errors
        {
            public const string InternalServer = "خطایی در برنامه رخ داده لطفا بعدا امتحان کنید";
            public const string Validation = "مقادیر را به درستی وارد کنید";
            public const string BadRequest = "درخواست نامعتر است";

            public static string DuplicatedValue(string name)
            {
                return $"این {name} از قیل موجود است";
            }
        }
    }
}
