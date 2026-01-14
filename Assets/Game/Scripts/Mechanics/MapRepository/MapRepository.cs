using System.Collections.Generic;

public class MapRepository
{
    public List<BeatmapData> _maps;

    public BeatmapData GetMap(int id)
    {
        return _maps[id];
    }
}
