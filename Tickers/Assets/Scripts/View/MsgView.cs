using TMPro;
using UnityEngine;

public class MsgView : MonoBehaviour
{
    public Msg Msg { get; private set; }

    [SerializeField]
    private TextMeshPro text;


    public void Setup(Msg msg)
    {
        this.Msg = msg;

        text.text = msg.Tick.ToString();
    }
}
