using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Screens.Clicker.Models;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Core.Network
{
    public class GetWeatherCommand : IServerCommand
    {
        private const string Url = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        [Serializable]
        public class WeatherData
        {
            public Properties properties;
        }

        [Serializable]
        public class Properties
        {
            public Period[] periods;
        }

        [Serializable]
        public class Period
        {
            public string name;
            public int temperature;
            public string temperatureUnit;
        }
        
        [Inject] private readonly IServerClient _serverClient;
        [Inject] private readonly WeatherModel _weatherModel;

        private readonly CancellationTokenSource _cancellationTokenSource = new ();

        public async UniTask Execute()
        {
            try
            {
                var (result, weatherData) = await _serverClient.SendGetRequestAsync<WeatherData>(Url, _cancellationTokenSource.Token);

                if (result == UnityWebRequest.Result.Success)
                {
                    Period forecast = weatherData.properties.periods[0];
                    
                    _weatherModel.Update(forecast.name, forecast.temperature, forecast.temperatureUnit);
                    Debug.Log($"[GetWeatherCommand] Executed. {forecast.name}: {forecast.temperature} {forecast.temperatureUnit}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error: {ex.Message}");
            }
        }

        public void Cancel()
        {
            _cancellationTokenSource?.Cancel();
            Debug.Log("[GetWeatherCommand] Cancelled.");
        }
    }
    
    public class GetWeatherCommandFactory : PlaceholderFactory<GetWeatherCommand> {}
}