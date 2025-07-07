#if ZENJECT
using System;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace UMediator.Implementation.Zenject
{
    internal sealed class ZenjectMediatorTypeFactory : IMediatorTypeFactory, IInitializable, IDisposable
    {
        private IInstantiator m_instantiator;

        public ZenjectMediatorTypeFactory(IInstantiator instantiator)
        {
            m_instantiator = instantiator;
        }

        public object CreateInstanceFor(Type type)
        {
            return m_instantiator.Instantiate(type);
        }

        public void Initialize()
        {
            SceneManager.activeSceneChanged += UpdateInstantiator;
        }

        private void UpdateInstantiator(Scene arg0, Scene arg1)
        {
            if (arg0 == arg1)
                return;
            if (Object.FindObjectOfType<SceneContext>() is { } sceneContext)
                m_instantiator = sceneContext.Container.Resolve<IInstantiator>();
        }

        public void Dispose()
        {
            SceneManager.activeSceneChanged -= UpdateInstantiator;
        }
    }
}
#endif