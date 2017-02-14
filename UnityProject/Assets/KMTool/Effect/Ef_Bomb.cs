using UnityEngine;
using System.Collections;

/// <summary>
/// Monster dead effect or other bomb effect to use this script
/// 
/// Maintaince Logs:
/// 2015-04-30	WP			Initial version. 
/// </summary>
public class Ef_Bomb : MonoBehaviour
{
    public Rigidbody[] rbs;
    public Vector3[] localPositions;

    public Vector3 center;

    public float power = 10.0F;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private bool ignoreForceX = false;
    [SerializeField]
    private bool ignoreForceY = false;
    [SerializeField]
    private bool ignoreForceZ = false;
    [SerializeField]
    [Range(0.01f, 5f)]
    private float forceFactorX = 1;
    [SerializeField]
    [Range(0.01f, 5f)]
    private float forceFactorY = 1;
    [SerializeField]
    [Range(0.01f, 5f)]
    private float forceFactorZ = 1;



    public delegate void DelOnFinished(Ef_Bomb ef);
    /// <summary>
    /// 当Finished之后会重置
    /// </summary>
    public DelOnFinished eventOnFinished;

    // Use this for initialization
    void Start()
    {
        if (rbs == null || rbs.Length < 1) rbs = gameObject.GetComponentsInChildren<Rigidbody>();
        if (localPositions == null || localPositions.Length < 1)
        {
            localPositions = new Vector3[rbs.Length];
            for (int i = 0; i < rbs.Length; i++)
            {
                localPositions[i] = rbs[i].transform.localPosition;
            }
        }

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 在设置这个值之前，请确定自身的坐标已经设置好不会改变。
    /// </summary>
    /// <param name="worldPos"></param>
    public void SetBombCenter(Vector3 worldPos)
    {
        center = worldPos - transform.position;
    }

    public void StartDeadEffect(float disableTime = 3)
    {
        gameObject.SetActive(true);

        KMTool.SoundManager.PlaySound(audioSource);

        Vector3 explosionPos = transform.position + center;

        foreach (Rigidbody rb in rbs)
        {
            if (rb)
            {
                rb.mass = Random.Range(0.1f, 0.4f);
                rb.isKinematic = false;
                Vector3 rbPos = rb.transform.position;

                float disX = Mathf.Clamp(Mathf.Abs(rbPos.x - explosionPos.x), 1, 10);
                float disY = Mathf.Clamp(Mathf.Abs(rbPos.y - explosionPos.z), 1, 10);
                float disZ = Mathf.Clamp(Mathf.Abs(rbPos.y - explosionPos.z), 1, 10);

                Vector3 force = (rbPos - explosionPos).normalized;
                if (ignoreForceX)
                    force.x = 0;
                else
                    force.x *= forceFactorX / disX;

                if (ignoreForceY)
                    force.y = 0;
                else
                    force.y *= forceFactorY / disY;

                if (ignoreForceZ)
                    force.z = 0;
                else
                    force.z *= forceFactorZ / disZ;

                rb.AddForce((force) * power, ForceMode.Impulse);
            }
        }

        if (disableTime <= 0)
            disableTime = .5f;

        Invoke("InvokeDisable", disableTime);
    }

    public void StartDeadEffect(Color[] colors, float disableTime = 3, float power = 1.5f)
    {
        gameObject.SetActive(true);

        KMTool.SoundManager.PlaySound(audioSource);

        this.power = power;

        foreach (Rigidbody rb in rbs)
        {
            if (rb)
            {
                //add color to mat
                if (colors.Length > 0)
                {
                    Color color = colors[Random.Range(0, colors.Length)];
                    rb.gameObject.GetComponent<Renderer>().material.color = color;
                }
            }
        }
        StartDeadEffect(disableTime);
    }

    void InvokeDisable()
    {
        //disable to this 
        gameObject.SetActive(false);
        Reset();
        //EffectCtrl.instance.AddBombEfToGC(this);

        if (eventOnFinished != null)
        {
            eventOnFinished(this);
            eventOnFinished = null;
        }
    }

    void Reset()
    {
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].isKinematic = true;
            rbs[i].transform.localPosition = localPositions[i];
            rbs[i].transform.localRotation = Quaternion.identity;
        }
    }

    void KMDebug()
    {
        rbs = null;
        localPositions = null;

        Start();

        Reset();
    }

    void KMEditor()
    {
        if (Application.isPlaying)
        {
            StartDeadEffect(new Color[] { Color.red, Color.gray }, 0);
        }
        else
        {
            Debug.Log("Please use this on Play Mode!");
        }
    }

    void OnDrawGizmos()
    {
        Vector3 pos = transform.position + center;
        Gizmos.DrawLine(pos + transform.forward, pos - transform.forward);
        Gizmos.DrawLine(pos + transform.right, pos - transform.right);
    }
}
