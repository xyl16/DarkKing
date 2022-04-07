/************************************************************************
*创 建 者      ：@ xyl16
*创建日期    ：2022-03-28 12:17:33 
*电子邮箱    ：1531141462@qq.com
*功能描述    ：服务器入口
***********************************************************************/


namespace Server
{
    class ServerStart
    {
        static void Main(string[] args)
        {
            ServerRoot.Ins.Init();

            while (true) {
                ServerRoot.Ins.Update();
            }
        }
    }
}
