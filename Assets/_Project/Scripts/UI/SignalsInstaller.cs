using UnityEngine;
using Zenject;

namespace RollicCase.UI
{
    /// <summary>Installs the signal bus that buttons, presenters, and flows talk through; each scene declares its own signals.</summary>
    [CreateAssetMenu(fileName = "SO_SignalsInstaller", menuName = "RollicCase/Installers/Signals Installer")]
    public sealed class SignalsInstaller : ScriptableObjectInstaller<SignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}
