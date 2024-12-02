using Screens;
using Zenject;

namespace Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ApplicationInstaller.Install(Container);
        }
    }
}