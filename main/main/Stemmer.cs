using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InfoRet
{/// <summary>
/// based of porter2 the english stemmer from http://snowball.tartarus.org/algorithms/english/stemmer.html
/// </summary>
    public static class Stemmer
    {

        static private char[] word;
        static private int len;
        static int R1;
        static int R2;
        static private char[] wordR1;
        static private char[] wordR2;
        static Dictionary<string, string> dict;
        static private bool isConst(int pos)
        {
            char letter = word[pos];
            if (letter == 'a' || letter == 'e' || letter == 'i' || letter == 'o' || letter == 'u' || letter == 'y')
                return false;
            return true;
        }

        static private bool isShortSy(int pos)
        {
            if (pos + 1 > word.Length - 1)
                return false;///////////////
            if (!isConst(pos) && pos > 0 && isConst(pos - 1) && len > 3
                && (isConst(pos + 1) && (word[pos + 1] != 'x' || word[pos + 1] != 'w' || word[pos + 1] != 'Y')))
                return true;
            else if (pos == 0 && len > 1 && isConst(pos + 1))
                return true;
            return false;
        }

        static private bool isShortWord()
        {
            return (isShortSy(firstVowel()) && R1 >= len);

        }
        static private int firstVowel()
        {
            for (int i = 0; i < len; i++)
            {
                if (!isConst(i))
                    return i;
            }

            return len - 1;
        }

        static private int findR1()
        {
            for (int i = 0; i < len; i++)
            {
                if (!isConst(i))
                {
                    while (i < len && !isConst(i))
                    {
                        i++;
                    }
                    return len > i + 1 ? i + 1 : len;
                }
            }
            return len;
        }
        static private int findR2()
        {
            for (int i = R1; i < len; i++)
            {
                if (!isConst(i))
                {
                    while (i < len && !isConst(i))
                    {
                        i++;
                    }
                    return len > i + 1 ? i + 1 : len;
                }
            }
            return len;
        }

        static private void measure()
        {
            R1 = findR1();
            R2 = findR2();
            if (R1 < len)
                wordR1 = word.SubArray(R1, word.Length - R1);
            else
                wordR1 = null;
            if (R2 < len)
                wordR2 = word.SubArray(R2, word.Length - R2);
            else
                wordR2 = null;
        }
        static private void astSetup()
        {
            if (word[0] == 'y')
                word[0] = 'Y';

            for (int i = 1; i < len; i++)
            {
                if (word[i] == 'y' && !isConst(i - 1))
                    word[i] = 'Y';
            }
        }

        static void step0()
        {
            if (word[len - 1] == '\'' && word[len - 2] == 's' && word[len - 3] == '\'')
                setupWord(word.SubArray(0, len - 3));
            else if (word[len - 1] == 's' && word[len - 2] == '\'')
                setupWord(word.SubArray(0, len - 2));
            else if (word[len - 1] == '\'')
                setupWord(word.SubArray(0, len - 1));
            return;
        }

        static void step1a()
        {
            string temp = new string(word);
            if (temp.EndsWith("sses"))
            {
                setupWord(word.SubArray(0, len - 2));
            }
            else if (temp.EndsWith("ies") || temp.EndsWith("ied"))
            {
                if (len > 4)
                    setupWord(word.SubArray(0, len - 2));
                else
                    setupWord(word.SubArray(0, len - 1));
            }
            else if (len > 2 && temp.EndsWith("s"))
            {
                if (!(firstVowel() == len - 2))
                    setupWord(word.SubArray(0, len - 1));
            }
        }

        static void step1b()
        {
            string temp = new string(word);
            string temp2 = new string(wordR1);
            bool extra = false;
            if (temp.EndsWith("eed") && temp2.Contains("eed"))
                setupWord(word.SubArray(0, len - 1));
            else if (temp.EndsWith("eedly") && temp2.Contains("eed"))
                setupWord(word.SubArray(0, len - 3));

            else if (temp.EndsWith("ed") && !(firstVowel() == len - 2))
            {
                setupWord(word.SubArray(0, len - 2));
                extra = true;
            }
            else if (temp.EndsWith("edly") && !(firstVowel() == len - 4))
            {
                setupWord(word.SubArray(0, len - 4));
                extra = true;
            }
            else if (temp.EndsWith("ing") && !(firstVowel() == len - 3))
            {
                setupWord(word.SubArray(0, len - 3));
                extra = true;
            }
            else if (temp.EndsWith("ingly") && !(firstVowel() == len - 5))
            {
                setupWord(word.SubArray(0, len - 5));
                extra = true;
            }
            if (isConst(len - 1) && extra)
            {
                if (word[len - 1] == word[len - 2] && (word[len - 1] == 'b' || word[len - 1] == 'd' || word[len - 1] == 'f' ||
                    word[len - 1] == 'g' || word[len - 1] == 'm' || word[len - 1] == 'n' || word[len - 1] == 'p' ||
                    word[len - 1] == 'r' || word[len - 1] == 't'))
                {
                    setupWord(word.SubArray(0, len - 1));
                }
            }

            if (isShortWord() && extra)
            {
                temp = new string(word);
                temp = temp + "e";
                setupWord(temp.ToCharArray());
            }

            if (extra)
            {
                temp = new string(word);
                if (temp.EndsWith("at") || temp.EndsWith("bl") || temp.EndsWith("iz"))
                {
                    temp = temp + "e";
                    setupWord(temp.ToCharArray());
                }
            }
        }

        static private void step1c()
        {

            if ((word[len - 1] == 'y' || word[len - 1] == 'Y') && (len - 2 != 0) && isConst(len - 2))
            {
                word[len - 1] = 'i';
            }
        }

        static private void step2()
        {
            string temp = new string(word);
            string tempR1 = new string(wordR1);

            if (temp.EndsWith("tional") && tempR1.Contains("tional"))
                setupWord(word.SubArray(0, len - 2));
            else if (temp.EndsWith("enci") && tempR1.Contains("enci"))
                word[len - 1] = 'e';
            else if (temp.EndsWith("anci") && tempR1.Contains("anci"))
                word[len - 1] = 'e';
            else if (temp.EndsWith("abli") && tempR1.Contains("abli"))
                word[len - 1] = 'e';
            else if (temp.EndsWith("abli") && tempR1.Contains("abli"))
                word[len - 1] = 'e';
            else if (temp.EndsWith("entli") && tempR1.Contains("entli"))
                setupWord(word.SubArray(0, len - 2));
            else if (temp.EndsWith("izer") && tempR1.Contains("izer"))
                setupWord(word.SubArray(0, len - 1));
            else if (temp.EndsWith("ization") && tempR1.Contains("ization"))
            {
                setupWord(word.SubArray(0, len - 4));
                word[len - 1] = 'e';
            }
            else if (temp.EndsWith("ational") && tempR1.Contains("ational"))
            {
                setupWord(word.SubArray(0, len - 4));
                word[len - 1] = 'e';
            }
            else if (temp.EndsWith("ation") && tempR1.Contains("ation"))
            {
                setupWord(word.SubArray(0, len - 2));
                word[len - 1] = 'e';
            }
            else if (temp.EndsWith("actor") && tempR1.Contains("actor"))
            {
                setupWord(word.SubArray(0, len - 1));
                word[len - 1] = 'e';
            }
            else if (temp.EndsWith("alism") && tempR1.Contains("alism"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("aliti") && tempR1.Contains("aliti"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("alli") && tempR1.Contains("alli"))
                setupWord(word.SubArray(0, len - 2));
            else if (temp.EndsWith("fulness") && tempR1.Contains("fulness"))
                setupWord(word.SubArray(0, len - 4));
            else if (temp.EndsWith("ousli") && tempR1.Contains("ousli"))
                setupWord(word.SubArray(0, len - 2));
            else if (temp.EndsWith("ousness") && tempR1.Contains("ousness"))
                setupWord(word.SubArray(0, len - 4));
            else if (temp.EndsWith("iveness") && tempR1.Contains("iveness"))
                setupWord(word.SubArray(0, len - 4));
            else if (temp.EndsWith("iviti") && tempR1.Contains("iviti"))
            {
                setupWord(word.SubArray(0, len - 2));
                word[len - 1] = 'e';
            }
            else if (temp.EndsWith("biliti") && tempR1.Contains("biliti"))
            {
                setupWord(word.SubArray(0, len - 3));
                word[len - 1] = 'e';
                word[len - 2] = 'l';
            }
            else if (temp.EndsWith("bli") && tempR1.Contains("bli"))
                word[len - 1] = 'e';
            else if (temp.EndsWith("ogi") && tempR1.Contains("ogi") && temp.EndsWith("logi"))
                setupWord(word.SubArray(0, len - 1));
            else if (temp.EndsWith("fulli") && tempR1.Contains("fulli"))
                setupWord(word.SubArray(0, len - 2));
            else if (temp.EndsWith("lessli") && tempR1.Contains("lessli"))
                setupWord(word.SubArray(0, len - 2));
            else if (tempR1.Contains("li") && (temp.EndsWith("lic") || temp.EndsWith("lid") || temp.EndsWith("lie") || temp.EndsWith("lig")
                || temp.EndsWith("lih") || temp.EndsWith("lik") || temp.EndsWith("lim") || temp.EndsWith("lin") || temp.EndsWith("lir")
                || temp.EndsWith("lit")))
            {
                setupWord(word.SubArray(0, len - 3));
            }


        }

        static private void step3()
        {
            string temp = new string(word);
            string tempR1 = new string(wordR1);
            string tempR2 = new string(wordR2);

            if (temp.EndsWith("ational") && tempR1.Contains("ational"))
            {
                setupWord(word.SubArray(0, len - 4));
                word[len - 1] = 'e';
            }
            else if (temp.EndsWith("alize") && tempR1.Contains("alize"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("icate") && tempR1.Contains("icate"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("iciti") && tempR1.Contains("iciti"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("ical") && tempR1.Contains("ical"))
                setupWord(word.SubArray(0, len - 2));
            else if (temp.EndsWith("ful") && tempR1.Contains("ful"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("ness") && tempR1.Contains("ness"))
                setupWord(word.SubArray(0, len - 4));
            else if (temp.EndsWith("ative") && tempR1.Contains("ative") && tempR2.Contains("ative"))
                setupWord(word.SubArray(0, len - 5));
        }

        static private void step4()
        {
            string temp = new string(word);
            string tempR2 = new string(wordR2);

            if (temp.EndsWith("al") && tempR2.Contains("al"))
                setupWord(word.SubArray(0, len - 2));
            else if (temp.EndsWith("ance") && tempR2.Contains("ance"))
                setupWord(word.SubArray(0, len - 4));
            else if (temp.EndsWith("ence") && tempR2.Contains("ence"))
                setupWord(word.SubArray(0, len - 4));
            else if (temp.EndsWith("er") && tempR2.Contains("er"))
                setupWord(word.SubArray(0, len - 2));
            else if (temp.EndsWith("ic") && tempR2.Contains("ic"))
                setupWord(word.SubArray(0, len - 2));
            else if (temp.EndsWith("able") && tempR2.Contains("able"))
                setupWord(word.SubArray(0, len - 4));
            else if (temp.EndsWith("ible") && tempR2.Contains("ible"))
                setupWord(word.SubArray(0, len - 4));
            else if (temp.EndsWith("ant") && tempR2.Contains("ant"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("ement") && tempR2.Contains("ement"))
                setupWord(word.SubArray(0, len - 5));
            else if (temp.EndsWith("ment") && tempR2.Contains("ment"))
                setupWord(word.SubArray(0, len - 4));
            else if (temp.EndsWith("ent") && tempR2.Contains("ent"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("ism") && tempR2.Contains("ism"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("ate") && tempR2.Contains("ate"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("iti") && tempR2.Contains("iti"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("ous") && tempR2.Contains("ous"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("ive") && tempR2.Contains("ive"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("ize") && tempR2.Contains("ize"))
                setupWord(word.SubArray(0, len - 3));
            else if (temp.EndsWith("ion") && tempR2.Contains("ion") && (word[len - 4] == 's' || word[len - 4] == 't'))
                setupWord(word.SubArray(0, len - 3));
        }

        static private void step5()
        {
            string temp = new string(word);
            string tempR1 = new string(wordR1);
            string tempR2 = new string(wordR2);

            if (temp.EndsWith("e") && tempR2.EndsWith("e"))
                setupWord(word.SubArray(0, len - 1));

            else if (temp.EndsWith("e") && tempR2.EndsWith("e"))
                setupWord(word.SubArray(0, len - 1));
            else if (temp.EndsWith("e") && tempR1.EndsWith("e") && (isShortSy(firstVowel()) && firstVowel() != len - 1))
                setupWord(word.SubArray(0, len - 1));

            else if (temp.EndsWith("l") && tempR2.EndsWith("l") && len >= 2 && word[len - 2] == 'l')
                setupWord(word.SubArray(0, len - 1));

        }
        static private void setupWord(char[] word)
        {
            Stemmer.word = word;
            len = word.Length;
        }

        static Stemmer()
        {   //special cases
            dict = new Dictionary<string, string>();
            dict.Add("sky", "sky");
            dict.Add("skies", "sky");
            dict.Add("skis", "ski");
            dict.Add("dying", "die");
            dict.Add("lying", "lie");
            dict.Add("news", "news");
            dict.Add("howe", "howe");
            dict.Add("bias", "bias");
            dict.Add("", "");
        }
        static public string stem(string str)
        {
            string ans = "";
            //Regex reg = new Regex("[^a-zA-Z0-9 -:]");
            //str = reg.Replace(str, "");

            char[] arr = str.Where(c => (char.IsLetterOrDigit(c) ||
                             c == '-' || c == ':' || c == '\'')).ToArray();

            str = new string(arr);

            if (Regex.IsMatch(str, "\\d") || str.Length < 3)
                return str;

            else if (dict.TryGetValue(str.ToLower(), out ans))
                return ans;
            else
            {
                setupWord(str.ToLower().ToCharArray());
                astSetup();
                R1 = 0;
                R2 = 0;
                measure();
                step0();
                measure();
                step1a();
                measure();
                step1b();
                measure();
                step1c();
                measure();
                step2();
                measure();
                step3();
                measure();
                step4();
                measure();
                step5();
                ans = new string(word);
                ans = ans.ToLower();
                return ans;
            }
        }

        /// <summary>
        /// from stack overflow for creating subarray extension by Marc Gravell
        /// </summary>

        private static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}


