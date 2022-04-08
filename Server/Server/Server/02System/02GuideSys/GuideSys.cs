/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-04-08 17:46:16 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：引导系统
***********************************************************************/

using PEProtocol;

public class GuideSys
{
    private static GuideSys ins = null;

    public static GuideSys Ins
    {
        get
        {
            if (ins == null)
            {
                ins = new GuideSys();
            }
            return ins;
        }
    }

    public void Init()
    {

    }

    public void ReqGuide(MsgPack pack)
    {
        ReqGuide data = pack.msg.ReqGuide;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspGuide,
        };

        PlayerData player = CacheSvc.Ins.GetPlayerDataBySession(pack.session);

        //更新任务引导id
        if (data.guideid == player.guideid)
        {
            //更新玩家数据
            player.guideid += 1;
        }
        else
        {
            msg.err = (int)ErrorCode.ServerDataError;
        }
        pack.session.SendMsg(msg);
    }
}

