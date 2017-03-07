
using UnityEngine;
using UnityEngine.Events;
using AnimationOrTween;
using System.Collections.Generic;

namespace KMTool
{

/// <summary>
/// Play the specified tween on click.
/// </summary>

    [ExecuteInEditMode]
    [AddComponentMenu("KMTool/Play Tween")]
    public class KMPlayTween : MonoBehaviour
    {
	    static public KMPlayTween current;

	    /// <summary>
	    /// Target on which there is one or more tween.
	    /// </summary>

	    public GameObject tweenTarget;

	    /// <summary>
	    /// If there are multiple tweens, you can choose which ones get activated by changing their group.
	    /// </summary>

	    public int tweenGroup = 0;

	    /// <summary>
	    /// Direction to tween in.
	    /// </summary>

	    public Direction playDirection = Direction.Forward;

	    /// <summary>
	    /// Whether the tween will be reset to the start or end when activated. If not, it will continue from where it currently is.
	    /// </summary>

	    public bool resetOnPlay = false;

	    /// <summary>
	    /// Whether the tween will be reset to the start if it's disabled when activated.
	    /// </summary>

	    public bool resetIfDisabled = false;

	    /// <summary>
	    /// What to do if the tweenTarget game object is currently disabled.
	    /// </summary>

	    public EnableCondition ifDisabledOnPlay = EnableCondition.DoNothing;

	    /// <summary>
	    /// What to do with the tweenTarget after the tween finishes.
	    /// </summary>

	    public DisableCondition disableWhenFinished = DisableCondition.DoNotDisable;

	    /// <summary>
	    /// Whether the tweens on the child game objects will be considered.
	    /// </summary>

	    public bool includeChildren = false;

	    /// <summary>
	    /// Event delegates called when the animation finishes.
	    /// </summary>

	    //public List<EventDelegate> onFinished = new List<EventDelegate>();
        public UnityEvent onFinished;

	    // Deprecated functionality, kept for backwards compatibility
	    [HideInInspector][SerializeField] GameObject eventReceiver;
	    [HideInInspector][SerializeField] string callWhenFinished;

	    KMTweener[] mTweens;
	    int mActive = 0;

	    void Awake ()
	    {
		    // Remove deprecated functionality if new one is used
		    if (eventReceiver != null )//&& EventDelegate.IsValid(onFinished))
		    {
			    eventReceiver = null;
			    callWhenFinished = null;
    #if UNITY_EDITOR
			    UnityEditor.EditorUtility.SetDirty(this);
    #endif
		    }
	    }

	    void Start()
	    {
		    if (tweenTarget == null)
		    {
			    tweenTarget = gameObject;
    #if UNITY_EDITOR
			    UnityEditor.EditorUtility.SetDirty(this);
    #endif
		    }
	    }

	    void Update ()
	    {
    #if UNITY_EDITOR
		    if (!Application.isPlaying) return;
    #endif
		    if (disableWhenFinished != DisableCondition.DoNotDisable && mTweens != null)
		    {
			    bool isFinished = true;
			    bool properDirection = true;

			    for (int i = 0, imax = mTweens.Length; i < imax; ++i)
			    {
				    KMTweener tw = mTweens[i];
				    if (tw.tweenGroup != tweenGroup) continue;

				    if (tw.enabled)
				    {
					    isFinished = false;
					    break;
				    }
				    else if ((int)tw.direction != (int)disableWhenFinished)
				    {
					    properDirection = false;
				    }
			    }

			    if (isFinished)
			    {
				    if (properDirection) KMTools.SetActive(tweenTarget, false);
				    mTweens = null;
			    }
		    }
	    }

	    /// <summary>
	    /// Activate the tweeners.
	    /// </summary>

	    public void Play (bool forward)
	    {
		    mActive = 0;
		    GameObject go = (tweenTarget == null) ? gameObject : tweenTarget;

		    if (!KMTools.GetActive(go))
		    {
			    // If the object is disabled, don't do anything
			    if (ifDisabledOnPlay != EnableCondition.EnableThenPlay) return;

			    // Enable the game object before tweening it
			    KMTools.SetActive(go, true);
		    }

		    // Gather the tweening components
		    mTweens = includeChildren ? go.GetComponentsInChildren<KMTweener>() : go.GetComponents<KMTweener>();

		    if (mTweens.Length == 0)
		    {
			    // No tweeners found -- should we disable the object?
			    if (disableWhenFinished != DisableCondition.DoNotDisable)
				    KMTools.SetActive(tweenTarget, false);
		    }
		    else
		    {
			    bool activated = false;
			    if (playDirection == Direction.Reverse) forward = !forward;

			    // Run through all located tween components
			    for (int i = 0, imax = mTweens.Length; i < imax; ++i)
			    {
				    KMTweener tw = mTweens[i];

				    // If the tweener's group matches, we can work with it
				    if (tw.tweenGroup == tweenGroup)
				    {
					    // Ensure that the game objects are enabled
					    if (!activated && !KMTools.GetActive(go))
					    {
						    activated = true;
						    KMTools.SetActive(go, true);
					    }

					    ++mActive;

					    // Toggle or activate the tween component
					    if (playDirection == Direction.Toggle)
					    {
						    // Listen for tween finished messages
                            tw.onFinished.AddListener(OnFinished);
						    //EventDelegate.Add(tw.onFinished, OnFinished, true);
						    tw.Toggle();
					    }
					    else
					    {
						    if (resetOnPlay || (resetIfDisabled && !tw.enabled))
						    {
							    tw.Play(forward);
							    tw.ResetToBeginning();
						    }
						    // Listen for tween finished messages
                            tw.onFinished.AddListener(OnFinished);
						    //EventDelegate.Add(tw.onFinished, OnFinished, true);
						    tw.Play(forward);
					    }
				    }
			    }
		    }
	    }

	    /// <summary>
	    /// Callback triggered when each tween executed by this script finishes.
	    /// </summary>

	    void OnFinished ()
	    {
		    if (--mActive == 0 && current == null)
		    {
			    current = this;

                if(onFinished != null) onFinished.Invoke();

                //Remove current onfinished!!
                if(KMTweener.current)
                    KMTweener.current.onFinished.RemoveListener(OnFinished);

			    // Legacy functionality
			    if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
				    eventReceiver.SendMessage(callWhenFinished, SendMessageOptions.DontRequireReceiver);

			    eventReceiver = null;
			    current = null;
		    }
	    }
    }
 }