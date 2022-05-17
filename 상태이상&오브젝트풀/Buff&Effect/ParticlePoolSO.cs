
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
namespace EZPool
{
    [CreateAssetMenu(fileName = "ParticleSystem Pool", menuName = "Pool/ParticleSystem Pool")]
    public class ParticlePoolSO : ComponentPoolSO<ParticleSystem> 
    {
        [SerializeField] protected int _code;
        [SerializeField] private int _initCount;
        [SerializeField] private bool _alreadyLoad; //???? ????

        [SerializeField] private Stack<ParticleSystem> _usingStack = new Stack<ParticleSystem>();

        private bool _init;
        public bool AlreadyLoad => _alreadyLoad;
        public int Code => _code;

        protected virtual void OnEnable()
        {
            _init = false;
        }

        public override ParticleSystem Pop()
        {
            if (!_init)
            {
                _init = true;
                SetParent(EffectManager.Instance.transform);
                Prewarm(_initCount);
            }
            var go = base.Pop();
            EffectManager.Instance.StartCoroutine(Process(go));

            return go;
        }

        public ParticleSystem Pop(Vector3 pos , float size = 1 )
        {
            var go = Pop();
            go.transform.position = pos;
            go.transform.localScale = _prefab.transform.lossyScale * size;
            return go;
        }

        IEnumerator Process(ParticleSystem particle)
        {
            particle.Play();
            while (particle.isPlaying)
            {
                yield return null;
            }
            Push(particle);
        }




#if UNITY_EDITOR
        private void OnCheckExistCode()
        {
            var poolSOs = Find<ParticlePoolSO>();
            foreach(var poolSo in poolSOs)
            {
                //poolSo._code = 11;
                //Debug.Log(poolSo.Code + "dd");
            }
            
        }

        T[] Find<T>() where T : ScriptableObject
        {
            List<T> addList = new List<T>();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var photonEvent = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (photonEvent.GetType() == typeof(T))
                {
                    addList.Add(photonEvent);
                }
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
            return addList.ToArray();

        }
#endif
    }
}