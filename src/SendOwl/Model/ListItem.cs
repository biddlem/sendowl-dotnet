using Newtonsoft.Json;

namespace SendOwl.Model
{
    public interface IListItem<T>
    {
        T Value { get; set; }
    }
}
