using Cysharp.Threading.Tasks;

namespace Core.Network
{
    public interface IServerCommand
    {
        UniTask Execute();
        void Cancel();
    }
}