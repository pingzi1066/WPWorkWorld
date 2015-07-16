/******************************************************************************
 *
 * Maintaince Logs:
 * 2012-12-20    WP   Initial version. 
 *
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class ItemMaker : MonoBehaviour
{

    public GameObject prefab;

    /// <summary>
    /// Maximum size of the container. Adding more items than this number will not work.
    /// </summary>

    public int maxItemCount = 9;

    /// <summary>
    /// Maximum number of rows to create.
    /// </summary>

    public int maxRows = 3;

    /// <summary>
    /// Maximum number of columns to create.
    /// </summary>

    public int maxColumns = 3;

    /// <summary>
    /// Width between icons.
    /// </summary>

    public float width = 128;

    /// <summary>
    /// height between icons.
    /// </summary>
    public float height = 128;

    /// <summary>
    /// Padding around the border.
    /// </summary>

    public int padding = 10;

    void Awake()
    {
        CreateSlots();
    }

    void CreateSlots()
    {
        if (prefab != null)
        {
            int count = 0;
            Bounds b = new Bounds();
            for (int y = 0; y < maxRows; ++y)
            {
                for (int x = 0; x < maxColumns; ++x)
                {

                    GameObject go = KMTools.AddGameObj(gameObject, prefab, true, false);
                    Transform t = go.transform;
                    t.localPosition = new Vector3(padding + (x + 0.5f) * width, -padding - (y + 0.5f) * height, 0f);
                    b.Encapsulate(new Vector3(padding * 2f + (x + 1) * width, -padding * 2f - (y + 1) * height, 0f));

                    if (++count >= maxItemCount)
                    {
                        return;
                    }
                }
            }
        }
    }
}
