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
    //加载角色数据,直接将信息返回到playerInfo中
    public static void LoadPlayer(ref PlayerInfo playerInfo)
    {
        //加载配置表文件
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        //读取基础信息
        //获得根节点
        XmlElement root = doc.DocumentElement;
        //获取信息
        playerInfo.Level = int.Parse(root.SelectSingleNode("Level").InnerText);
        playerInfo.HP= int.Parse(root.SelectSingleNode("HP").InnerText);
        playerInfo.GunID_1 = int.Parse(root.SelectSingleNode("Gun1").InnerText);
        playerInfo.GunID_2 = int.Parse(root.SelectSingleNode("Gun2").InnerText);
        playerInfo.MoveSpeed = float.Parse(root.SelectSingleNode("MoveSpeed").InnerText);
        playerInfo.Weapon_1 = root.SelectSingleNode("Weapon_1").InnerText;
        playerInfo.Weapon_2 = root.SelectSingleNode("Weapon_2").InnerText;
        playerInfo.Money = int.Parse(root.SelectSingleNode("Money").InnerText);
    }

    //保存玩家装备数据，在军械库调用
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
        sb.Append(string.Format("\t<Weapon_1>{0}</Weapon_1>\n", playerInfo.Weapon_1));
        sb.Append(string.Format("\t<Weapon_2>{0}</Weapon_2>\n", playerInfo.Weapon_2));
        sb.Append(string.Format("\t<Money>{0}</Money>\n", playerInfo.Money));

        sb.Append("</PlayerData>");
        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }


}
