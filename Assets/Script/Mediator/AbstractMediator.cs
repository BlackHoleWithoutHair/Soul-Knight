using System.Collections.Generic;

public class AbstractMediator
{
    protected List<AbstractController> controllers = new List<AbstractController>();
    protected AbstractMediator() { }
    public void Register<T>(T controller) where T : AbstractController
    {
        controllers.Add(controller);
    }
    public T GetController<T>() where T : AbstractController
    {
        foreach (AbstractController controller in controllers)
        {
            if (controller is T)
            {
                return controller as T;
            }
        }
        return default(T);
    }
}
