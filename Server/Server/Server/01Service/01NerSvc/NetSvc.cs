/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-03-28 14:16:31 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：网络服务
***********************************************************************/


using PENet;
using PEProtocol;
using System.Collections.Generic;

public class MsgPack
{
    public ServerSession session;
    public GameMsg msg;

    public MsgPack(ServerSession session, GameMsg msg)
    {
        this.session = session;
        this.msg = msg;
    }
}

public class NetSvc
{
    private static NetSvc ins = null;

    public static NetSvc Ins
    {
        get
        {
            if (ins == null)
            {
                ins = new NetSvc();
            }
            return ins;
        }
    }

    public static readonly string obj = "lock";
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(SrvCfg.srvIP, SrvCfg.srvPort);

        PECommon.Log("NetSvc Init Done");
    }

    public void AddMsgQue(MsgPack pack)
    {
        lock (obj)
        {
            msgPackQue.Enqueue(pack);
        }
    }

    public void Update()
    {
        if (msgPackQue.Count > 0)
        {
            lock (obj)
            {
                MsgPack msg = msgPackQue.Dequeue();
                HandOutMsg(msg);
            }
        }
    }

    private void HandOutMsg(MsgPack pack)
    {
        switch ((CMD)pack.msg.cmd)
        {
            case CMD.ReqLogin:
                LoginSys.Ins.ReqLogin(pack);
                break;
            case CMD.ReqRename:
                LoginSys.Ins.ReqRename(pack);
                break;
            case CMD.ReqGuide:
                GuideSys.Ins.ReqGuide(pack);
                break;
        }
    }
}
