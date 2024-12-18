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
            public const string ExactLength = "{DisplayName} باید {MinLength} کاراکتر باشد";
            public const string GreaterThanZero = "{DisplayName} باید بزرگ تر از 0 باشد";
            public const string Between = "{DisplayName} باید بین {From} تا {To} باشد";
            public static string NotInEnum(int min,int max) => $"مقدار فیلد '{{DisplayName}}' معتبر نیست.باید بین {min} تا {max} باشد";


        }
        public static class Subjects
        {
            public const string VerifyCodeMailSubject = "کد ثبت حساب کاربری";
        }
        public static class Errors
        {
            public const string InternalServer = "خطایی در برنامه رخ داده لطفا بعدا امتحان کنید";
            public const string Validation = "مقادیر را به درستی وارد کنید";
            public const string BadRequest = "درخواست نامعتر است";
            public const string InvalidToken = "لینک فعال ساز نا معتبر است";
            public const string InvalidCode = "کد فعال ساز نا معتبر است";
            public const string EmailAlreadyVerified = "این ایمیل قبلا فعال شده است";
            public const string PhoneNumberAlreadyVerified = "این شماره تلفن قبلا فعال شده است";
            public const string InvalidTimeToSendCode = "زمان ارسال مجدد کد نرسیده است.";
            public const string UserNotActive = "حساب کاربری فعال نیست";
            public static List<string> NotExistsRolesErrors(List<string> rolesName)
            {
                return rolesName.Select(role => $"نقش {role} معتبر نیشت").ToList();
            }

            public static string DuplicatedValue(string name)
            {
                return $"این {name} از قبل موجود است";
            }
        }
    }
}
