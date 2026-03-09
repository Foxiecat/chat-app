namespace chat_app.Api.Features.Shared.Interfaces
{
    public interface IMapper<in TRequest, out TResponse, TEntity>
    {
        TEntity ToEntity(TRequest request);
        TResponse ToResponse(TEntity entity);
    }

    public interface IMapper<in TRequest, out TEntity>
    {
        TEntity ToEntity(TRequest request);
    }
}
