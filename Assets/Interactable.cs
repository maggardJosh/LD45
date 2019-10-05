using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool GhostInteractable = true;
    public Func<PlayerController, bool> interactAction { private get; set; }
    private GameObject interactIndicator;
    public bool canInteract = false;
    public bool oneTime = false;
    private int timesUsed = 0;
    public bool RequiresArms = false;
    public bool RequiresSkull = false;
    public bool RequiresLegs = false;

    void Awake()
    {
        interactIndicator = Instantiate(GameSettings.InteractIndicatorPrefab);
        interactIndicator.transform.parent = transform;
        interactIndicator.transform.localPosition = Vector3.zero;
        interactIndicator.transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        canInteract = GetCanInteract();
        interactIndicator.transform.GetChild(0).gameObject.SetActive(canInteract);
    }

    private bool GetCanInteract()
    {
        var pc = FindObjectOfType<PlayerController>();
        if (!GhostInteractable && pc.CurrentSetting == GameSettings.GhostSetting)
            return false; 

        if (RequiresArms && !pc.CurrentSetting.Torso)
            return false;

        if (RequiresLegs && !pc.CurrentSetting.Legs)
            return false;

        if (RequiresSkull && !pc.CurrentSetting.Head)
            return false;

        if (oneTime && timesUsed > 0)
            return false;

        var bc = interactIndicator.GetComponent<BoxCollider2D>();
        return bc.bounds.Intersects(pc.GetComponent<BoxCollider2D>().bounds);
    }

    public bool Use(PlayerController pc)
    {
        if (!canInteract)
            return false;

        if (!interactAction.Invoke(pc))
            return false;

        timesUsed++;
        return true;

    }
}
