using chat_app.Api.Features.Members;

namespace chat_app.Api.Features.Auth.Interfaces;

public interface ITokenService
{
    string GenerateToken(Member member);
}

