
/// <summary>
/// Using this attribute to table holders to display enhanced table inspector.
/// </summary>
/// <seealso cref="UnityEngine.PropertyAttribute" />
public class TableAttribute : UnityEngine.PropertyAttribute
{
    /// <summary>
    /// The row structure type
    /// </summary>
    public System.Type RowType;

    /// <summary>
    /// Initializes a new instance of the <see cref="TableAttribute"/> class.
    /// </summary>
    /// <param name="rowType">Type of the row.</param>
    public TableAttribute(System.Type rowType)
    {
        RowType = rowType;
    }
}
