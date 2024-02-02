public interface IGenericInitializable<T>
{
    bool TryInitialize(T data);
}
