using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public Skill[] skillTable;  

    [Header("Player Slot")]
    public int _attackIndex = 0; 
    public int _defendIndex = 1; 
    public int _utilityIndex = 2;
    public void UseSkill(int index)
    {
        var skill = skillTable[index];
        skill.TryActivate();
    }

    public void UseAttack() => UseSkill(_attackIndex);
    public void UseDefend() => UseSkill(_defendIndex);
    public void UseUtility() => UseSkill(_utilityIndex);
}
