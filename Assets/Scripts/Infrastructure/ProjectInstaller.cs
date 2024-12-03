using Screens;
using Screens.Clicker.Installers;
using Zenject;

namespace Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ApplicationInstaller.Install(Container);
            ClickerInstaller.Install(Container);
        }
    }
}