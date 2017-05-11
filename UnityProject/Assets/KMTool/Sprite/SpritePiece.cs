/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-05-11     WP      Initial version
 * 
 * *****************************************************************************/

using KMTool;
using UnityEngine;
using System.Collections;

namespace KMTool
{
/// <summary>
/// 一个精灵的显示、位置相关的控制。
/// renderer and position controller of a sprite !
/// </summary>
public class SpritePiece : MonoBehaviour 
{
        public enum JointPos
        {
            Left,
            Right,
            Top,
            Buttom,
        }

        /// <summary>
        /// the sprite ren of room
        /// 楼层的渲染对象
        /// </summary>
        [SerializeField] private SpriteRenderer sprite;

        /// <summary>
        /// the top pos of sprite 
        /// 这个位置代表Sprite图片的顶点
        /// </summary>
        private Vector3 topPos
        {
            get
            {
                Vector3 pos = sprite.transform.position;
                pos.y += maxY;
                return pos;
            }
        }

        private Vector3 buttomPos
        {
            get
            {
                Vector3 pos = sprite.transform.position;
                pos.y += minY;
                return pos;
            }
        }

        private Vector3 leftPos
        {
            get
            {
                Vector3 pos = sprite.transform.position;
                pos.x += minX;
                return pos;
            }
        }

        private Vector3 rightPos
        {
            get
            {
                Vector3 pos = sprite.transform.position;
                pos.x += maxX;
                return pos;
            }
        }

    
        [SerializeField][DisableEdit] private float maxY;
        [SerializeField][DisableEdit] private float maxX;
        [SerializeField][DisableEdit] private float minY;
        [SerializeField][DisableEdit] private float minX;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    
        [ContextMenu("Init Default Sprite")]
        private void Init()
        {
            SetSprite(sprite.sprite);
        }
    
        public void Init(Sprite sp, SpritePiece target, JointPos jointType)
        {
            SetSprite(sp);

            Joint(target,jointType);
        }

        public void SetSprite(Sprite sp)
        {
            if(sprite && sprite.sprite != sp)
            {
                sprite.sprite = sp;

                //calc top pos:
                float factorX = sprite.transform.lossyScale.x;
                float factorY = sprite.transform.lossyScale.y;
                maxX = sprite.sprite.bounds.max.x * factorX;
                maxY = sprite.sprite.bounds.max.y * factorY;
                minX = sprite.sprite.bounds.min.x * factorX;
                minY = sprite.sprite.bounds.min.y * factorY;
            }

            if(Application.isEditor)
            {
                //calc top pos:
                float factorX = sprite.transform.lossyScale.x;
                float factorY = sprite.transform.lossyScale.y;
                maxX = sprite.sprite.bounds.max.x * factorX;
                maxY = sprite.sprite.bounds.max.y * factorY;
                minX = sprite.sprite.bounds.min.x * factorX;
                minY = sprite.sprite.bounds.min.y * factorY;
            }
        }

        public void Joint(SpritePiece target,JointPos jointPos)
        {
            if (target)
            {
                switch (jointPos)
                {
                    case JointPos.Top:
                        transform.position += target.topPos - buttomPos;
                        break;
                    case JointPos.Right:
                        transform.position += target.rightPos - leftPos;
                        break;
                    case JointPos.Left:
                        transform.position += target.leftPos - rightPos;
                        break;
                    case JointPos.Buttom:
                        transform.position += target.buttomPos - topPos;
                        break;
                }
            }
        }

        #region 测试

    #if UNITY_EDITOR

        public SpritePiece target;
        public JointPos tp;

        public void KMDebug()
        {
            Debug.Log(" ---------KMDebug----------", gameObject);
            Joint(target, tp);
        }

        public void KMEditor()
        {
            Debug.Log(" ---------KMEditor----------", gameObject);
        }

        void OnDrawGizmosSelected()
        {
            if(sprite == null) return;

            UnityEditor.Handles.color = Color.red;
            DrawArrow(topPos);
            UnityEditor.Handles.color = Color.gray;
            DrawArrow(leftPos);
            UnityEditor.Handles.color = Color.green;
            DrawArrow(buttomPos);
            UnityEditor.Handles.color = Color.yellow;
            DrawArrow(rightPos);
        }

        void DrawArrow(Vector3 pos)
        {
            UnityEditor.Handles.ArrowHandleCap(0,
               pos,
               transform.rotation,
               1,
               EventType.Repaint);
        }

    #endif

        #endregion
    }
}