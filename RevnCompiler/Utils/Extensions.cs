using System;
using System.Collections.Generic;
using System.Linq;

using RevnCompiler.ASTs;

namespace RevnCompiler.Utils
{
    public static class Extensions
    {
        public static bool IsIn<T>(this T o, IEnumerable<T> e)
        {
            return e.Contains(o);
        }

        public static Accessibility ConvertToAccessibility(this string s)
        {
			switch (s)
			{
				case "private": return Accessibility.Private;
				case "public": return Accessibility.Public;
				case "protected": return Accessibility.Protected;
				case "internal": return Accessibility.Internal;
				default: throw new NotImplementedException();
			}
        }
    }
}
