namespace Api.Retrievers;

public abstract class Retriever<T>
{
    public abstract IEnumerable<T> RetrieveAll();
}
