using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JackalWebHost.Models;

public class TileChange
{
    public string? BackgroundImageSrc;
    public string BackgroundColor;
    public int Rotate;

    public bool IsUnknown;

    public LevelChange[] Levels;

    public int X;
    public int Y;
}