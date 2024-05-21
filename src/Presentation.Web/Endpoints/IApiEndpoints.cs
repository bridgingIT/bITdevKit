namespace BridgingIT.DevKit.Presentation.Web;

using Microsoft.AspNetCore.Routing;

public interface IApiEndpoints
{
    bool Enabled { get; set; }

    bool IsRegistered { get; set; }

    void Map(IEndpointRouteBuilder app);
}