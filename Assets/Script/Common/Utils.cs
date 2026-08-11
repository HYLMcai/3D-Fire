using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class Utils
{
    static string path = Application.dataPath + @"/Resources/PlayerData/PlayerData.xml";
    //加载角色数据，直接读取到 playerInfo 中
    public static void LoadPlayer(ref PlayerInfo playerInfo)
    {
        //加载XML文件
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        //获取根节点
        XmlElement root = doc.DocumentElement;
        //读取玩家信息
        playerInfo.Level = int.Parse(root.SelectSingleNode("Level").InnerText);
        playerInfo.HP= int.Parse(root.SelectSingleNode("HP").InnerText);
        playerInfo.GunID_1 = int.Parse(root.SelectSingleNode("Gun1").InnerText);
        playerInfo.GunID_2 = int.Parse(root.SelectSingleNode("Gun2").InnerText);
        playerInfo.MoveSpeed = float.Parse(root.SelectSingleNode("MoveSpeed").InnerText);
        playerInfo.Money = int.Parse(root.SelectSingleNode("Money").InnerText);
    }

    //保存玩家数据到 XML
    public static void SavePlayerInfo(PlayerInfo playerInfo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        sb.Append("<PlayerData>\n");

        sb.Append(string.Format("\t<Level>{0}</Level>\n", playerInfo.Level));
        sb.Append(string.Format("\t<HP>{0}</HP>\n", playerInfo.HP));
        sb.Append(string.Format("\t<Gun1>{0}</Gun1>\n", playerInfo.GunID_1));
        sb.Append(string.Format("\t<Gun2>{0}</Gun2>\n", playerInfo.GunID_2));
        sb.Append(string.Format("\t<MoveSpeed>{0}</MoveSpeed>\n", playerInfo.MoveSpeed));
        sb.Append(string.Format("\t<Money>{0}</Money>\n", playerInfo.Money));

        sb.Append("</PlayerData>");
        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }


}
