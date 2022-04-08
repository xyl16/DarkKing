/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-04-08 18:40:41 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：配置数据服务
***********************************************************************/

using System.Xml;
using System.Collections.Generic;
using System;

public class CfgSvc
{
    private static CfgSvc ins = null;

    public static CfgSvc Ins
    {
        get
        {
            if (ins == null)
            {
                ins = new CfgSvc();
            }
            return ins;
        }
    }

    public void Init()
    {
        InitGuideCfg();
    }

    #region 自动引导配置
    private Dictionary<int, AutoGuideCfg> guideTaskDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg()
    {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\\UnityProject\\DarkKing\\Client\\Assets\\Resources\\ResCfgs\\guide.xml");

            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

            for (int i = 0; i < nodLst.Count; i++)
            {
                XmlElement ele = nodLst[i] as XmlElement;

                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                AutoGuideCfg mc = new AutoGuideCfg
                {
                    ID = ID
                };

                foreach (XmlElement e in nodLst[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "coin":
                            mc.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            mc.exp = int.Parse(e.InnerText);
                            break;
                    }
                }
                guideTaskDic.Add(ID, mc);
            }
    }
    public AutoGuideCfg GetAutoGuideData(int id)
    {
        AutoGuideCfg agc = null;
        if (guideTaskDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }

    #endregion
}

public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int coin;
    public int exp;
}

public class BaseData<T>
{
    public int ID;
}
