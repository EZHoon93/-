
using Photon.Pun;

using UnityEngine;
using UnityEngine.UI;

public class ObtainableItemSlider : MonoBehaviour 
{ 
    [SerializeField] Slider _slider;
    //ObtainableItemController _obtainableItemController;
  
    //private void Awake()
    //{
    //    _obtainableItemController =  this.transform.parent.GetComponent<ObtainableItemController>();
    //}

    //public void OnPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    _slider.value = 0;
    //    _slider.maxValue = _obtainableItemController.GetNeedTime;
    //}

    //private void Update()
    //{
    //    _slider.value = _obtainableItemController.CurrentGettingTime;
    //}
}
