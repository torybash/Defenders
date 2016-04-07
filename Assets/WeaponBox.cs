using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponBox : MonoBehaviour {

    [SerializeField] Image reloadImg;
    [SerializeField] Image typeImg;
    [SerializeField] Text countText;

    bool isReloading = false;

    public void UpdateCount(int count) {
        countText.text = "" + count;
    }

    public void StartReload(float duration) {
        if (isReloading) return;
        StartCoroutine(ReloadCR(duration));
    }

    private IEnumerator ReloadCR(float duration) {
        isReloading = true;
        reloadImg.enabled = true;
        float endTime = Time.time + duration;
        while (Time.time < endTime) {
            reloadImg.fillAmount = (endTime - Time.time) / duration; // 1 - 0
            yield return new WaitForEndOfFrame();
        }
        reloadImg.enabled = false;
        isReloading = false;
    }
}
