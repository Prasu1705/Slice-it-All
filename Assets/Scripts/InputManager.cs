using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public Knife knife;

    public delegate void onClickEvent();
    public event onClickEvent onClick;

    public bool isClicked;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isClicked = true;
            knife.rb.isKinematic = false;
            knife.isGrounded = false;
            Click();
        }
    }
    public void Click()
    {
        if(onClick!=null)
        {
            
            onClick?.Invoke();
        }
    }
}
