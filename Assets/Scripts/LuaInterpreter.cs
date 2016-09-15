using System.IO;
using MoonSharp.Interpreter;

namespace Assets.Scripts
{
    public class LuaInterpreter
    {

        private Script script;

        /// <summary>
        /// Create a new lua interpreter instance for a given script.
        /// </summary>
        /// <param name="_luaCode"></param>
        public LuaInterpreter(string _luaCode)
        {
            script = new Script();
            script.DoString(_luaCode);
        }

        /// <summary>
        /// Runs the script assigned to this interpreter if possible and returns the result.
        /// </summary>
        /// <returns></returns>
        public DynValue Run()
        {
            var result = Script.RunString(script.GetSourceCode(1).Code);
            return result;
        }

        /// <summary>
        /// Call a specific function within the interpreters attatched script with optional parameters
        /// </summary>
        /// <param name="_func"></param>
        /// <param name="_args"></param>
        /// <returns></returns>
        public DynValue Call(string _func, params object[] _args)
        {
            var result = _args == null ? script.Call(script.Globals[_func]) : script.Call(script.Globals[_func], _args);
            return result;
        }
    }
}
