using Zenject;

namespace Core.Network
{
    public class NetworkInstaller : Installer<NetworkInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<ServerClient>()
                .AsSingle();
            
            Container
                .Bind<ServerRequestInvoker>()
                .AsSingle();
            
            Container
                .BindFactory<GetWeatherCommand, GetWeatherCommandFactory>()
                .AsTransient();
            
            Container
                .BindFactory<GetFactsListCommand, GetFactsListCommandFactory>()
                .AsTransient();
            
            Container
                .BindFactory<string, GetFactCommand, GetFactCommandFactory>()
                .AsTransient();
        }
    }
}