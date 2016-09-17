using System;
using System.Reflection;
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

        private LuaInterpreter interp;

        public void Start()
        {
            Script.DefaultOptions.DebugPrint = Debug.Log;

            RegisterObjectType(typeof(GameObject));
            RegisterObjectType(typeof(CharacterAPIController));
        }

        public void OnButtonClick()
        {
            var script = inputField.text;
            interp = new LuaInterpreter(script);

            interp.Script.Globals["GetCharacter"] = (Func<int, CharacterAPIController>) CharacterAPI.GetGameObject;
            interp.Script.Globals["GetID"] = (Func<GameObject, int>) CharacterAPI.GetID;
            interp.BuildScript();
            interp.Run();
        }

        /// <summary>
        /// Registers a custom object type for use with the LUA interpreter.
        /// </summary>
        /// <param name="_type"></param>
        public void RegisterObjectType(Type _type)
        {
            UserData.RegisterType(_type);
        }

        //private void Test(string _str)
        //{
        //    Debug.Log(_str);
        //}
    }
}
