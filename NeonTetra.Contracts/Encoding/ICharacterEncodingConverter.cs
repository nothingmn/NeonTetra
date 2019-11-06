namespace NeonTetra.Contracts.Encoding
{
    public interface ICharacterEncodingConverter
    {
        string Convert(string input, System.Text.Encoding source, System.Text.Encoding destination,
            bool replaceWithVisualEquivanets = false);
    }
}