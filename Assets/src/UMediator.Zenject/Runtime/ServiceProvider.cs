#if ZENJECT

// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 14.11.2025
// Description:
// -------------------------------------------------------------------

using Zenject;

namespace UMediator.Zenject
{
    public class ServiceProvider : IMediatorServiceProvider
    {
        private readonly DiContainer m_container;

        public ServiceProvider(DiContainer container)
        {
            m_container = container;
        }

        public T GetService<T>()
        {
            return m_container.Resolve<T>();
        }
    }
}
#endif