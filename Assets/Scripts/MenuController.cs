using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {

    [SerializeField] RectTransform shopPanel;


    public void StartClicked() {
        Application.LoadLevel("Main");
    }
    public void OpenShop(bool on) {
        if (on) IAPManager.Start();
        OpenPanel(shopPanel, on);
    }

    private void OpenPanel(RectTransform panel, bool open) {
        panel.gameObject.SetActive(open);
    }




    public void BuyTestItemClicked() {
        IAPManager.Purchase("testprod");
    }
}
