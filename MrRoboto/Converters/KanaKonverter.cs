using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MrRoboto.Converters
{
    class KanaKonverter : IValueConverter
    {
        static readonly Dictionary<string, string> dict = new Dictionary<string, string>
        {
            { "a", "あ"}, { "e", "え"}, { "u", "う"}, { "i", "い"}, { "o", "お"},
            { "ka", "か"},{ "ke", "け"},{ "ku", "く"},{ "ki", "き"},{ "ko", "こ"},
            { "sa", "さ"},{ "se", "せ"},{ "su", "す"},{ "shi", "し"},{ "so", "そ"},
            { "ta", "た"},{ "te", "て"},{ "tsu", "つ"},{ "chi", "ち"},{ "to", "と"},
            { "na", "な"},{ "ne", "ね"},{ "nu", "ぬ"},{ "ni", "に"},{ "no", "の"},
            { "ha", "は"},{ "he", "へ"},{ "hu", "ふ"},{ "hi", "ひ"},{ "ho", "ほ"},
            { "ma", "ま"},{ "me", "め"},{ "mu", "む"},{ "mi", "み"},{ "mo", "も"},
            { "ra", "ら"},{ "re", "れ"},{ "ru", "る"},{ "ri", "り"},{ "ro", "ろ"},
            
            { "ya", "や"},{ "yu", "ゆ"},{ "yo", "よ"},
            { "wa", "わ"},{ "wi", "ゐ"},{ "wo", "を"},{"we", "ゑ"},

            { "ga", "が"},{ "ge", "げ"},{ "gu", "ぐ"},{ "gi", "ぎ"},{ "go", "ご"},
            { "za", "ざ"},{ "ze", "ぜ"},{ "zu", "ず"},{ "zi", "じ"},{ "zo", "ぞ"},
            { "da", "だ"},{ "de", "で"},{ "dzu", "づ"},{ "dzi", "ぢ"},{ "do", "ど"},
            { "ba", "ば"},{ "be", "べ"},{ "bu", "ぶ"},{ "bi", "び"},{ "bo", "ぼ"},
            { "pa", "ぱ"},{ "pe", "ぺ"},{ "pu", "ぷ"},{ "pi", "ぴ"},{ "po", "ぽ"},
            { "n", "ん"},

            { "nya", "にや"},{ "nyu", "にゆ"},{ "nyo", "によ"},
            { "cha", "ちや"},{ "chu", "ちゆ"},{ "cho", "ちよ"},
            { "sha", "しや"},{ "shu", "しゆ"},{ "sho", "しよ"},
            { "kya", "きや"},{ "kyu", "きゆ"},{ "kyo", "きよ"},
            { "gya", "ぎや"},{ "gyu", "ぎゆ"},{ "gyo", "ぎよ"},
            { "rya", "りや"},{ "ryu", "りゆ"},{ "ryo", "りよ"},
            { "mya", "みや"},{ "myu", "みゆ"},{ "myo", "みよ"},
            { "hya", "ひや"},{ "hyu", "ひゆ"},{ "hyo", "ひよ"},
            { "pya", "ぴや"},{ "pyu", "ぴゆ"},{ "pyo", "ぴよ"},
            { "bya", "びや"},{ "byu", "びゆ"},{ "byo", "びよ"},
            { "dzya", "ぢや"},{ "dzyu", "ぢゆ"},{ "dzyo", "ぢよ"},
            { "jya", "じや"},{ "jyu", "じゆ"},{ "jyo", "じよ"}


        };
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var str = value as string;
            if (str == null) return "";
            var sb = new StringBuilder();
            var split = str.Split(' ');
            foreach (var romanji in split)
            {
                if (dict.ContainsKey(romanji))
                    sb.Append(dict[romanji]);
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
