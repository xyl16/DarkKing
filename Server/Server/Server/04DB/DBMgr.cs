/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-03-30 14:53:30 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：数据库管理类
***********************************************************************/


using MySql.Data.MySqlClient;
using PEProtocol;
using System;

public class DBMgr
{
    private static DBMgr ins = null;

    public static DBMgr Ins
    {
        get
        {
            if (ins == null)
            {
                ins = new DBMgr();
            }
            return ins;
        }
    }

    private MySqlConnection conn;

    public void Init()
    {
        string connStr = "server=localhost;User Id=root;password=;Database=darkgod;Charset=utf8";
        conn = new MySqlConnection(connStr);
        conn.Open();
    }

    public PlayerData QueryPlayerData(string acct, string pass)
    {
        bool isNew = true;
        PlayerData playerData = null;
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                //账号存在
                isNew = false;
                string _pass = reader.GetString("pass");
                if (_pass.Equals(pass))
                {
                    //密码正确
                    playerData = new PlayerData
                    {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                        lv = reader.GetInt32("lv"),
                        exp = reader.GetInt32("exp"),
                        power = reader.GetInt32("power"),
                        coin = reader.GetInt32("coin"),
                        diamond = reader.GetInt32("diamond"),

                        hp = reader.GetInt32("hp"),
                        ad = reader.GetInt32("ad"),
                        ap = reader.GetInt32("ap"),
                        addef = reader.GetInt32("addef"),
                        apdef = reader.GetInt32("apdef"),
                        dodge = reader.GetInt32("dodge"),
                        pierce = reader.GetInt32("pierce"),
                        critical = reader.GetInt32("critical"),
                    };
                }
            }
        }
        catch (Exception e)
        {
            PECommon.Log("查询玩家账号出错！" + e, LogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
            if (isNew)
            {
                //账号不存在，创建新号
                playerData = new PlayerData
                {
                    id = -1,//暂定为-1，待插入数据库生成唯一id
                    name = "",//一定为空，用于客户端判断是否为新账号(显示创角面板)
                    lv = 1,
                    exp = 0,
                    power = 0,
                    coin = 0,
                    diamond = 0,

                    hp = 0,
                    ad = 0,
                    ap = 0,
                    addef = 0,
                    apdef = 0,
                    dodge = 0,
                    pierce = 0,
                    critical = 0,
                };
                playerData.id = InsertNewAcct(acct, pass, playerData);
            }
        }

        return playerData;
    }

    /// <summary>
    /// 插入新账号
    /// </summary>
    private int InsertNewAcct(string acct, string pass, PlayerData pd)
    {
        int id = -1;
        try
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into account set acct = @acct,pass = @pass,name = @name,lv = @lv, exp = @exp,power = @power,coin = @coin," +
                "diamond = @diamond,hp = @hp,ad = @ad,ap = @ap,addef = @addef,apdef = @apdef,dodge = @dodge,pierce = @pierce,critical = @critical", conn);

            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("lv", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);

            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);

            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (Exception e)
        {
            PECommon.Log("插入玩家新账号出错！" + e, LogType.Error);
        }
        return id;//返回自动生成的主键
    }

    /// <summary>
    /// 查询指定名字在数据库中是否存在
    /// </summary>
    public bool QueryNameData(string name)
    {
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where name = @name", conn);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return true;
            }
        }
        catch (Exception e)
        {
            PECommon.Log("查找指定名字是否存在失败!" + e, LogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        return false;
    }

    public bool UpdatePlayerDBData(int id, PlayerData pd)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand("update account set name = @name,lv = @lv, exp = @exp,power = @power,coin = @coin,diamond = @diamond," +
                "hp = @hp,ad = @ad,ap = @ap,addef = @addef,apdef = @apdef,dodge = @dodge,pierce = @pierce,critical = @critical where id =@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("lv", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);

            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);

            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            PECommon.Log("更新数据库玩家名字出错！" + e, LogType.Error);
            return false;
        }
        return true;
    }
}


