#if ZENJECT
using UMediator.Zenject;
using Zenject;

namespace Sandbox._1._Simple_Request
{
    public class MediatrInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.AddUMediator(typeof(Main_SimpleRequest));
        }
    }
}
#endif