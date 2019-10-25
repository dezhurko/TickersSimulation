using TMPro;
using UnityEngine;

public class MsgView : MonoBehaviour
{
    private Msg msg;
    
    [SerializeField]
    private TextMeshPro text;


    public void Setup(Msg msg)
    {
        this.msg = msg;

        text.text = msg.Tick.ToString();
    }
}
