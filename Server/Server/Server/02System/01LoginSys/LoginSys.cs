/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-03-28 14:20:57 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：登录业务系统
***********************************************************************/


using PEProtocol;

public class LoginSys
{
    private static LoginSys ins = null;

    public static LoginSys Ins
    {
        get
        {
            if (ins == null)
            {
                ins = new LoginSys();
            }
            return ins;
        }
    }

    private CacheSvc cacheSvc;

    public void Init()
    {
        cacheSvc = CacheSvc.Ins;
    }

    public void ReqLogin(MsgPack pack)
    {
        //返回结果给客户端
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin,
            rspLogin = new RspLogin
            {

            }
        };


        //当前账号是否已上线
        if (cacheSvc.IsAcctOnline(pack.msg.reqLogin.acct))
        {
            //已上线，返回错误信息
            msg.err = (int)ErrorCode.AcctIsOnline;
        }
        else
        {
            //未上线
            PlayerData pd = cacheSvc.GetPlayerData(pack.msg.reqLogin.acct, pack.msg.reqLogin.pass);
            //只有账号密码均正确,pd才存在
            if (pd == null)
            {
                //账号存在,密码错误
                msg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                msg.rspLogin = new RspLogin
                {
                    playerData = pd
                };

                //缓存账号数据
                cacheSvc.AccOnline(pack.msg.reqLogin.acct, pack.session, pd);
            }
        }

        pack.session.SendMsg(msg);
    }

    public void ReqRename(MsgPack pack)
    {
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename
        };

        if (cacheSvc.isNameExist(pack.msg.reqRename.name))
        {
            //名字是否已经存在
            //存在:返回错误码
            msg.err = (int)ErrorCode.NameIsExist;
        }
        else
        {
            //不存在:更新缓存，以及数据库，再返回客户端
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.session);
            playerData.name = pack.msg.reqRename.name;//更新缓存

            if (!cacheSvc.UpdatePlayerDBData(playerData.id, playerData))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename
                {
                    name = pack.msg.reqRename.name,
                };
            }

        }
        pack.session.SendMsg(msg);//发给对应改名玩家
    }

    public void ClearOfflineData(ServerSession session)
    {
        cacheSvc.AcctOffline(session);
    }
}

