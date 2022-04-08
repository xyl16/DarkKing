/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-03-28 14:20:57 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：网络通信协议(客户端服务端公用)
***********************************************************************/

using PENet;
using System;

namespace PEProtocol
{
    public enum ErrorCode
    {
        None = 0,//无错误

        UpdateDBError,//更新数据库失败

        AcctIsOnline,//账号已经上线
        WrongPass,//密码错误
        NameIsExist,//名字存在
    }

    public enum CMD
    {
        None = 0,
        //登录相关 100
        ReqLogin = 101,
        RespLogin = 102,
        ReqRename = 103,
        RspRename = 104,
    }

    [Serializable]
    public class GameMsg:PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
        public ReqRename reqRename;
        public RspRename rspRename;
    }

    #region 登录相关
    [Serializable]
    public class PlayerData
    {
        //玩家基本信息
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;
        public int pierce;
        public int critical;

        public int guideid;
    }

    [Serializable]
    public class ReqLogin {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;      
    }

    [Serializable]
    public class ReqRename {
        public string name;
    }

    [Serializable]
    public class RspRename
    {
        public string name;
    }

    #endregion

    public class SrvCfg {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
