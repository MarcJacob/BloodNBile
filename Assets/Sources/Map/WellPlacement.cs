using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WellPlacement
{
    public SerializableVector3 Position;
    public HumorLevels Humors;
    public WellPlacement(SerializableVector3 pos, HumorLevels humors)
    {
        Position = pos;
        Humors = humors;
    }
}