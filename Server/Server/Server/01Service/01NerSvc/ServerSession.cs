/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-03-28 14:41:54 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：网路会话连接
***********************************************************************/

using PENet;
using PEProtocol;

public class ServerSession : PESession<GameMsg>
{
    public int sessionID = 0;
    protected override void OnConnected()
    {
        sessionID = ServerRoot.Ins.GetSessionID();
        PECommon.Log("Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("RecPack CMD:" + (CMD)msg.cmd);

        MsgPack pack = new MsgPack(this,msg);
        NetSvc.Ins.AddMsgQue(pack);
        PECommon.Log("客户端连接ID:" + sessionID);
    }

    protected override void OnDisConnected()
    {
        LoginSys.Ins.ClearOfflineData(this);
        PECommon.Log("Client DisCon");
    }
}
