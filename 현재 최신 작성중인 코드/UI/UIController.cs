using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIControllerInfo
{
    public float x;
    public float y;
    public float size;
}
public class UIController : MonoBehaviour
{

    #region Varaibles
    [SerializeField] private UltimateJoystick _ultimateJoystick;
    [SerializeField] private Image _itemImage;
    [SerializeField] private InputInfoSO _inputInfoSO;

    public Vector3 _lastInputVector3;
    #endregion
    #region Properties
    #endregion

    #region Life Cycle
    private void Awake()
    {
        _ultimateJoystick.OnPointerDownCallback -= Down;
        _ultimateJoystick.OnPointerDownCallback += Down;
        _ultimateJoystick.OnDragCallback -= Drag;
        _ultimateJoystick.OnDragCallback += Drag;
        _ultimateJoystick.OnPointerUpCallback -= Up;
        _ultimateJoystick.OnPointerUpCallback += Up;
        _inputInfoSO.Setup(this);
    }
    #endregion

    #region public

    public void SetupItem(ItemSO itemSO)
    {
        if (itemSO == null)
            return;
        if (itemSO.ItemSprite != null)
        {
            _itemImage.sprite = itemSO.ItemSprite;
            _itemImage.enabled = true;
        }
        else
        {
            _itemImage.enabled = false;
        }

        if(itemSO.uIType == InputDefine.UIType.Joystick)
        {
            _itemImage.transform.ResetTransform(_ultimateJoystick.joystick);
            _ultimateJoystick.joystick.gameObject.SetActive(true);
        }
        else
        {
            _itemImage.transform.ResetTransform(_ultimateJoystick.joystick);
            _ultimateJoystick.joystick.gameObject.SetActive(true);
            //_itemImage.transform.ResetTransform(_ultimateJoystick.joystickBase);
            //_ultimateJoystick.joystick.gameObject.SetActive(false);
        }
    }

    #endregion

    #region CallBack

    private void Down()
    {
        CheckTouchType(InputDefine.InputTouchType.Down);
    }
    private void Drag()
    {
        CheckTouchType(InputDefine.InputTouchType.Drag);
    }
    private void Up()
    {
        CheckTouchType(InputDefine.InputTouchType.Up);
    }

    #endregion

    #region private


    private void CheckTouchType(InputDefine.InputTouchType inputTouchType)
    {
        if (_inputInfoSO == null)
        {
            return;
        }
        var inputVector3 = GetInputVector3();
        switch (inputTouchType)
        {
            case InputDefine.InputTouchType.Down:
                _inputInfoSO.OnDown(inputVector3);
                break;
            case InputDefine.InputTouchType.Drag:
                _lastInputVector3 = inputVector3;
                _inputInfoSO.OnDrag(inputVector3);

                break;
            case InputDefine.InputTouchType.Up:
                _inputInfoSO.OnUp(_lastInputVector3);
                break;
        }
        _inputInfoSO.inputVector3 = inputVector3;
    }

    private Vector3 GetInputVector3()
    {
        var inputVector3 = new Vector3(_ultimateJoystick.GetHorizontalAxis(), 0, _ultimateJoystick.GetVerticalAxis());
        var quaternion = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        return quaternion * inputVector3;
    }

    #endregion








}
