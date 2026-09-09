using fastnoise;
using biome;



class GenerateWorld
{
    int width = 500;
    int height = 500;
    private int seed = Random.Shared.Next();
    private FastNoiseLite Elevation = new FastNoiseLite();
    private FastNoiseLite Moisture = new FastNoiseLite();
    private FastNoiseLite Temperature = new FastNoiseLite();
    public Tile[,] World { get; private set; }

    public GenerateWorld(List<Biome> biomes){
        Elevation.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        Moisture.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        Temperature.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        Elevation.SetSeed(seed); 
        Moisture.SetSeed(seed+1); 
        Temperature.SetSeed(seed+2); 

        World = new Tile[width, height];
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                Tile tile = new Tile
                {
                    elevation = Elevation.GetNoise(x,y),
                    moisture = Moisture.GetNoise(x,y),
                    temperature = Temperature.GetNoise(x,y),

                };
                tile.Biome = Biome.GetBiome(tile, biomes);

                World[x, y] = tile;


            }

        }


    }
    

}