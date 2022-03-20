using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class EnumExtension
    {
        public static TEnum ToEnum<TEnum>(this int val)
        {
            return (TEnum)System.Enum.ToObject(typeof(TEnum), val);
        }
    }

    public enum EmployeeType
    {
        Admin = 1,
        Developer = 2,
        Projectmanager = 3,
        SQA = 4
    }
}
