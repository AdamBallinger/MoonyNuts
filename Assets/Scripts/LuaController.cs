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
            // Run the entire script
            //Debug.Log(interp.Run().String);
            // Passing param to lua function "Test"
            //Debug.Log(interp.Call("Test", new object[] { "Hello" }));
            // Running the function called "Test"
            //Debug.Log(interp.Call("Test", "Jamie Likes Ducks").String);
            interp.RegisterGlobalsCallback<string>(Test, "Test");
            interp.BuildScript();
            interp.Run();
        }

        private void Test(string _str)
        {
            Debug.Log(_str);
        }
    }
}
