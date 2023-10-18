using SoulKnightProtocol;
using System.Reflection;
namespace KnightServer
{
    public class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDic = new Dictionary<RequestCode, BaseController>();
        private Server server;
        public Server m_Server => server;
        public ControllerManager(Server server)
        {
            controllerDic.Add(RequestCode.User, new UserController(this));
            controllerDic.Add(RequestCode.Room, new RoomController(this));
            controllerDic.Add(RequestCode.Game, new GameController(this));
            this.server = server;
        }
        public void HandleRequest(MainPack pack, Client client, bool isUDP = false)
        {
            //Console.WriteLine(pack.RequestCode+" "+pack.ActionCode);
            if (controllerDic.TryGetValue(pack.RequestCode, out BaseController controller))
            {
                string methodName = pack.ActionCode.ToString();
                MethodInfo methodInfo = controller.GetType().GetMethod(methodName);
                if (methodInfo == null)
                {
                    Console.WriteLine("没有找到对应的Action: " + methodName + " 来处理请求");
                }
                if (isUDP)
                {
                    methodInfo.Invoke(controller, new object[] { client, pack });

                }
                else
                {
                    object ret = methodInfo.Invoke(controller, new object[] { client, pack });
                    if (ret != null)
                    {
                        client.Send(ret as MainPack);
                    }
                }

            }
            else
            {

            }
        }
    }

}
