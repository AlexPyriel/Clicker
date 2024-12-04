using UniRx;

namespace Screens.Facts.Models
{
    public class FactModel
    {
        public ReactiveProperty<string> Id { get; } = new();
        public ReactiveProperty<string> Name { get; } = new();
        public ReactiveProperty<string> Description { get; } = new();

        public FactModel(string id, string name, string? description = null)
        {
            Id.Value = id;
            Name.Value = name;
            Description.Value = description;
        }
    }
}