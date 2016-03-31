/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-03-30     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 提供于Rect的便用方法
/// </summary>
public static class KMRect
{

    static public Rect TT(this Rect rect, float x)
    {
        return rect;
    }

    /// <summary>
    /// Hierarchy 里面从右数的小位置
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    static public Rect H_CR(this Rect rect, int index = 1, int offectX = 0)
    {
        int dis = 2 * index;
        rect.x += rect.width - index * rect.height - dis - offectX;

        return H_Size(rect);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    static public Rect H_Size(this Rect rect, int width = 16, int height = 16)
    {
        rect.size = new Vector2(width, height);
        return rect;
    }
}