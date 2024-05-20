using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CanvasButtonInitializer : MonoBehaviour
{
    [SerializeField] HeroController controller;
    [SerializeField] CameraController cameraController;
    [SerializeField] Wallet wallet;
    [SerializeField] PhotonView PV;
    void Start()
    {
        if (!PV.IsMine) return;

        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        for (int i = 0; i < canvas.childCount; i++)
        {
            Transform child = canvas.GetChild(i);
            switch (child.name)
            {
                case "CameraRotate":
                    CameraRotate cameraRotate = child.GetComponent<CameraRotate>();
                    cameraRotate.SetCameraRoort(cameraController.transform);
                    cameraRotate.SetPhotonView(PV);
                    break;
                case "JumpButton":
                    child.GetComponent<Button>().onClick.AddListener(controller.StartJumpAnimation);
                    break;
                case "AttackButton":
                    child.GetComponent<Button>().onClick.AddListener(controller.StartAttackAnimation);
                    break;
                case "ThrowButton":
                    child.GetComponent<Button>().onClick.AddListener(controller.StartThrowAnimation);
                    break;
                case "ExplosionButton":
                    child.GetComponent<Button>().onClick.AddListener(controller.Explode);
                    break;     
                case "MoneyViewer":
                    if (wallet.HasMoneyViewer())
                        return;
                    wallet.SetMoneyViewer(child.GetComponent<MoneyViewer>());
                    break;
            }
        }
    }
}