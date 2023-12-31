namespace Exceptions
{
    public class MissingConfigurationException : Exception
    {
        public MissingConfigurationException(string configurationKey) :
            base($"The required configuration value '{configurationKey}' is missing.")
        { }
    }
}
