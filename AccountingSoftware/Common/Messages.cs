using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Common
{
    public class Messages
    {
        #region Common

        public static string NotSet = "ثبت نشده";

        #endregion

        #region User

        public static string LoginSuccessful = "خوش آمدید";
        public static string LoginFailed = "نام کاربری یا رمز عبور شما اشتباه است";

        public static string ChangePasswordSuccessful = "رمز عبور با موفقیت تغییر کرد";
        public static string ChangePasswordFailed = "رمز عبور با موفقیت تغییر نکرد";

        #endregion

        #region Account

        public static string CreateAccountSuccessful = "حساب جدید با موفقیت ثبت شد";
        public static string CreateAccountFailed = "حساب جدید با موفقیت ثبت نشد";

        public static string EditAccountSuccessful = "حساب مورد نظر با موفقیت ویرایش شد";
        public static string EditAccountFailed = "حساب مورد نظر با موفقیت ویرایش نشد";

        public static string DeleteAccountText = "آیا از حذف حساب مورد نظر اطمینان دارید؟";
        public static string DeleteAccountSuccessful = "حساب مورد نظر با موفقیت حذف شد";
        public static string DeleteAccountFailed = "حساب مورد نظر با موفقیت حذف نشد";

        #endregion

        #region Transaction

        public static string CreateTransactionSuccessful = "تراکنش جدید با موفقیت ثبت شد";
        public static string CreateTransactionFailed = "تراکنش جدید با موفقیت ثبت نشد";
        public static string CreateTransactionFailedAccountNotValid = "حساب مورد نظر یافت نشد";

        public static string EditTransactionSuccessful = "تراکنش مورد نظر با موفقیت ویرایش شد";
        public static string EditTransactionFailed = "تراکنش مورد نظر با موفقیت ویرایش نشد";

        public static string DeleteTransactionText = "آیا از حذف تراکنش مورد نظر اطمینان دارید؟";
        public static string DeleteTransactionSuccessful = "تراکنش مورد نظر با موفقیت حذف شد";
        public static string DeleteTransactionFailed = "تراکنش مورد نظر با موفقیت حذف نشد";

        public static string ChangeTransactionStateSuccessful = "تغییر وضعیت تراکنش مورد نظر با موفقیت انجام شد";
        public static string ChangeTransactionStateFailed = "تغییر وضعیت تراکنش مورد نظر با موفقیت انجام نشد";

        #endregion

        #region Calendar

        public static string CreateEventSuccessful = "رویداد جدید با موفقیت ثبت شد";
        public static string CreateEventFailed = "رویداد جدید با موفقیت ثبت نشد";

        public static string DeleteEventText = "آیا از حذف رویداد مورد نظر اطمینان دارید؟";
        public static string DeleteEventSuccessful = "رویداد مورد نظر با موفقیت حذف شد";
        public static string DeleteEventFailed = "رویداد مورد نظر با موفقیت حذف نشد";

        #endregion
    }
}
