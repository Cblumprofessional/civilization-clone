namespace biome;

public class NoiseRange
{
    public float min {get;set; }
    public float max {get;set; }
}

public class Biome
{
    public string name {get; set;} = "";
    public char symbol{get; set;} 
    public string color {get; set;} = "#ffffff";
    public NoiseRange ?elevation {get; set;}
    public NoiseRange ?moisture {get; set;}
    public NoiseRange ?temperature {get; set;}

    public static Biome? GetBiome(Tile tile, List<Biome> biomes)
    {
        foreach(Biome biome in biomes){
            bool elevationMatches = biome.elevation == null || tile.elevation >= biome.elevation.min && tile.elevation <= biome.elevation.max;
            bool moistureMatches = biome.moisture == null || tile.moisture >= biome.moisture.min && tile.moisture <= biome.moisture.max;
            bool temperatureMatches = biome.temperature == null || tile.temperature >= biome.temperature.min && tile.temperature <= biome.temperature.max;

            if(elevationMatches && moistureMatches && temperatureMatches)
            {
                return biome;
            };
        
    }
    return null;
    
}


}
public class Tile
    {
        public float elevation{get; set;}
        public float moisture{get; set;}
        public float temperature{get; set;}
        public Biome? Biome{get; set;}

}