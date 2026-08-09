using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MEventArgs
{

}

public class MPlayerInfoArgs : MEventArgs
{
    public PlayerInfo PlayerInfo;

    public MPlayerInfoArgs(PlayerInfo playerInfo)
    {
        PlayerInfo = playerInfo;
    }
}

public class MPlayerObjectArgs : MEventArgs
{
    public GameObject Player;
    public MPlayerObjectArgs(GameObject player)
    {
        Player = player;
    }
}

public class MEnemyDeadArgs : MEventArgs
{
    public int Score;
    public MEnemyDeadArgs(int score)
    {
        Score = score;
    }
}

public class MPlayerHPChange : MEventArgs
{
    public int CurHP;
    public MPlayerHPChange(int curHP)
    {
        CurHP = curHP;
    }
}

