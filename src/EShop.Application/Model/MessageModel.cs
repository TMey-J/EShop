namespace EShop.Application.Model
{
    public record MessageModel<TEntity>(ActionTypes ActionTypes, TEntity Data);
    public enum ActionTypes
    {
        Create,
        Update,
        Delete
    }
}
