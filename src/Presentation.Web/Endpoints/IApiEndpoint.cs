namespace BridgingIT.DevKit.Presentation.Web;

using Microsoft.AspNetCore.Routing;

public interface IApiEndpoint
{
    void MapApiEndpoints(IEndpointRouteBuilder app);
}