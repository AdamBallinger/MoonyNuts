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

        public void Start()
        {
            Script.DefaultOptions.DebugPrint = Debug.Log;
            LuaInterpreter.Create();

            RegisterObjectType(typeof(GameObject));
            RegisterObjectType(typeof(CharacterAPIController));
        }

        public void OnButtonClick()
        {
            var script = inputField.text;
            LuaInterpreter.Current.SetSourceCode(script);
            
            LuaInterpreter.Current.Script.Globals["GetCharacter"] = (Func<int, CharacterAPIController>) CharacterAPI.GetGameObject;
            LuaInterpreter.Current.Script.Globals["GetCharacterID"] = (Func<GameObject, int>) CharacterAPI.GetID;
            LuaInterpreter.Current.Script.Globals["duck"] = CharacterAPI.GetGameObject(0);
            LuaInterpreter.Current.Run();
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
