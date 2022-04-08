/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-03-28 15:59:00 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：客户端服务端通用工具类
***********************************************************************/

using PENet;
using PEProtocol;

public enum LogType {
    Log = 0,
    Warn = 1,
    Error = 2,
    Info = 3
}

public class PECommon
{
    public static void Log(string msg = "", LogType tp = LogType.Log) {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg,lv);
    }

    public static int GetFightByProps(PlayerData pd) {
        return pd.lv*100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    public static int GetPowerLimit(int lv) {
        return ((lv - 1) / 10 * 150 + 150);
    }

    public static int GetExpUpValByLv(int lv) {
        return 100 * lv * lv;
    }
}

