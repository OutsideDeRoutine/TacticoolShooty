using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] float mouseSensitivity;
    [SerializeField] bool freeView = true; //Vista libre
    [SerializeField] float verticalAltLimit, horizontalAltLimit; //Limites de la vista libre

    private bool resetingRotation = false;
    private Quaternion startRotation;
    private Quaternion resetRotation;
    private Quaternion localResetRotation;

    void Start()
    {
       Cursor.lockState = CursorLockMode.Locked;
        startRotation = transform.parent.localRotation;
        resetRotation = transform.parent.rotation;
        localResetRotation = startRotation;
    }
    //SACAR A UN CONTROLADOR EXTERNO
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyDown(KeyCode.LeftAlt)) SetFreeView(!freeView);
    }

    void LateUpdate () {
        var h = Input.GetAxis("Mouse X");
        var v = Input.GetAxis("Mouse Y");

        if (freeView) FreeLook(h, v);
        else if (!resetingRotation) Look(h, v);
        else ResetRotation();
    }

    // LIMITAR
    void FreeLook(float h, float v)
    {
        transform.parent.rotation = resetRotation;
        //Vertical 
        LookVertical(v);

        //Horizontal
        transform.parent.RotateAround(transform.parent.position, transform.up, h * mouseSensitivity);

        localResetRotation = transform.parent.localRotation;
        resetRotation = transform.parent.rotation;
    }

    void Look(float h, float v)
    {
        transform.parent.rotation = resetRotation;
        //Vertical 
        LookVertical(v);
       
        //Horizontal
        player.transform.RotateAround(player.transform.position, Vector3.up, h * mouseSensitivity);
        localResetRotation = transform.parent.localRotation;
        resetRotation = transform.parent.rotation;
    }

    // LIMITAR
    void LookVertical(float v)
    {
        
        transform.parent.RotateAround(transform.parent.position, player.transform.up, v * mouseSensitivity);
        
    }

    void ResetRotation()
    {
       
        transform.parent.localRotation = Quaternion.Lerp(localResetRotation, startRotation,0.2f* mouseSensitivity);
        localResetRotation = transform.parent.localRotation;
        if (Mathf.Abs( Quaternion.Angle(transform.parent.localRotation , startRotation) )<5f)
        {
            transform.parent.localRotation = startRotation;
            resetingRotation = false;
            resetRotation = transform.parent.rotation;
        }
    }

    public void SetFreeView(bool free)
    {
        freeView = free;
        if(!free) resetingRotation = true;
    }
}
