using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyNotif : MonoBehaviour
{
    [SerializeField] private RectTransform economyNotifLand;
    [SerializeField] private GameObject templatePrefab;
    public bool[] slotClaim;
    public int slotQueueNumber;
    public RectTransform[] slotRect;
    public static EconomyNotif Instance;
    public List<EconomyNotifData> ECdata;
    public struct EconomyNotifData
    {
        public string spriteNameIcon;
        public string notifName;
        public int quantity;
        public bool isAdding;
        public Sprite sprite;
        public bool isFlat;
        public EconomyNotifData(string spriteNameIcon, string notifName, int quantity, bool isAdding, Sprite sprite, bool isFlat)
        {
            this.spriteNameIcon = spriteNameIcon;
            this.notifName = notifName;
            this.quantity = quantity;
            this.isAdding = isAdding;
            this.sprite = sprite;
            this.isFlat = isFlat;
        }

    }
    private void Awake()
    {
        Instance = this;
        slotClaim = new bool[slotRect.Length];
        ECdata = new List<EconomyNotifData>();


        StartCoroutine(Run());
    }

    public void InstanceThePrefab()
    {
        GameObject prefab = Instantiate(templatePrefab, economyNotifLand, false);


    }
    public void Append(string spriteNameIcon, string notifName, int quantity, bool isAdding)
    {
        EconomyNotifData data = new EconomyNotifData(spriteNameIcon, notifName, quantity, isAdding, null, false);
        ECdata.Add(data);
    }
    public void Append(Sprite sprite, string notifName, int quantity, bool isAdding)
    {
        EconomyNotifData data = new EconomyNotifData(null, notifName, quantity, isAdding, sprite, false);
        ECdata.Add(data);
    }
    public void Append(string spriteNameIcon, string notifName)
    {
        EconomyNotifData data = new EconomyNotifData(spriteNameIcon, notifName, 1, true, null, true);
        ECdata.Add(data);
    }
    private IEnumerator Run()
    {
        while (true)
        {
            if (slotQueueNumber < slotClaim.Length && slotQueueNumber < ECdata.Count)
            {
                if (ECdata[slotQueueNumber].quantity < 1)
                {
                    ECdata.RemoveAt(slotQueueNumber);
                    yield return null;
                    continue;
                }
                GameObject prefab = Instantiate(templatePrefab, economyNotifLand, false);
                EconomyNotifCard ENC = prefab.GetComponent<EconomyNotifCard>();
                EconomyNotifData ENdata = ECdata[slotQueueNumber];
                ENC.thisRect.anchoredPosition = slotRect[slotQueueNumber].anchoredPosition;
                ENC.notifText.text =
                (ENdata.spriteNameIcon == null ? "" : "<sprite name=\"" + ENdata.spriteNameIcon + "\">")
                + " " + ENdata.notifName + " " +
                (ENdata.isFlat ? "" : (ENdata.isAdding ? "" : "<color=\"red\">") + (ENdata.isAdding ? "+" : "-")
                + " " + StoreData.Instance.MoneyString(ENdata.quantity));
                ENC.slotQueueNumber = slotQueueNumber;
                ENC.canvasGroup.alpha = 0;
                ENC.spriteImage.sprite = ENdata.sprite;
                slotClaim[slotQueueNumber] = true;
                slotQueueNumber++;
            }
            yield return null;
        }
    }
}
