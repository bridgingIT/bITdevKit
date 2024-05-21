namespace BridgingIT.DevKit.Presentation.Web;

using Microsoft.AspNetCore.Routing;

public abstract class ApiEndpointsBase : IApiEndpoints
{
    public bool Enabled { get; set; } = true;

    public bool IsRegistered { get; set; }

    public abstract void Map(IEndpointRouteBuilder app);
}