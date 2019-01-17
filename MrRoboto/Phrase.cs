using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MrRoboto
{
    public class Phrase
    {
        public string Kana { get; set; }
        public string Ego { get; set; }
        public override bool Equals(object obj)
        {
            if (!(obj is Phrase))
            {
                return base.Equals(obj);
            }
            var ph = obj as Phrase;
            return Kana == ph.Kana && Ego == ph.Ego;
        }

        public override int GetHashCode()
        {
            return 5 * Kana.GetHashCode() + 7 * Ego.GetHashCode();
        }
    }
}
