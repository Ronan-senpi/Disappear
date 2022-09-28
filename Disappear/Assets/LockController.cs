
using UnityEngine;
public class LockController : MonoBehaviour
{
 
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        PlayerController.MainPlayer.CanMoveOrRotate = false;
    }

    private void OnDisable()
    {
        if (PlayerController.MainPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerController.MainPlayer.CanMoveOrRotate = true;
        }
    }
}