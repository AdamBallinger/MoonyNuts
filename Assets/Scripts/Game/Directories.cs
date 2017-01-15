
using System.IO;
using System.Net.Mime;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Directories
    {
        public static readonly string Save_Directory = Path.Combine(Application.streamingAssetsPath, "Levels");
        
        public static void Check()
        {
            if(!Directory.Exists(Save_Directory))
            {
                Directory.CreateDirectory(Save_Directory);
            }
        }
    }
}
