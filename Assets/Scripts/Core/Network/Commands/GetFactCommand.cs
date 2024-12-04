using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Screens.Facts.Models;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Core.Network
{
    public class GetFactCommand : IServerCommand
    {
        private const string FactEndPoint = "https://dogapi.dog/api/v2/breeds/{0}";
        private string _breedId;

        [Serializable]
        public class BreedData
        {
            public string id;
            public BreedAttributes attributes;
        }

        [Serializable]
        public class BreedAttributes
        {
            public string name;
            public string description;
        }

        [Serializable]
        public class BreedResponse
        {
            public BreedData data;
        }
        
        [Inject] private readonly IServerClient _serverClient;
        [Inject] private readonly CurrentFactModel _currentFactModel;

        private readonly CancellationTokenSource _cancellationTokenSource = new ();

        public GetFactCommand(string id)
        {
            _breedId = id;
        }

        public async UniTask Execute()
        {
            string url = string.Format(FactEndPoint, _breedId);
            
            try
            {
                var (result, breedResponse) = await _serverClient.SendGetRequestAsync<BreedResponse>(url, _cancellationTokenSource.Token);

                if (result == UnityWebRequest.Result.Success)
                {
                    var data = breedResponse.data.attributes;

                    _currentFactModel.Update(data.name, data.description);
                    
                    Debug.Log($"[GetFactCommand] Executed. Fact {data.name} description:  {data.description}");
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
            Debug.Log("[GetFactsCommand] Cancelled.");
        }
    }
    
    public class GetFactCommandFactory : PlaceholderFactory<string, GetFactCommand> {}
}