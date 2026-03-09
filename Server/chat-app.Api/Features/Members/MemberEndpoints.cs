using Microsoft.AspNetCore.Mvc;

namespace chat_app.Api.Features.Members
{
    [Route("/members")]
    public static class MemberEndpoints
    {
        public static void MapMemberEndpoints(this WebApplication app)
        {
            RouteGroupBuilder memberGroup = app.MapGroup("/members");
        }
    }
}
