using Core.Network;
using Screens;
using Screens.Clicker.Installers;
using Screens.Facts.Installers;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Install<ApplicationInstaller>();
            Install<ClickerInstaller>();
            Install<FactsInstaller>();
            Install<NetworkInstaller>();
        }
        
        private void Install<T>() where T : Installer<T>
        {
            Installer<T>.Install(Container);
            Debug.Log($"[PROJECT INSTALLER] Install: <b>{typeof(T).Name}</b>");
        }
    }
}