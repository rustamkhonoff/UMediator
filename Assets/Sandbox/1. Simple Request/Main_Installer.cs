using UMediator;
using UMediator.Pipeline;
using Zenject;

namespace Sandbox._1._Simple_Request
{
    public class Main_Installer : MonoInstaller
    {
        public override void InstallBindings()
        {
            DiContainer container = ProjectContext.Instance.Container;
            container.BindInterfacesTo<Main_SimpleRequest.MessageWithNoBNotificationBehavior>().AsTransient();
            container.BindInterfacesTo<Main_SimpleRequest.MessageNotStartsWithANotificationBehavior>().AsTransient();


            // 1. Связывание Generic-Пайплайна (LoggerPipeline)
            // Zenject правильно обрабатывает это как Open Generic Type
            container.Bind(typeof(IPipelineBehavior<,>))
                .To(typeof(Main_SimpleRequest.LoggerPipeline<,>))
                .AsTransient();

            // 2. Связывание Конкретного Пайплайна (DividerPipeline)
            // Этот биндинг добавляет DividerPipeline в коллекцию для конкретного запроса DivideRequest
            container.Bind(typeof(IPipelineBehavior<Main_SimpleRequest.DivideRequest, float>))
                .To<Main_SimpleRequest.DividerPipeline>()
                .AsTransient();
        }
    }
}