using System;
using System.Collections;
using System.Collections.Generic;
using _Camera;
using UnityEngine;
using UnityEngine.UI;

public class G4_UICtrl : MonoBehaviour
{
    public static G4_UICtrl Instance;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button leftBtn, rightBtn, upBtn, downBtn;
    [SerializeField] private Button leftSwing, rightSwing, upSwing, downSwing;
    [Space(10)]
    [Header("Switch")] 
    [SerializeField] private GameObject moveContent;
    [SerializeField] private GameObject switchContent;
    [SerializeField] private Button switchBtn;
    [SerializeField] private Sprite moveSprite, swingSprite;
    [SerializeField] private Text switchText;
    private bool _switch = false;

    public static Action<Vector2Int>  OnMove;
    public static Action<Vector2Int>  OnSwing;
    
    private void Awake()
        {
            Instance = this;

            _canvas.worldCamera = CameraManager.Instance.GetUICamera();
            ResetButtonCtrl();
            switchBtn.onClick.AddListener(() =>
            {
                _switch = !_switch;
                moveContent.SetActive(!_switch);
                switchBtn.GetComponent<Image>().sprite = _switch ? moveSprite : swingSprite;
                switchText.text = _switch ? "Move" : "Swing";
                switchContent.SetActive(_switch);
            });

            MovementBtn();
            SwingBtn();
        }

    public void ResetButtonCtrl()
    {
        _switch = false;
        moveContent.SetActive(true);
        switchBtn.GetComponent<Image>().sprite = swingSprite;
        switchText.text = "Swing";
        switchContent.SetActive(false);
    }

        private void MovementBtn()
        {
            leftBtn.onClick.AddListener(()=>
            {
                OnMove?.Invoke(Vector2Int.left);
            });
            
            rightBtn.onClick.AddListener(()=>
            {
                OnMove?.Invoke(Vector2Int.right);
            });
            
            upBtn.onClick.AddListener(()=>
            {
                OnMove?.Invoke(Vector2Int.up);
            });
            
            downBtn.onClick.AddListener(()=>
            {
                OnMove?.Invoke(Vector2Int.down);
            });
        }
        
        private void SwingBtn()
        {
            leftSwing.onClick.AddListener(()=>
            {
                OnSwing?.Invoke(Vector2Int.left);
            });
            
            rightSwing.onClick.AddListener(()=>
            {
                OnSwing?.Invoke(Vector2Int.right);
            });
            
            upSwing.onClick.AddListener(()=>
            {
                OnSwing?.Invoke(Vector2Int.up);
            });
            
            downSwing.onClick.AddListener(()=>
            {
                OnSwing?.Invoke(Vector2Int.down);
            });
        }
}
