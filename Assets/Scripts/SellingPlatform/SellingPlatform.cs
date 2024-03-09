using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SellingPlatform : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private Canvas barCanvas;
    [SerializeField] private Image barFillImage;
    [SerializeField] private RayPointerAble rayPointerAble;
    [SerializeField] private AudioSource audioSource;
    [Header("Preview")]
    public int index;
    public int chunkID;
    public int IDsp;
    public int maxStockQuantity;
    public SellingPlatformStockManager stockManager;
    public bool isFill;


    public void FillStock()
    {
        if (!isFill)
        {
            barCanvas.gameObject.SetActive(true);
            rayPointerAble.mode = RayPointerAble.Type.Ignore;
            isFill = true;

            StartCoroutine(FillStockIE());
        }
    }
    private IEnumerator FillStockIE()
    {

        while (true)
        {
            barFillImage.fillAmount += StoreData.Instance.data.fillShelvesDIfficulty * Time.deltaTime;
            if (barFillImage.fillAmount == 1f)
            {
                break;
            }
            yield return null;
        }
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        barCanvas.gameObject.SetActive(false);
        rayPointerAble.mode = RayPointerAble.Type.SellingPlatform;
        barFillImage.fillAmount = 0f;
        stockManager.FillStock();
        isFill = false;
    }

}
