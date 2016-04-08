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

    public void StartDeadEffect(Color[] colors, float disableTime = 3, float power = 1.5f)
    {
        SoundManager.PlaySound(audioSource);

        gameObject.SetActive(true);

        this.power = power;

        Vector3 explosionPos = transform.position + center;

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
                rb.mass = Random.Range(0.1f, 0.4f);
                rb.isKinematic = false;
                rb.AddForce((rb.transform.position - explosionPos) * power, ForceMode.Impulse);
            }
        }

        if (disableTime > 0)
            Invoke("InvokeDisable", disableTime);
    }

    void InvokeDisable()
    {
        //disable to this 
        gameObject.SetActive(false);
        Reset();
        //EffectCtrl.instance.AddBombEfToGC(this);
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
