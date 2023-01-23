using Model;
using TMPro;
using UnityEngine;

namespace View
{
    public class MsgView : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshPro text;

        public Msg Msg { get; private set; }

        public void Setup(Msg msg)
        {
            this.Msg = msg;

            text.text = msg.Tick.ToString();
        }
    }
}