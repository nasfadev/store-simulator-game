using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trash : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private Canvas barCanvas;
    [SerializeField] private Image barFillImage;
    [SerializeField] private RayPointerAble rayPointerAble;
    [SerializeField] private AudioSource audioSource;
    [Header("Preview")]
    public int id;
    public Type type;

    private bool isDelete;
    public enum Type
    {
        GrabAble,
        ToolAble
    }
    public void Delete()
    {
        if (!isDelete)
        {
            barCanvas.gameObject.SetActive(true);
            rayPointerAble.mode = RayPointerAble.Type.Ignore;
            isDelete = true;

            StartCoroutine(DeleteIE());
        }
    }
    private IEnumerator DeleteIE()
    {

        while (true)
        {
            barFillImage.fillAmount += StoreData.Instance.data.trashDIfficulty * Time.deltaTime;
            if (barFillImage.fillAmount == 1f)
            {
                break;
            }
            yield return null;
        }
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        TrashGenerator.Instance.Remove(this);
    }

}
