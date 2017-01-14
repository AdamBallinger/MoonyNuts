using System;
using Assets.Scripts.API;
using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LuaController : MonoBehaviour
    {

        public Button runButton;
        public InputField inputField;
        public InputField errorOutput;

        public void Start()
        {
            // Set the LUA print function to use the Unity Debug.Log function.
            Script.DefaultOptions.DebugPrint = Debug.Log;
            LuaInterpreter.Create();

            inputField.text = @"d2 = GetCharacter(1)
while true do
duck.MoveLeft()
duck.Speak()
duck.MoveUp()
duck.MoveRight()
duck.MoveDown()

d2.MoveRight()
d2.MoveUp()
d2.Speak()
d2.MoveLeft()
d2.MoveDown()
end
";

            // Register types for the LUA interpreter to understand and use.
            RegisterObjectType(typeof(GameObject));
            RegisterObjectType(typeof(CharacterAPIController));
        }

        /// <summary>
        /// Event triggered when the "Run Script" button is clicked. Sets the LUA globals and runs the LUA code in the script box.
        /// </summary>
        public void OnButtonClickRunScript()
        {
            var script = inputField.text;
            LuaInterpreter.Create();
            LuaInterpreter.Current.SetSourceCode(script);
            
            // Set the globals for the interpreter.
            LuaInterpreter.Current.Script.Globals["GetCharacter"] = (Func<int, CharacterAPIController>) CharacterAPI.GetGameObject;
            LuaInterpreter.Current.Script.Globals["GetCharacterID"] = (Func<GameObject, int>) CharacterAPI.GetID;
            LuaInterpreter.Current.Script.Globals["duck"] = CharacterAPI.GetGameObject(0);

            var scriptResult = LuaInterpreter.Current.Run();
            errorOutput.text = scriptResult;
        }

        public void OnButtonClickStopScript()
        {
            LuaInterpreter.Current.Terminate();
        }

        public void OnButtonClickCloseApplication()
        {
            LuaInterpreter.Current.Terminate();
            Application.Quit();
        }

        /// <summary>
        /// Registers a custom object type for use with the LUA interpreter.
        /// </summary>
        /// <param name="_type"></param>
        public void RegisterObjectType(Type _type)
        {
            UserData.RegisterType(_type);
        }
    }
}
