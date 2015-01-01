﻿
/// <summary>
/// 提供 编辑器 String 
/// 
/// Maintaince Logs:
/// 2014-11-01      WP      Initial version
/// </summary>
public static class StrsEditor
{
    private const string MENU_HEAD = "Tools/";
    private const string MENU_TRANSFORM = "变换/";

    public const string MENU_COPY_POS_ROT = MENU_HEAD + MENU_TRANSFORM + "复制世界坐标与旋转 %#c";
    public const string MENU_PASTE_POS_ROT = MENU_HEAD + MENU_TRANSFORM + "粘贴世界坐标与旋转 %#d";
    public const string MENU_RoundPRS = MENU_HEAD + MENU_TRANSFORM + "使自身PRS变为整数 %#r";
    public const string MENU_RoundPRSNChildren = MENU_HEAD + MENU_TRANSFORM + "子对象下所有物品的PRS坐标精确到整数";
    public const string MENU_RoundScaleNChildren = MENU_HEAD + MENU_TRANSFORM + "子对象下所有物品的大小精确到整数";
    public const string MENU_CopyLocalPosition = MENU_HEAD + MENU_TRANSFORM + "复制局部坐标 %#x";
    public const string MENU_PasteLocalPosition = MENU_HEAD + MENU_TRANSFORM + "粘贴局部坐标 %#v";

    private const string MENU_TEST = "测试/";

    public const string MENU_KMDebug = MENU_HEAD + MENU_TEST + "SendMessage方法KMDebug到对象 %#l";
    public const string MENU_KMEditor = MENU_HEAD + MENU_TEST + "SendMessage方法KMEditor到对象 %#k";

    //private const string MENU_Navigation = "寻路/";

    //public const string MENU_SetToNavigationStatic = MENU_HEAD + MENU_Navigation + "静态寻路对象(包括OffMeshLinkGeneration)";
    //public const string MENU_SetToNavigationStaticAndWithOutOffMeshLinkGeneration = MENU_HEAD + MENU_Navigation + "所选->静态寻路对象(不包括OffMeshLinkGeneration)";
    //public const string MENU_SetToWithOutNavigationStatic = MENU_HEAD + MENU_Navigation + "所选->静态非寻路对象(不包括OffMeshLinkGeneration)";

    private const string MENU_AUDIO = MENU_HEAD + "音效/";
    public const string MENU_AUDIO_SET3D = MENU_AUDIO + "默认3D音效";

    private const string MENU_Time = "时间/";
    public const string MENU_Time_Plus = MENU_HEAD + MENU_Time + "游戏时间递增 %#=";
    public const string MENU_Time_Minus = MENU_HEAD + MENU_Time + "游戏时间递减 %#-";
    public const string MENU_Time_Default = MENU_HEAD + MENU_Time + "游戏时间默认";
}
