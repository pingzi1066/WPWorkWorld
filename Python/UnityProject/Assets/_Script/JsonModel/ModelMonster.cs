/// <summary>
/// 怪物数据类
/// 
/// Maintaince Logs:
/// 2015-04-27	WP			Initial version. 
/// </summary>
public class ModelMonster : ModelBase<StaticMonster>
{
    const int DropNum = 1;

    //private StaticMonster datas { get { return StaticMonster.Instance(); } }

    public float ATK { get { return datas.GetFloat(templateID, "attack"); } }
    private float DEF { get { return datas.GetFloat(templateID, "defense"); } }
    private float HP { get { return datas.GetFloat(templateID, "hp"); } }

    public string prefabName { get { return datas.GetStr(templateID, "prefab_name"); } }

   
    public class DropData
    {
        public int dropRare;
        public int dropMin;
        public int dropMax;
        public int dropCount;

        public DropData(int rare, int tp, int min, int max, int count)
        {
            dropRare = rare;
            dropMin = min;
            dropMax = max;
            dropCount = count;
        }
    }

    public ModelMonster(int id)
    {
        Init(id);
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="id"></param>
    public void Init(int id)
    {
        templateID = id;
    }
}