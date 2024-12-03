using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Core.Network
{
    public interface IServerClient
    {
        public UniTask<(UnityWebRequest.Result Result, T ResultData)> SendGetRequestAsync<T>(string url,
            CancellationToken cancellationToken);
    }
}