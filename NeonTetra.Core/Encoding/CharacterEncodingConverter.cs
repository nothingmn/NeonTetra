using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Encoding;

namespace NeonTetra.Core.Encoding
{
    public class CharacterEncodingConverter : ICharacterEncodingConverter
    {
        private readonly Dictionary<char, char> VisuallySimilarByteMapUtf8 = new Dictionary<char, char>
        {
            {'À', 'A'},
            {'Á', 'A'},
            {'Â', 'A'},
            {'Ã', 'A'},
            {'Ä', 'A'},
            {'Å', 'A'},
            {'Æ', 'A'},
            {'Ç', 'C'},
            {'È', 'E'},
            {'É', 'E'},
            {'Ê', 'E'},
            {'Ë', 'E'},
            {'Ì', 'I'},
            {'Í', 'I'},
            {'Î', 'I'},
            {'Ï', 'I'},
            {'Ð', 'D'},
            {'Ñ', 'N'},
            {'Ò', 'O'},
            {'Ó', 'O'},
            {'Ô', 'O'},
            {'Õ', 'O'},
            {'Ö', 'O'},
            {'×', 'x'},
            {'Ø', 'O'},
            {'Ù', 'U'},
            {'Ú', 'U'},
            {'Û', 'U'},
            {'Ü', 'U'},
            {'Ý', 'Y'},
            {'ß', 'B'},
            {'à', 'a'},
            {'á', 'a'},
            {'â', 'a'},
            {'ã', 'a'},
            {'ä', 'a'},
            {'å', 'a'},
            {'æ', 'a'},
            {'ç', 'c'},
            {'è', 'e'},
            {'é', 'e'},
            {'ê', 'e'},
            {'ë', 'e'},
            {'ì', 'i'},
            {'í', 'i'},
            {'î', 'i'},
            {'ï', 'i'},
            {'ð', 'd'},
            {'ñ', 'n'},
            {'ò', 'o'},
            {'ó', 'o'},
            {'ô', 'o'},
            {'õ', 'o'},
            {'ö', 'o'},
            {'÷', '/'},
            {'ø', '0'},
            {'ù', 'u'},
            {'ú', 'u'},
            {'û', 'u'},
            {'ü', 'u'},
            {'ý', 'y'},
            {'ÿ', 'y'}
        };

        public string Convert(string input, System.Text.Encoding source, System.Text.Encoding destination,
            bool replaceWithVisualEquivanets = false)
        {
            if (input == null) return null;
            if (source == null) source = System.Text.Encoding.UTF8;
            if (destination == null) destination = System.Text.Encoding.ASCII;
            if (source.Equals(destination)) return input;

            if (replaceWithVisualEquivanets && destination.Equals(System.Text.Encoding.ASCII))
                return ConvertToVisuallySimilarAscii(input, source);
            return destination.GetString(source.GetBytes(input));
        }

        private string ConvertToVisuallySimilarAscii(string input, System.Text.Encoding source)
        {
            var sourceEncoded =
                source.GetString(source.GetBytes(input)); //make sure it is encoded correctly with the source encoding

            var result = new StringBuilder(input.Length);

            foreach (var c in sourceEncoded)
                if (VisuallySimilarByteMapUtf8.ContainsKey(c))
                    result.Append(VisuallySimilarByteMapUtf8[c]);
                else
                    result.Append(c);

            return System.Text.Encoding.ASCII.GetString(
                System.Text.Encoding.ASCII.GetBytes(result.ToString())); //return it, finallly encoded as ascii
        }
    }
}