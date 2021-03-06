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
        ReqGuide data = pack.msg.reqGuide;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspGuide,
        };

        PlayerData player = CacheSvc.Ins.GetPlayerDataBySession(pack.session);
        AutoGuideCfg gc = CfgSvc.Ins.GetAutoGuideData(data.guideid);

        //更新任务引导id
        if (data.guideid == player.guideid)
        {
            player.guideid += 1;

            //更新玩家数据
            player.coin += gc.coin;
            CalExp(player,gc.exp);

            if (!CacheSvc.Ins.UpdatePlayerDBData(player.id,player)) {//更新数据库
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else {
                msg.rspGuide = new RspGuide
                {
                    guideid = player.guideid,
                    coin = player.coin,
                    lv = player.lv,
                    exp = player.exp,
                };
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ServerDataError;
        }
        pack.session.SendMsg(msg);
    }

    private void CalExp(PlayerData pd,int addExp) {
        int curLv = pd.lv;
        int curExp = pd.exp;
        int restExp = addExp;
        while (true) {
            int need = PECommon.GetExpUpValByLv(curLv);
            if (restExp >= need)
            {
                curLv++;
                curExp = 0;
                restExp -= need;
            }
            else {
                pd.lv = curLv;
                pd.exp = curExp + restExp;
                break;
            }
        }
    }
}

