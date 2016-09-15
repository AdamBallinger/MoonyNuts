using System.IO;
using MoonSharp.Interpreter;

namespace Assets.Scripts
{
    public class LuaInterpreter
    {

        private Script script;

        public LuaInterpreter(string _luaCode)
        {
            script = new Script();
            script.DoString(_luaCode);
        }

        public DynValue Run()
        {
            var result = Script.RunString(script.GetSourceCode(1).Code);
            return result;
        }
    }
}
