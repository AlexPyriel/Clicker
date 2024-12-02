using Screens.Application.Presenters;
using Screens.Application.Views;
using Zenject;

namespace Screens
{
    public class ApplicationInstaller : Installer<ApplicationInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ApplicationView>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            Container
                .BindInterfacesTo<ApplicationPresenter>()
                .AsSingle();
        }
    }
}