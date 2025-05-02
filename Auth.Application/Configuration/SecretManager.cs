using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Auth.Service.Configuration;

[ExcludeFromCodeCoverage]
public class SecretManager
{
    public IpRateLimiting IpRateLimiting { get; set; }
    public AuthenticatedRateLimit AuthenticatedRateLimit { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public Logging Logging { get; set; }
    public TokenConfigurations TokenConfigurations { get; set; }
    public Google Google { get; set; }
    public string AllowedHosts { get; set; }
}

[ExcludeFromCodeCoverage]
public class IpRateLimiting
{
    public bool EnableEndpointRateLimiting { get; set; }
    public bool StackBlockedRequests { get; set; }
    public string RealIpHeader { get; set; }
    public string ClientIdHeader { get; set; }
    public List<GeneralRule> GeneralRules { get; set; }
}

[ExcludeFromCodeCoverage]
public class GeneralRule
{
    public string Endpoint { get; set; }
    public string Period { get; set; }
    public int Limit { get; set; }
}

[ExcludeFromCodeCoverage]
public class AuthenticatedRateLimit
{
    public string Period { get; set; }
    public int Limit { get; set; }
}

[ExcludeFromCodeCoverage]
public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
    public string RedisConnection { get; set; }
    public string RabbitConnection { get; set; }
}

[ExcludeFromCodeCoverage]
public class Logging
{
    public LogLevel LogLevel { get; set; }
}

[ExcludeFromCodeCoverage]
public class LogLevel
{
    public string Default { get; set; }
    public string Microsoft { get; set; }
    public string MicrosoftHostingLifetime { get; set; }
}

[ExcludeFromCodeCoverage]
public class TokenConfigurations
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Secret { get; set; }
    public int Minutes { get; set; }
    public int DaysToExpiry { get; set; }
}

[ExcludeFromCodeCoverage]
public class Google
{
    public string BaseUrl { get; set; }
    public string Key { get; set; }
}
