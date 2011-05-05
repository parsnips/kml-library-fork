namespace Core.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    public static class StringUtils
    {
        internal const char WILDCARD_STRING = '*';
        internal const char WILDCARD_CHAR = '?';

        public static string[] Split(string str, string delim)
        {
            if (str == null)
            {
                return null;
            }

            string[] delims = {delim};
            return str.Split(delims, StringSplitOptions.None);
        }

        public static List<string> SplitToList(string str, string delim)
        {
            var bits = Split(str, delim);
            return new List<string>(bits);
        }

        public static List<string> SplitToList(string str, string delim, bool trim)
        {
            if (!trim)
            {
                return SplitToList(str, delim);
            }

            var bits = Split(str, delim);
            var ans = new List<string>();
            foreach (var bit in bits)
            {
                ans.Add(bit.Trim());
            }

            return ans;
        }

        public static bool IsNumeric(object Expression)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, 
                                    out retNum);
            return isNum;
        }

        public static bool IsAlphaNumeric(char x)
        {
            var an = new Regex(@"\w");
            return an.Match(x.ToString()).Success;
        }

        public static bool IsAlphaNumeric(string x)
        {
            foreach (var c in x)
            {
                if (!IsAlphaNumeric(c))
                {
                    return false;
                }
            }

            return true;
        }

        public static string SubstringAlphaNumericAtPosition(string str, int pos)
        {
            if (pos < 0 || pos > str.Length - 1)
            {
                return string.Empty;
            }

            var chars = str.ToCharArray();
            var last = pos - 1;
            for (var i = pos; i < chars.Length; i++)
            {
                if (IsAlphaNumeric(chars[i]))
                {
                    last++;
                }
                else
                {
                    break;
                }
            }

            if (last >= pos)
            {
                return str.Substring(pos, last - pos + 1);
            }

            return string.Empty;
        }

        public static string SubstringBetween(string str, string start, string end)
        {
            var startPos = str.IndexOf(start);
            if (startPos < 0)
            {
                return string.Empty;
            }

            startPos++;
            var endPos = str.IndexOf(end);
            if (endPos < startPos)
            {
                return str.Substring(startPos);
            }

            endPos--;
            return str.Substring(startPos, endPos - startPos + 1);
        }

        /// <summary>
        ///   Get phrase in present tense: e.g. there "are no boxes" selected; there "is one box" selected; there "are 5 boxes" selected
        /// </summary>
        public static string GetSmartPlural(int num, string noun)
        {
            return GetSmartPlural(num, noun, "s");
        }

        public static string GetSmartPlural(int num, string noun, string addToMakePlural)
        {
            // --e.g. there are no polylines selected...
            switch (num)
            {
                case 0:
                    return "are no " + noun + addToMakePlural;
                case 1:
                    return "is one " + noun;
                default:
                    return "are " + num + " " + noun + addToMakePlural;
            }
        }

        /// <summary>
        ///   Get phrase in past tense: e.g. "no boxes were" selected; "one box was" selected; "5 boxes were" selected
        /// </summary>
        public static string GetSmartPluralPastTense(int num, string noun)
        {
            return GetSmartPluralPastTense(num, noun, "s");
        }

        public static string GetSmartPluralPastTense(int num, string noun, string addToMakePlural)
        {
            switch (num)
            {
                case 0:
                    return "no " + noun + addToMakePlural + " were";
                case 1:
                    return "one " + noun + " was";
                default:
                    return num + " " + noun + addToMakePlural + " were";
            }
        }

        public static string RenameFolder(string oldDirPath, string proposedName)
        {
            return oldDirPath.Substring(0, oldDirPath.LastIndexOf(Path.DirectorySeparatorChar)) +
                   Path.DirectorySeparatorChar + proposedName;
        }

        public static string RemoveFromFront(string str, string frontStr)
        {
            if (!str.StartsWith(frontStr))
            {
                throw new ArgumentException("Error in RemoveFromFront: " + str + " does not start with " + frontStr);
            }

            return str.Substring(frontStr.Length);
        }

        public static string RemoveExcessWhiteSpace(string str)
        {
            var ans = str;
            while (ans.Contains("  "))
            {
                ans = ans.Replace("  ", " ");
            }

            return ans;
        }

        /// <summary>
        ///   Comparison delegate for strings that contain letters and numbers. Improves upon standard string comparisons where "A11" comes before "A2". Usage: Array.Sort(list, StringUtils.CompareAlphaNumeric);
        /// </summary>
        public static int CompareAlphaNumeric(string a, string b)
        {
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                return 0;
            }

            var ans = CompareAlphaNumericNums(a, b);
            if (ans == 0)
            {
                return a.CompareTo(b);
            }

            return ans;
        }

        private static int CompareAlphaNumericNums(string a, string b)
        {
            if (String.IsNullOrEmpty(a))
            {
                return -1;
            }

            if (String.IsNullOrEmpty(b))
            {
                return 1;
            }

            if (a.Length != b.Length)
            {
                // --e.g. 11 vs 2
                int testA, testB;
                if (int.TryParse(a, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out testA))
                {
                    if (int.TryParse(b, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out testB))
                    {
                        return testA.CompareTo(testB);
                    }
                }

                var numStrtA = FindNumericStart(a);
                var numStrtB = FindNumericStart(b);

// --e.g. 2nd floor vs 10th floor
                if (numStrtA == 0 && numStrtB == 0)
                {
                    var numA = ExtractNumber(a);
                    var numB = ExtractNumber(b);
                    if (numA > 0 && numB > 0)
                    {
                        return numA.CompareTo(numB);
                    }
                }

// --e.g. New.11 vs New.2                
                if (numStrtA > 0 && numStrtB == numStrtA)
                {
                    if (a.Substring(0, numStrtA) == b.Substring(0, numStrtA))
                    {
                        var postA = a.Substring(numStrtA);
                        var postB = b.Substring(numStrtA);
                        var numA = ExtractNumber(postA);
                        var numB = ExtractNumber(postB);
                        if (numA != -1 && numB != -1)
                        {
                            var ans = numA.CompareTo(numB);
                            if (ans != 0)
                            {
                                return ans;
                            }

// --e.g. New.11A vs New.11A.B
                            return postA.CompareTo(postB);
                        }
                    }
                }
            }

            int tstA, tstB; // --- only necessary if number formats are mixed -- e.g. 30,000 vs 100000
            if (int.TryParse(a, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out tstA))
            {
                if (int.TryParse(b, NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out tstB))
                {
                    return tstA.CompareTo(tstB);
                }
            }

            return a.CompareTo(b);
        }

        public static int FindNumericStart(string numStr)
        {
            int test;
            var numStrt = -1;
            for (var i = 0; i < numStr.Length; i++)
            {
                if (int.TryParse(numStr.Substring(i, 1), out test))
                {
                    numStrt = i;
                    break;
                }
            }

            return numStrt;
        }

        public static int FindNumericEnd(string numStr)
        {
            return FindNumericEnd(FindNumericStart(numStr), numStr);
        }

        public static int FindNumericEnd(int start, string numStr)
        {
            int test;
            var numEnd = -1;
            for (var i = start; i < numStr.Length; i++)
            {
                if (!int.TryParse(numStr.Substring(i, 1), out test))
                {
                    numEnd = i;
                    break;
                }
            }

            return numEnd;
        }

        public static int ExtractNumber(string numStr)
        {
            var ans = -1;
            if (int.TryParse(numStr, out ans))
            {
                return ans;
            }

            int test;
            var numStrt = FindNumericStart(numStr);
            if (numStrt == -1)
            {
                return -1;
            }

            var concat = string.Empty;
            for (var i = numStrt; i < numStr.Length; i++)
            {
                concat += numStr[i];
                if (!int.TryParse(concat, out test))
                {
                    return ans;
                }

                ans = test;
            }

            return ans;
        }

        public static string Join(IEnumerable list, string delim)
        {
            var ans = string.Empty;
            foreach (var o in list)
            {
                ans += Convert.ToString(o) + delim;
            }

            ans = ans.Substring(0, ans.Length - delim.Length);
            return ans;
        }

        public static string AppendIfExists(string div, string val)
        {
            if (String.IsNullOrEmpty(val))
            {
                return string.Empty;
            }

            return div + val;
        }

        public static bool WildcardEquals(string pattern, string str)
        {
            return WildcardEquals(pattern, str, false);
        }

        public static bool WildcardEquals(string pattern, string str, bool ignoreCase)
        {
            if (ignoreCase)
            {
                pattern = pattern.ToLower();
                str = str.ToLower();
            }

            if (pattern == null || str == null)
            {
                return false;
            }
            else if (pattern.Length == 1 && pattern[0] == WILDCARD_STRING)
            {
                return true;
            }
            else if (str == String.Empty && pattern != String.Empty)
            {
                return false;
            }
            else if (pattern.IndexOf(WILDCARD_STRING) == -1 && pattern.IndexOf(WILDCARD_CHAR) == -1)
            {
                return pattern == str;
            }
            else
            {
                return WildcardEquals(pattern, 0, str, 0);
            }
        }

        // Copied from beagled/Lucene.Net/Search/WildcardTermEnum.cs
        internal static bool WildcardEquals(string pattern, int patternIdx, string string_Renamed, int stringIdx)
        {
            var p = patternIdx;

            for (var s = stringIdx;; ++p, ++s)
            {
                // End of string yet?
                var sEnd = s >= string_Renamed.Length;

// End of pattern yet?
                var pEnd = p >= pattern.Length;

                // If we're looking at the end of the string...
                if (sEnd)
                {
                    // Assume the only thing left on the pattern is/are wildcards
                    var justWildcardsLeft = true;

                    // Current wildcard position
                    var wildcardSearchPos = p;

// While we haven't found the end of the pattern,
                    // and haven't encountered any non-wildcard characters
                    while (wildcardSearchPos < pattern.Length && justWildcardsLeft)
                    {
                        // Check the character at the current position
                        var wildchar = pattern[wildcardSearchPos];

                        // If it's not a wildcard character, then there is more
                        // pattern information after this/these wildcards.
                        if (wildchar != WILDCARD_CHAR && wildchar != WILDCARD_STRING)
                        {
                            justWildcardsLeft = false;
                        }
                        else
                        {
                            // to prevent "cat" matches "ca??"
                            if (wildchar == WILDCARD_CHAR)
                            {
                                return false;
                            }

                            // Look at the next character
                            wildcardSearchPos++;
                        }
                    }

                    // This was a prefix wildcard search, and we've matched, so
                    // return true.
                    if (justWildcardsLeft)
                    {
                        return true;
                    }
                }

                // If we've gone past the end of the string, or the pattern,
                // return false.
                if (sEnd || pEnd)
                {
                    break;
                }

                // Match a single character, so continue.
                if (pattern[p] == WILDCARD_CHAR)
                {
                    continue;
                }

                if (pattern[p] == WILDCARD_STRING)
                {
                    // Look at the character beyond the '*'.
                    ++p;

// Examine the string, starting at the last character.
                    for (var i = string_Renamed.Length; i >= s; --i)
                    {
                        if (WildcardEquals(pattern, p, string_Renamed, i))
                        {
                            return true;
                        }
                    }

                    break;
                }

                if (pattern[p] != string_Renamed[s])
                {
                    break;
                }
            }

            return false;
        }
    }
}