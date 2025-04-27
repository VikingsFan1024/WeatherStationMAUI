public class VW_Message<T>(T model) :
    CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<T>(model)
{
    private readonly T _model = model;
    public T Model => _model;
}