using Infrastructure;
using Screens.Clicker.Presenters;
using Screens.Clicker.Views;
using Zenject;

namespace Screens.Clicker.Installers
{
    public class ClickerInstaller : Installer<ClickerInstaller>
    {
        private const string ConfigResourcesPath = "Configs/ClickerConfig";

        public override void InstallBindings()
        {
            Container
                .Bind<ClickerConfig>()
                .FromResource(ConfigResourcesPath)
                .AsSingle();
            
            Container
                .Bind<ClickerView>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            Container
                .BindInterfacesTo<ClickerPresenter>()
                .AsSingle();
        }
    }
}