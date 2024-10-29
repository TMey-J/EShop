namespace EShop.Application.Constants;

public static class EmailTemplates
{
    public static string VerifyUserCodeEmail(string url,string email) =>
    $"""
        <div style="display: flex;justify-content: center;color: #F3EDC8;" dir="rtl">
            <div style="background-color: #9A031E;padding: 10px;width: 500px;border-radius: 20px;">
                <h1 style="text-align: center;">فروشگاه الکترونیک</h1>
                <hr />
                <h2 style="text-align: center;">درخواست ثبت نام در سایت</h2>
                <p>سلام {email} عزیز</p>
                <p>برای فعال سازی حساب کاربری روی لینک زیر کلیک کنید : </p>
                <h3 style="text-align: center;">
                <a href="{url}">لینک فعال سازی حساب کاربری</a>
                </h3>
                <hr />
                <p>اگر شما این کد را درخواست نکرده‌اید میتوانید این ایمیل را نادیده بگیرید.</p>
            </div>
        </div>
     """;
}