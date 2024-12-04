using System;
using UniRx;

namespace Screens.Facts.Models
{
    public class CurrentFactModel
    {
        public ReactiveProperty<string> Name { get; } = new();
        public ReactiveProperty<string> Description { get; } = new();
        public IObservable<Unit> PropertyChanged { get; }

        public CurrentFactModel()
        {
            PropertyChanged = Observable
                .CombineLatest(
                    Name.AsObservable(),
                    Description.AsObservable(),
                    (name, description) => Unit.Default
                ).
                Throttle(TimeSpan.FromMilliseconds(100));
        }
        
        public void Update(string name, string description)
        {
            Name.Value = name;
            Description.Value = description;
        }
    }
}