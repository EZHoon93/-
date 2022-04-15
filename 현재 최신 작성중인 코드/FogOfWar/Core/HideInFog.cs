using System.Linq;

using UnityEngine;

namespace FoW
{
    [AddComponentMenu("FogOfWar/HideInFog")]
    public class HideInFog : MonoBehaviour
    {
        [SerializeField]
        private FogTeamEventChannelSO _fogTeamEventChannelSO;
        [Range(0.0f, 1.0f)]
        public float minFogStrength = 0.2f;
        [SerializeField] Renderer[] _renderers;
        [SerializeField] Canvas _canvas;

        //public Renderer[] Renderes => _renderers;

        private void Reset()
        {
            _renderers = GetComponentsInChildren<Renderer>();
            _canvas = GetComponent<Canvas>();
        }
        private void OnEnable()
        {
            minFogStrength = 1;
            _fogTeamEventChannelSO.onEventRaised += UpdateInFog;
            
        }
        private void OnDisable()
        {
            _fogTeamEventChannelSO.onEventRaised -= UpdateInFog;
        }
        public void Clear()
        {
            //_renderers.
        }
        public void SetActiveRender(bool visible)
        {
            if (_canvas != null)
                _canvas.enabled = visible;
            foreach (var render in _renderers)
                render.enabled = visible;
        }
        public void AddRender(GameObject gameObject )
        {
            var list = _renderers.ToList();
            foreach(var r in gameObject.GetComponentsInChildren<Renderer>())
            {
                list.Add(r);
            }

            _renderers = list.ToArray();
        }

        public void AddRenderer(Renderer renderer)
        {
            var list = _renderers.ToList();
            list.Add(renderer);
            _renderers = list.ToArray();
        }

        public void UpdateInFog(FogOfWarTeam fow )
        {
            if (fow == null)
            {
                Debug.LogWarning("There is no Fog Of War team for team #" + fow.team.ToString());
                return;
            }
            bool visible = fow.GetFogValue(this.transform.position) < minFogStrength * 255;
            SetActiveRender(visible);
        }
    }
}
