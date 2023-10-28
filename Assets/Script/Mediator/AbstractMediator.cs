using System.Collections.Generic;
using System.Linq;

public class AbstractMediator
{
    protected List<AbstractController> controllers = new List<AbstractController>();
    protected List<AbstractSystem> systems = new List<AbstractSystem>();
    protected AbstractMediator() { }
    public void RegisterController<T>(T controller) where T : AbstractController
    {
        controllers.Add(controller);
    }
    public void RegisterSystem<T>(T system) where T : AbstractSystem
    {
        systems.Add(system);
    }
    public T GetController<T>() where T : AbstractController
    {
        AbstractController controller = controllers.Where(controller => controller is T).ToArray()[0];
        if (controller != null) return controller as T;
        return default(T);
    }
    public T GetSystem<T>() where T : AbstractSystem
    {
        AbstractSystem system = systems.Where(system => system is T).ToArray()[0];
        if (system != null) return system as T;
        return default(T);
    }
}
