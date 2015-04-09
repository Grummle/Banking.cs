using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.cs
{
    public static class Extensions
    {
        public static string OfxFormat(this DateTime val)
        {
            return val.ToString("yyyyMMddHHmmss");
        }

        public static string OfxFormat(this Guid val)
        {
            return val.ToString("N");
        }
    }
}
