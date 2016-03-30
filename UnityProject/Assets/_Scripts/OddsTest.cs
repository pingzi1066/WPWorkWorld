/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2015-12-04     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 描述
/// </summary>
public class OddsTest : MonoBehaviour
{
    //执行次数
    public int beginCount = 20;

    //技能次数
    public int skillCount = 5;

    private int curCount = 5;

    //机率
    public int[] oddsBySkill;

    private int curIndex = 0;

    private string desc = "";

    //好的开始
    public int bestStart = 2;
    //好的结束
    public int bestEnd = 10;
    //好技能在一次里
    private int bestSkillNumInOnce = 0;
    //多少次算好技能
    public int bestSkillEnd = 4;
    //全部统计
    private int bestSkillNumInAll = 0;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void KMDebug() { Begin(); }

    void Begin()
    {
        bestSkillNumInAll = 0;
        for (int i = 0; i < beginCount; i++)
        {
            desc = "";
            curIndex = 0;
            curCount = skillCount;
            if (bestSkillNumInOnce >= bestSkillEnd)
            {
                bestSkillNumInAll++;
            }

            bestSkillNumInOnce = 0;
            OnSkill(100);
            //Debug.Log(bestSkillNumInOnce + " in good skill \n" + desc);
        }

        Debug.Log(" best skill num is : " + bestSkillNumInAll + " and sum is ： " + beginCount);
    }

    void OnSkill(int oddSum)
    {
        if (curCount > 0)
            for (; curIndex < oddsBySkill.Length; )
            {
                int odd = oddsBySkill[curIndex];
                if (OddsByInt(odd, oddSum))
                {

                    if (curIndex >= bestStart && curIndex <= bestEnd)
                    {
                        desc += " --good skill";
                        bestSkillNumInOnce++;
                    }

                    desc += "第" + (curIndex + 1) + "技能：" + oddsBySkill[curIndex] + "%\n";

                    curIndex = 0;
                    curCount--;

                    if (curCount > 0)
                    {
                        OnSkill(100);
                    }

                    break;

                }
                else
                {
                    if (curIndex > oddsBySkill.Length - 2)
                    {
                        Debug.Log(oddsBySkill[curIndex] + "----" + oddSum);
                    }

                    curIndex++;
                }
                oddSum -= odd;
            }

        if (curCount > 0)
        {
            desc += "普通攻击" + "\n";
            curIndex = 0;
            curCount--;

            if (curCount > 0)
            {
                OnSkill(100);
            }
        }
    }

    public bool OddsByInt(int odds, int oddSum)
    {
        if (odds > 100 || odds < 0) return false;
        if (UnityEngine.Random.Range(0, oddSum * 100) < odds * 100)
        {
            return true;
        }
        else return false;
    }
}