using fastnoise;
using biome;




class GenerateWorld
{
    int width = 128;
    int height = 128;
    private int seed = Random.Shared.Next();
    private FastNoiseLite Elevation = new FastNoiseLite();
    private FastNoiseLite Moisture = new FastNoiseLite();
    private FastNoiseLite Temperature = new FastNoiseLite();
    public Tile[,] World { get; private set; }

    public GenerateWorld(List<Biome> biomes, List<Features> features, List<Resources> resources){
        Elevation.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        Moisture.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        Temperature.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        Elevation.SetSeed(seed); 
        Moisture.SetSeed(seed+1); 
        Temperature.SetSeed(seed+2); 

        getBonuses bonuses = new getBonuses(features, resources);

        World = new Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = new Tile
                {
                    elevation = Elevation.GetNoise(x,y),
                    moisture = Moisture.GetNoise(x,y),
                    temperature = Temperature.GetNoise(x,y),

                };
                tile.Biome = Biome.GetBiome(tile, biomes);
                if(tile.Biome != null)
                {
                    Features? feature = bonuses.GetFeature(tile.Biome);
                    Resources? resource = bonuses.GetResource(tile.Biome);

                    tile.feature = feature?.name;
                    tile.resource = resource?.name;

                    tile.gold =
                        tile.Biome.yields.gold +
                        (feature?.yields.gold ?? 0) +
                        (resource?.yields.gold ?? 0);

                    tile.food =
                        tile.Biome.yields.food +
                        (feature?.yields.food ?? 0) +
                        (resource?.yields.food ?? 0);

                    tile.production =
                        tile.Biome.yields.production +
                        (feature?.yields.production ?? 0) +
                        (resource?.yields.production ?? 0);

                }
                World[x, y] = tile;


            }

        }


    }
    

}