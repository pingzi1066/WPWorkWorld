/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-03-30     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;

namespace KMTool
{

    /// <summary>
    /// 提供于Rect的便用方法
    /// </summary>
    public static class KMRect
    {
        /// <summary>
        /// Hierarchy 里面从右往左数的位置
        /// </summary>
        static public Rect H_CR(this Rect rect, int index = 1, int offectX = 0)
        {
            int dis = 2 * index;
            rect.x += rect.width - index * rect.height - dis - offectX;

            return H_Size(rect);
        }

        /// <summary>
        /// 重置定义Rect的大小
        /// </summary>
        static public Rect H_Size(this Rect rect, int width = 16, int height = 16)
        {
            rect.size = new Vector2(width, height);
            return rect;
        }
    }
}