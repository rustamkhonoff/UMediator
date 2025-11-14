#if REFLEX
// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 14.11.2025
// Description:
// -------------------------------------------------------------------
using Reflex.Core;

namespace UMediator.Reflex
{
    public class ServiceProvider : IMediatorServiceProvider
    {
        public T GetService<T>()
        {
            return Container.ProjectContainer.Resolve<T>();
        }
    }
}
#endif