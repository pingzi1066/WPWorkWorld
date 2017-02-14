/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-02-14     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;

namespace KMTool
{
    public class StaticAvatar : StaticJson<StaticAvatar> { }

    public class ModelAvatar : ModelBase<StaticAvatar>
    {
        public string Name
        {
            get
            {
                return GetStr("name");
            }
        }

        public string Info
        {
            get
            {
                return GetStr("info");
            }
        }

        public GameObject GetUIPrefab()
        {
            string path = "Avatar/UI/" + GetStr("ui_res");

            GameObject go = Resources.Load(path, typeof(GameObject)) as GameObject;

            return go;
        }

        public GameObject GetGamePrefab()
        {
            string path = "Avatar/UI/" + GetStr("game_res");

            GameObject go = Resources.Load(path, typeof(GameObject)) as GameObject;

            return go;
        }
    }

    public enum E_AvatarData
    {
        /// <summary>
        /// 当前选择的角色 的ID
        /// </summary>
        curAvatarId,
    }

    /// <summary>
    /// 用于角色表数据
    /// </summary>
    public class AvatarData : LocalInt<AvatarData, E_AvatarData>
    {
        protected override int GetDefaultValue(E_AvatarData e)
        {
            switch (e)
            {
                case E_AvatarData.curAvatarId:
                    return AvatarListData.instance.GetData(E_AvatarList.UnlockIds)[0];
                default:
                    Debug.Log("the AvatarData isn't unknow value " + e);
                    break;
            }

            return 0;
        }
    }

    public enum E_AvatarList
    {
        UnlockIds,
    }

    /// <summary>
    /// 当前已经解锁的角色
    /// </summary>
    public class AvatarListData : LocalListInt<AvatarListData, E_AvatarList>
    {
        protected override List<int> GetDefaultValue(E_AvatarList e)
        {
            List<int> list = new List<int>();
            switch (e)
            {
                case E_AvatarList.UnlockIds:
                    list.Add(StaticAvatar.Instance().allID[0]);
                    break;
                default:
                    Debug.Log("the AvatarData isn't unknow value " + e);
                    break;
            }

            return list;
        }
    }
}