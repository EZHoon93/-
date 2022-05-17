

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EZPool
{

    [CreateAssetMenu(fileName = "BuffPoolSO Pool", menuName = "Pool/BuffPoolSO Pool")]
    public class BuffPoolSO : ParticlePoolSO
    {
        [AssetsOnly] [BoxGroup("Asset")] [SerializeField] private BuffFactoryBase _buffACtion;
        [SerializeField] private float _durationTime;
        [SerializeField] private Vector3 _addPos;
        [SerializeField] private Stack<BuffFactoryBase> _stackAction = new Stack<BuffFactoryBase>();
        [SerializeField] private object data;

        public Vector3 AddPos => _addPos;

        public float DurationTime => _durationTime;

        public override void OnDisable()
        {
            base.OnDisable();
            _stackAction.Clear();
        }

        public ParticleSystem Pop(Transform target)
        {
            var go = Pop();
            go.transform.SetParent(target.transform);
            go.transform.localPosition = _addPos;
            return go;
        }

        public BuffFactoryBase GetBuffAction()
        {
            BuffFactoryBase buff;
            if(_stackAction.Count > 0)
                buff = _stackAction.Pop();
            else
                buff = _buffACtion.Clone() as BuffFactoryBase;


            buff.onEndEvent += () => { _stackAction.Push(buff); };

            return buff;
        }

        public void ApplyBuff(Damageable target)
        {
            if (target.photonView.IsMine == false)
                return;
            target.ApplyBuff(this);
            //EffectManager.Instance.LocalSendBuffToServer(target.photonView.ViewID, _code , _durationTime);
        }
        public int PopBuffInRange(Vector3 center, float radius, int useViewID,
        LayerMask attackLayer, Define.Team ourTeam)
        {
            Collider[] colliders = new Collider[10];

            var hitCount = Physics.OverlapSphereNonAlloc(center, radius, colliders, attackLayer);
            if (hitCount > 0)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    var damageable = colliders[i].gameObject.GetComponent<Damageable>();
                    //buff.
                    
                    if (damageable != null)
                    {
                        if (damageable.Team == ourTeam)
                            continue;
                        ApplyBuff(damageable);
                    }
                }
            }

            return hitCount;
        }


    }
}