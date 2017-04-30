using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameCode
{
    public enum Tag
    {
        Passable = 0,
        UnPassable = 1,
    }

    public static string GetTag(int index)
    {
        string[] tag = { "Passable", "UnPassable" };
        return tag[index];
    }
    
}
