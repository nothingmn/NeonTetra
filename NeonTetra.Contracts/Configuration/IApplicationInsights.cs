namespace NeonTetra.Contracts.Configuration
{
    public interface IApplicationInsights
    {
        string Key { get; set; }
        bool Enabled { get; set; }
    }
}