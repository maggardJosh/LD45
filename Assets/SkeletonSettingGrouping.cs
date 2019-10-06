using UnityEngine;

[CreateAssetMenu(fileName = "EntitySettings", menuName = "Custom/Skeleton Setting Grouping")]
public class SkeletonSettingGrouping : ScriptableObject
{
    public bool Head = true;
    public bool Torso = false;
    public bool Legs = false;
    public bool Wings = false;

    public EntitySettings EntitySettings;
    public AnimatorOverrideController AnimatorOverrideController;
    public CharacterSettings CharacterSettings;

    public void ApplyToPlayer(PlayerController pc, SkeletonSettingGrouping lastSetting = null)
    {
        GetCombinedGroupWithPlayer(pc, lastSetting).ForceApplyToPlayer(pc);
    }

    public SkeletonSettingGrouping GetOverlapGroupWithPlayer(PlayerController pc, SkeletonSettingGrouping lastSetting = null)
    {
        lastSetting = lastSetting ?? pc.CurrentSetting;
        bool overlapHead = Head && (lastSetting?.Head ?? false);
        bool overlapTorso = Torso && (lastSetting?.Torso ?? false);
        bool overlapLegs = Legs && (lastSetting?.Legs ?? false);
        bool overlapWings = Wings && (lastSetting?.Wings ?? false);
        var resultGroup = FindGroupingSetting(overlapHead, overlapTorso, overlapLegs, overlapWings);
        return resultGroup;
    }

    public SkeletonSettingGrouping GetCombinedGroupWithPlayer(PlayerController pc, SkeletonSettingGrouping lastSetting = null)
    {
        lastSetting = lastSetting ?? pc.CurrentSetting;
        bool combinedHead = Head || (lastSetting?.Head ?? false);
        bool combinedTorso = Torso || (lastSetting?.Torso ?? false);
        bool combinedLegs = Legs || (lastSetting?.Legs ?? false);
        bool combinedWings = Wings || (lastSetting?.Wings ?? false);
        var resultGroup = FindGroupingSetting(combinedHead, combinedTorso, combinedLegs, combinedWings);
        return resultGroup;
    }

    public static SkeletonSettingGrouping FindGroupingSetting(bool head, bool torso, bool legs, bool wings)
    {
        foreach (SkeletonSettingGrouping setting in Resources.LoadAll<SkeletonSettingGrouping>("GroupSettings"))
        {
            if (setting.Head == head &&
                setting.Torso == torso &&
                setting.Legs == legs &&
                setting.Wings == wings)
            {
                return setting;
            }
        }
        return null;
    }

    public void ForceApplyToPlayer(PlayerController pc)
    {
        pc.GetComponent<Animator>().runtimeAnimatorController = AnimatorOverrideController;
        pc.GetComponent<BaseEntity>().Settings = EntitySettings;
        pc.SetBoxColliderHeight(CharacterSettings.ColliderHeight);

        pc.ApplySetting = this;
        pc.CurrentSetting = this;
    }

    internal int GetNumPowerups()
    {
        int numPowerups = 0;
        if (Head)
            numPowerups++;
        if (Torso)
            numPowerups++;
        if (Legs)
            numPowerups++;
        if (Wings)
            numPowerups++;
        return numPowerups;
    }
}