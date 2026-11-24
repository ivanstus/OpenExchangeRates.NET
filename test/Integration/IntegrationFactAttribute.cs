using System.Runtime.CompilerServices;

namespace OpenExchangeRates.Tests.Integration;

public sealed class IntegrationFactAttribute : FactAttribute
{
    public IntegrationFactAttribute([CallerFilePath] string? sourceFilePath = null,
        [CallerLineNumber] int sourceLineNumber = -1) : base(sourceFilePath, sourceLineNumber)
    {
        if (IntegrationTestConfiguration.GetAppId() == null)
            Skip = $"Set the {IntegrationTestConfiguration.AppIdKey} environment variable to run integration tests.";
    }
}