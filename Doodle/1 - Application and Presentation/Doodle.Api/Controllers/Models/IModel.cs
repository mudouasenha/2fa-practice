namespace Doodle.Api.Controllers.Models
{
    // TODO: Fix it.
    public interface IModel<TModel> : TInput
    {
        public TInput ToInput<TModel>(TModel model) where TModel : TInput;
    }

    public interface TInput
    { }
}