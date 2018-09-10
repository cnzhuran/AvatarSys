using UnityEngine;
using Assembly;
using UnityEngine.UI;

public class AvatarManager : InstanceBase<AvatarManager>
{
    private string[] _configs = new string[6]
        {
            "skeleton",
            "head_1",
            "chest_1",
            "hand_1",
            "feet_1",
            "weapon_1"
        };

    private DiyCharacterController _characterInst = null;


    internal void EquipmentChange(string name, int index, Transform trans)
    {
        Debug.Log("  name = " + name + "     index = " + index);
        switch (name)
        {
            case "Head":
                _configs[1] = "head_" + index;
                break;

            case "Chest":
                _configs[2] = "chest_" + index;
                break;

            case "Hand":
                _configs[3] = "hand_" + index;
                break;

            case "Feet":
                _configs[4] = "feet_" + index;
                break;

            case "Weapon":
                _configs[5] = "weapon_" + index;
                break;
        }

        CharacterMgr.ChangeSkin(_characterInst, _configs, true);

        Transform transText = trans.Find("Text");
        Text text = transText.GetComponent<Text>();
        text.text = "√";
    }

    internal void GenerateGameObject()
    {
        _characterInst = CharacterMgr.Generate(_configs, true);
    }

}