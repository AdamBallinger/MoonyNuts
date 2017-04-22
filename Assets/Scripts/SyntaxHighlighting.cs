using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SyntaxHighlighting : MonoBehaviour
    {
        public InputField luaInput;

        public void OnLuaInputChanged()
        {
            Debug.Log("SCANNING SYTNAX");
            var lines = Regex.Split(luaInput.text, @"\r\n|\r|\n").ToList();

            //foreach(var line in lines)
            //{
            //    if(line.Trim().StartsWith("--"))
            //    {
            //        lines[lines.IndexOf(line)] = line.Insert(0, "<color=2FFFA0FF>");
            //        lines[lines.IndexOf(line)] = line.Insert(line.Length - 1, "</color>");
            //    }
            //}

            for(var i = 0; i < lines.Count; i++)
            {
                if(lines[i].Trim().StartsWith("--"))
                {
                    lines[i] = lines[i].Insert(0, "<color=#2FFFA0FF>");
                    lines[i] = lines[i].Insert(lines[i].Length, "</color>");
                }
            }

            // TODO: This wont work because of how rich text input field work :(
            //luaInput.text = string.Join("\n", lines.ToArray());
        }
    }
}
