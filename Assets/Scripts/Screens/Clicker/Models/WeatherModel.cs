using System;
using UniRx;

namespace Screens.Clicker.Models
{
    public class WeatherModel
    {
        public ReactiveProperty<string> Name { get; } = new();
        public ReactiveProperty<int> Temperature { get; } = new();
        public ReactiveProperty<string> TemperatureUnit { get; } = new();
        public IObservable<Unit> PropertyChanged { get; }

        public WeatherModel()
        {
            PropertyChanged = Observable.Merge(
                Name.AsObservable().Select(_ => Unit.Default),
                Temperature.AsObservable().Select(_ => Unit.Default),
                TemperatureUnit.AsObservable().Select(_ => Unit.Default)
            );
        }
        
        public void Update(string name, int temperature, string temperatureUnit)
        {
            Name.Value = name;
            Temperature.Value = temperature;
            TemperatureUnit.Value = temperatureUnit;
        }
    }
}