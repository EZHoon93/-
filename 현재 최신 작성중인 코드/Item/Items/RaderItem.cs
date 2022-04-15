using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaderItem : ObtainItemBase
{
    [SerializeField] GameObject _prefab;
    public void Enter(PlayerController enterPlayer, Collider collider)
    {
    }

    public override void Use(int playerViewID, Vector3 inputVector)
    {
    }

   

    //[SerializeField] GameObject _raderObeject;
    //[SerializeField] GameObject _isFindObject;
    //[SerializeField] float _durationTime;
    //[SerializeField] float _speed;

    //protected override void Init()
    //{
    //    _raderObeject.gameObject.SetActive(false);
    //}
    //protected override void CallBackDown(Vector3 inputVector3)
    //{
    //    //_inputControllerObject.RemoveToPlayer();
    //    //Use();
    //}
    //protected override void Use()
    //{
    //    StartCoroutine(Process());
    //}

    //IEnumerator Process()
    //{
    //    float _remainTime = _durationTime;
    //    _raderObeject.gameObject.SetActive(true);
    //    while (_remainTime > 0)
    //    {
    //        this.transform.position = _playerController.transform.position;
    //        _remainTime -= Time.deltaTime;
    //        Vector3 v3 = this._raderObeject.transform.localScale;
    //        float temp = _speed * Time.deltaTime;
    //        this._raderObeject.transform.localScale = new Vector3(v3.x + temp, v3.y + temp, v3.z + temp);
    //        yield return null;
    //    }
    //    _raderObeject.gameObject.SetActive(false);
    //    Managers.Resource.PunDestroy(this);
    //}

    //public void Enter(PlayerController enterPlayer, Collider collider)
    //{
    //    _isFindObject.gameObject.SetActive(true);
    //}
    //protected override void Use()
    //{
    //    print("Use!!)");
    //    GameObject game_obj_scanner = Instantiate(_prefab);
    //    game_obj_scanner.transform.position = _playerController.transform.position;
    //}
}
