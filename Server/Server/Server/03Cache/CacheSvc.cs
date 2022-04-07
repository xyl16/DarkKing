/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-03-29 17:41:08 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：缓存层
***********************************************************************/


using PEProtocol;
using System.Collections.Generic;

public class CacheSvc
{
    private static CacheSvc ins = null;

    public static CacheSvc Ins
    {
        get
        {
            if (ins == null)
            {
                ins = new CacheSvc();
            }
            return ins;
        }
    }

    private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    public void Init()
    {

    }

    public bool IsAcctOnline(string acct)
    {
        return onLineAcctDic.ContainsKey(acct);
    }

    /// <summary>
    /// 根据账号密码返回对应账号信息,密码错误返回null,账号不存在则默认创建新账号
    /// </summary>
    public PlayerData GetPlayerData(string acct, string pass)
    {
        PlayerData player = DBMgr.Ins.QueryPlayerData(acct, pass);
        return player;
    }

    /// <summary>
    /// 账号上线,缓存数据
    /// </summary>
    public void AccOnline(string acct, ServerSession session, PlayerData playerData)
    {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }

    public bool isNameExist(string name)
    {
        return DBMgr.Ins.QueryNameData(name);
    }

    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        if (onLineSessionDic.TryGetValue(session, out PlayerData playerData))
        {
            return playerData;
        }
        return null;
    }

    public bool UpdatePlayerDBData(int id, PlayerData playerData)
    {
        return DBMgr.Ins.UpdatePlayerDBData(id, playerData);
    }

    /// <summary>
    /// 客户端下线,清除缓存信息
    /// </summary>
    public void AcctOffline(ServerSession session)
    {
        foreach (var item in onLineAcctDic)
        {
            if (item.Value == session)
            {
                onLineAcctDic.Remove(item.Key);
                break;
            }
        }

        bool succ = onLineSessionDic.Remove(session);
        PECommon.Log("下线客户端ID：" + session.sessionID + "下线成功状态:" + succ);
    }
}
