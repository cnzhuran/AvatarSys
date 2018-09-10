using UnityEngine;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour
{

    public Transform[] transCol;
    private string[] names = { "Weapon", "Head", "Chest", "Hand", "Feet" };

    //private delegate void OnButtonClick(string name, int index);
    //private event OnButtonClick ButtonClick;
    //private Action<string, int> ButtonClickAction;

	// Use this for initialization
	void Start ()
    {
        int i = 0;
        for (; i < transCol.Length; )
        {
            Transform trans = transCol[i];
            InitColBtn(trans, i + 1);
            ++i;
        }


        AvatarManager.Instance.GenerateGameObject();
    }

    private void InitColBtn(Transform trans, int colIndex)
    {
        foreach (string name in names)
        {
            InitSingleBtn(trans, name, colIndex);
        }
    }

    private void InitSingleBtn(Transform trans, string name, int colIndex)
    {
        Transform transBtn = trans.Find(name);
        Button button = transBtn.GetComponent<Button>();
        button.onClick.AddListener(
            delegate()
            {
                AvatarManager.Instance.EquipmentChange(name, colIndex, transBtn);
            });
    }

}
