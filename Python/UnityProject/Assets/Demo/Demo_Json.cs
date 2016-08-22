using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 描述
/// 
/// Maintaince Logs:
/// 2015-	WP			Initial version. 
/// </summary>
public class Demo_Json : MonoBehaviour
{

	public Text tex;

    // Use this for initialization
    void Start()
    {
        
//        StaticBoss.Instance().Print();

        PrintRandom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void KMDebug() { PrintRandom(); }

    void PrintRandom()
    {
//        int length = StaticMonster.Instance().allID.Length;
		int id = 300001;// = StaticMonster.Instance().allID[Random.Range(0, length)];

		string log = "";
		string keyName = "defense";

		log += keyName + ": " + ModelMonster.GetData(id)[keyName] + "\n";

		keyName = "atk";

		log += keyName + ": " + ModelMonster.GetData(id)[keyName] + "\n";

		Debug.Log(ModelMonster.GetData(id)["defense"]);
		Debug.Log(ModelMonster.GetData(100)["atk"]);

        log += StaticMonster.Instance().Print();

		if (tex) {
			tex.text = log;
		}

        
        //int id = StaticBoss.Instance().allID[0];
        //Debug.Log(ModelBoss.GetData(id).name + " id is " + id);
//        Debug.Log(StaticBoss.GetInstance().allID.Length);
    }
}
