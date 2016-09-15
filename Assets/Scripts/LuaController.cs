using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LuaController : MonoBehaviour
    {

        public Button runButton;
        public InputField inputField;

        private LuaInterpreter interp;

        public void OnButtonClick()
        {
            var script = inputField.text;
            interp = new LuaInterpreter(script);
            Debug.Log(interp.Run().String);
        }
    }
}
