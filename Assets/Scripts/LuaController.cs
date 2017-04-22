using System;
using System.Linq;
using System.Text.RegularExpressions;
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

        public GameObject runningImageGO;
        public GameObject stoppedImageGO;

        [Space(2)]
        [Header("Script Requirements")]
        public bool lineLimit = false;
        public int maxLines = 0;

        [Space(2)]
        [Header("Default Script Values")]

        public bool setDefaultText = true;

        [Multiline]
        [TextArea(4, 30)]
        public string inputDefaultText = string.Empty;

        public void Start()
        {
            // Set the Lua print function to use the Unity Debug.Log function.
            Script.DefaultOptions.DebugPrint = Debug.Log;
            LuaInterpreter.Create();

            if(setDefaultText)
                inputField.text = inputDefaultText;

            // Register types for the Lua interpreter to understand and use.
            RegisterObjectType(typeof(GameObject));
            RegisterObjectType(typeof(CharacterAPIController));
        }

        /// <summary>
        /// Event triggered when the "Run Script" button is clicked. Sets the Lua globals and runs the Lua code in the script box.
        /// </summary>
        public void OnButtonClickRunScript()
        {
            var script = inputField.text;

            if (lineLimit && !ScriptIsValid(script))
            {
                errorOutput.text = "Your script has exceeded the maximum number of lines allowed! You must not exceed " + maxLines + " lines.";
                return;
            }

            LuaInterpreter.Create();
            LuaInterpreter.Current.SetSourceCode(script);
            
            // Set the globals for the interpreter.
            LuaInterpreter.Current.Script.Globals["GetCharacter"] = (Func<int, CharacterAPIController>) CharacterAPI.GetGameObject;
            LuaInterpreter.Current.Script.Globals["GetCharacterID"] = (Func<GameObject, int>) CharacterAPI.GetID;
            LuaInterpreter.Current.Script.Globals["player"] = CharacterAPI.GetGameObject(0);

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
        /// Registers a custom object type for use with the Lua interpreter.
        /// </summary>
        /// <param name="_type"></param>
        public void RegisterObjectType(Type _type)
        {
            UserData.RegisterType(_type);
        }

        private void Update()
        {
            if(LuaInterpreter.Current != null)
            {
                runningImageGO.SetActive(LuaInterpreter.Current.IsRunning);
                stoppedImageGO.SetActive(!LuaInterpreter.Current.IsRunning);
            }
        }

        /// <summary>
        /// Checks if the script meets levels requirements.
        /// </summary>
        /// <param name="_script"></param>
        /// <returns></returns>
        private bool ScriptIsValid(string _script)
        {
            var lines = Regex.Split(_script, @"\r\n|\r|\n");

            var lineCount = lines.Where(line => !line.Trim().StartsWith("--")).Count(line => line.Trim().Length > 0);

            Debug.Log("Script is " + lineCount + " lines!");

            return lineCount <= maxLines;
        }
    }
}
