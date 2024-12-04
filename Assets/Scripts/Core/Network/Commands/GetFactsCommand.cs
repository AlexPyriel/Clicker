using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Screens.Facts.Models;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Core.Network
{
    public class GetFactsCommand : IServerCommand
    {
        private const string Url = "https://dogapi.dog/api/v2/breeds";

        [Serializable]
        public class BreedData
        {
            public string id;
            public string type;
            public BreedAttributes attributes;
        }

        [Serializable]
        public class BreedAttributes
        {
            public string name;
        }

        [Serializable]
        public class BreedResponse
        {
            public List<BreedData> data;
        }
        
        [Inject] private readonly IServerClient _serverClient;
        [Inject] private readonly FactsModel _factsModel;

        private readonly CancellationTokenSource _cancellationTokenSource = new ();

        public async UniTask Execute()
        {
            try
            {
                var (result, breedResponse) = await _serverClient.SendGetRequestAsync<BreedResponse>(Url, _cancellationTokenSource.Token);

                if (result == UnityWebRequest.Result.Success)
                {
                    List<FactModel> breeds = new List<FactModel>();

                    foreach (var breed in breedResponse.data)
                    {
                        breeds.Add(new FactModel(breed.id, breed.attributes.name));
                    }
                    
                    _factsModel.Update(breeds);
                    
                    Debug.Log($"[GetFactsCommand] Executed. Facts Length:  {_factsModel.Breeds.Value.Count}");
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
    
    public class GetFactsCommandFactory : PlaceholderFactory<GetFactsCommand> {}
}