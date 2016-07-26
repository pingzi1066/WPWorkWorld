/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-25     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// Generic class for using <see cref="TableAttribute"/>
/// </summary>
/// <typeparam name="T">Type of struct</typeparam>
public class DataTable<T> where T : struct
{
    /// <summary>
    /// The Rows
    /// </summary>
    public T[] Rows;

    /// <summary>
    /// Gets the length of <see cref="Rows"/>.
    /// </summary>
    /// <value>
    /// The count.
    /// </value>
    public int Count { get { return Rows.Length; } }

    /// <summary>
    /// Whether the inner Rows exist?
    /// </summary>
    /// <param name="table">The table.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator bool(DataTable<T> table)
    {
        return table.Rows == null ? false : true;
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="Table{T}"/> to <see cref="T[]"/>.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator T[](DataTable<T> table)
    {
        return table.Rows;
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T[]"/> to <see cref="Table{T}"/>.
    /// </summary>
    /// <param name="rows">The rows.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator DataTable<T>(T[] rows)
    {
        return new DataTable<T> { Rows = rows };
    }

    /// <summary>
    /// Gets or sets the <see cref="T"/> at the specified index.
    /// </summary>
    /// <value>
    /// The <see cref="T"/>.
    /// </value>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public T this[int index]
    {
        get
        {
            return Rows[index];
        }
        set
        {
            Rows[index] = value;
        }
    }
}