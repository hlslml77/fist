using MobaCommon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 游戏数据
/// </summary>
public class GameData
{
    public static PlayerDto Player;

    //用basecontrol既可以访问数据，也可以获取游戏物体
    public static BaseControl  MyControl;
}

