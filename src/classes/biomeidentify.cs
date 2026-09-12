using System.Runtime.CompilerServices;

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
    public bool passable {get; set;}
    public int weight {get; set;}
    public Yields yields{get; set;} = new Yields();
    public NoiseRange ?elevation {get; set;}
    public NoiseRange ?moisture {get; set;}
    public NoiseRange ?temperature {get; set;}

    public List<string> allowed_resources {get; set; } = new List<string>();
    public List<string> allowed_features {get; set; } = new List<string>();
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
        public int gold{get; set;}
        public int food {get; set;}
        public int production {get; set;}
        public string? resource {get; set;}
        public string? feature {get; set;}
        public Biome? Biome{get; set;}

}

public class Yields
{
    public int gold{get; set;}
    public int food{get; set;}
    public int production {get; set;}
}

public class Features
{
    public string? name {get; set;}
    public Yields yields {get; set;} = new Yields();
}

public class Resources
{
    public string? name {get; set;}
    public Yields yields {get; set;} = new Yields();

}


public class getBonuses
{
    private readonly List<Features> features;
    private readonly List<Resources> resources;

    public getBonuses(List<Features> features, List<Resources> resources)
    {
        this.features = features;
        this.resources = resources;
    }
    public Features? GetFeature(Biome biome)
    {
        if(Random.Shared.NextDouble() < .2)
        {
            string featureName = biome.allowed_features[Random.Shared.Next(biome.allowed_features.Count)];
            return features.FirstOrDefault(
                f => f.name == featureName
            );    
        }
        return null;
    }

    public Resources? GetResource(Biome biome)
    {
        if(Random.Shared.NextDouble() < .2)
        {
            string resourceName = biome.allowed_resources[Random.Shared.Next(biome.allowed_resources.Count)];
            return resources.FirstOrDefault(
                f => f.name == resourceName
            );    
        }
        return null;
    }
}