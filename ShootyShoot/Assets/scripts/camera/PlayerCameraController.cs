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

    void Start()
    {
       Cursor.lockState = CursorLockMode.Locked;
        startRotation = transform.parent.rotation;
        resetRotation = startRotation;
    }
    //SACAR A UN CONTROLADOR EXTERNO
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyDown(KeyCode.LeftAlt)) SetFreeView(!freeView);
    }

    void LateUpdate () {
        var h = Input.GetAxis("Mouse X");
        var v = -Input.GetAxis("Mouse Y");

        if (freeView) FreeLook(h, v);
        else if (!resetingRotation) Look(h, v);
        else ResetRotation();
    }

    // NORMALIZAR
    // LIMITAR
    void FreeLook(float h, float v)
    {
        transform.parent.rotation = resetRotation;
        //Vertical 
        LookVertical(v);

        //Horizontal
        transform.parent.RotateAround(transform.parent.position, Vector3.up, h * mouseSensitivity);


        resetRotation = transform.parent.rotation;
    }

    void Look(float h, float v)
    {
        //Vertical 
        LookVertical(v);

        //Horizontal
        player.transform.RotateAround(player.transform.position, Vector3.up, h * mouseSensitivity);
    }

    // NORMALIZAR
    // LIMITAR
    void LookVertical(float v)
    {
        
        transform.parent.RotateAround(transform.parent.position, Vector3.forward, v * mouseSensitivity);
        
    }

    // NO FUNCIONA
    void ResetRotation()
    {
        Debug.Log(Quaternion.Angle(transform.parent.rotation, startRotation));
        transform.parent.rotation= Quaternion.Lerp(transform.parent.rotation, startRotation, 0.01f * mouseSensitivity);
        if (Mathf.Abs(Quaternion.Angle(transform.parent.rotation, startRotation)) < 3f)
        {
            resetingRotation = false;
            resetRotation = startRotation;
        }
    }

    public void SetFreeView(bool free)
    {
        freeView = free;
        if(!free) resetingRotation = true;
    }
}
