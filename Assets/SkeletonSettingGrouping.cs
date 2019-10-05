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

    public void ApplyToPlayer(PlayerController pc, SkeletonSettingGrouping lastSetting)
    {
        bool combinedHead = Head || (lastSetting?.Head ?? false);
        bool combinedTorso = Torso || (lastSetting?.Torso ?? false);
        bool combinedLegs = Legs || (lastSetting?.Legs ?? false);
        bool combinedWings = Wings || (lastSetting?.Wings ?? false);
        FindGroupingSetting(combinedHead, combinedTorso, combinedLegs, combinedWings).ForceApplyToPlayer(pc);
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
}