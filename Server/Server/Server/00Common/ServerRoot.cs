/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-03-28 12:17:33 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：服务器初始化
***********************************************************************/


public class ServerRoot
{
    private static ServerRoot ins = null;

    public static ServerRoot Ins {
        get {
            if (ins == null) {
                ins = new ServerRoot();
            }
            return ins;
        }
    }

    public void Init() {
        //数据层
        DBMgr.Ins.Init();

        //服务层
        NetSvc.Ins.Init();
        CacheSvc.Ins.Init();

        //业务系统层
        LoginSys.Ins.Init();

    }

    public void Update() {
        NetSvc.Ins.Update();
    }

    private int sessionID = 0;
    public int GetSessionID() {
        return sessionID + 1;
    }
}
