using Screens.Facts.Models;
using Screens.Facts.Presenters;
using Screens.Facts.Views;
using Zenject;

namespace Screens.Facts.Installers
{
    public class FactsInstaller : Installer<FactsInstaller>
    {
        public override void InstallBindings()
        {

            Container
                .Bind<FactsModel>()
                .AsSingle();
            
            Container
                .Bind<FactsView>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            Container
                .BindInterfacesTo<FactsPresenter>()
                .AsSingle();
        }
    }
}