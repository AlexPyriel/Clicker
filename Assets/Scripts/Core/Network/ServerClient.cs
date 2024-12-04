using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Network
{
    public class ServerClient : IServerClient
    {
        public async UniTask<(UnityWebRequest.Result Result, T ResultData)> SendGetRequestAsync<T>(string url, CancellationToken cancellationToken)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            UnityWebRequest serverResponse = await webRequest.SendWebRequest().WithCancellation(cancellationToken);

            UnityWebRequest.Result result = serverResponse.result;
            T resultData = default(T);

            if (result == UnityWebRequest.Result.Success)
            {
                string json = serverResponse.downloadHandler.text;
                
                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        resultData = JsonUtility.FromJson<T>(json);
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.LogError($"JSON deserialization error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"An unexpected error occurred: {ex.Message}");
                    }
                }
                else
                {
                    Debug.LogError("Received empty JSON");
                }
            }
            
            return (result, resultData);
        }
    }
}