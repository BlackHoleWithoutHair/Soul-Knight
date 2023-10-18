using SoulKnightProtocol;
namespace KnightServer
{
    public class UserController : BaseController
    {
        public UserController(ControllerManager manager) : base(manager)
        {
            requestCode = RequestCode.User;
        }
        public MainPack Register(Client client, MainPack pack)
        {
            if (client.m_UserDao.Register(pack))
            {
                pack.ReturnCode = ReturnCode.Success;
            }
            else
            {
                pack.ReturnCode = ReturnCode.Fail;
            }
            return pack;
        }
        public MainPack Login(Client client, MainPack pack)
        {
            if (client.m_UserDao.Login(pack))
            {
                pack.ReturnCode = ReturnCode.Success;
                client.userName = pack.LoginPack.UserName;
            }
            else
            {
                pack.ReturnCode = ReturnCode.Fail;
            }
            return pack;
        }
    }

}
