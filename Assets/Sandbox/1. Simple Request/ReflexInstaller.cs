#if REFLEX
// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 14.11.2025
// Description:
// -------------------------------------------------------------------

using Reflex.Core;
using UMediator.Reflex;
using UnityEngine;
using IInstaller = Reflex.Core.IInstaller;

namespace Sandbox._1._Simple_Request
{
    public class ReflexInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddUMediator(typeof(ReflexInstaller));
        }
    }
}
#endif