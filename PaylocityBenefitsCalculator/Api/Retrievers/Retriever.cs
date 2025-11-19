namespace Api.Retrievers;

/// <summary>
/// Retrieve items from the database.
/// </summary>
public abstract class Retriever<T>
{
    public abstract IEnumerable<T> RetrieveAll();
}
