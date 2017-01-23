using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scripting_Engine
{
    public static class SlowUsing
    {
        public static String SortUsing(String code)
        {
            List<String> usingList = new List<String>();
            var usingIndex = -1;
            var colonIndex = -1;
            while((usingIndex = code.IndexOf("using ")) != -1)
            {
                if (code.Length < usingIndex + 6) break;
                if ((colonIndex = code.IndexOf(";", usingIndex)) == -1) break;

                String usingString = code.Substring(usingIndex, colonIndex - usingIndex + 1);
                usingList.Add(usingString);
                code = code.Substring(0, usingIndex) + code.Substring(colonIndex+1);
            }

            //Remove using duplicates
            usingList = usingList.Distinct().ToList();
            return String.Join(Environment.NewLine, usingList) + code;
        }
    }
}
