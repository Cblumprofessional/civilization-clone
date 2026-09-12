using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using biome;
using squad;

bool debug = false;
if (args.Contains("-1"))
{
    debug = true;
}

string? testFile = @"./src/log.txt";
string? unknown = @"./src/unknown.txt";
string tileInfoFile = @"./src/tileinfo.txt";

int width = 128;
int height = 128;

var builder = WebApplication.CreateBuilder(args);

string biomeJson = File.ReadAllText("./src/json/biomes.json");
List<Biome>? biomes = JsonSerializer.Deserialize<List<Biome>>(biomeJson);

string featuresJson = File.ReadAllText("./src/json/features.json");
List<Features>? features = JsonSerializer.Deserialize<List<Features>>(featuresJson);

string resourcesJson = File.ReadAllText("./src/json/resources.json");
List<Resources>? resources = JsonSerializer.Deserialize<List<Resources>>(resourcesJson);
GenerateWorld generator = new GenerateWorld(biomes, features, resources);

Squad squad = new Squad{troopCount = 1, x = 50, y = 50};



if(debug){

    foreach (Biome biome in biomes)
    {
        
        Console.WriteLine(
            $"{biome.name} | {biome.symbol} | " +
            $"E: {biome.elevation?.min} to {biome.elevation?.max} | " +
            $"M: {biome.moisture?.min} to {biome.moisture?.max} | " +
            $"T: {biome.temperature?.min} to {biome.temperature?.max} | " +
            $"P:{biome.passable} | " +
            $"W:{biome.weight}"
        );
    }

    Console.WriteLine($"C: {squad.troopCount}");
    Console.WriteLine($"X: {squad.x}");
    Console.WriteLine($"Y: {squad.y}");

    

    
    using (StreamWriter writer = new StreamWriter(testFile, false))
    using (StreamWriter ukfile = new StreamWriter(unknown, false))



     for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                {
                    Tile tile = generator.World[i, j];
                    writer.Write(generator.World[i, j].Biome?.symbol ?? '?');
                    if (tile.Biome == null)
                    {
                        ukfile.Write(
                            $"X:{i} Y:{j} | " +
                            $"E:{tile.elevation:F3} " +
                            $"M:{tile.moisture:F3} " +
                            $"T:{tile.temperature:F3}"
                           
                        );
                    }

                }
                
            }
            writer.WriteLine();
            
        }

        using (StreamWriter tileWriter = new StreamWriter(tileInfoFile, false))
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = generator.World[x, y];

                    tileWriter.WriteLine(
                        $"X:{x} Y:{y} | " +
                        $"Biome:{tile.Biome?.name ?? "Unknown"} | " +
                        $"Symbol:{tile.Biome?.symbol ?? '?'} | " +
                        $"Elevation:{tile.elevation:F3} | " +
                        $"Moisture:{tile.moisture:F3} | " +
                        $"Temperature:{tile.temperature:F3} | " +
                        $"Feature:{tile.feature ?? "None"} | " +
                        $"Resource:{tile.resource ?? "None"} | " +
                        $"Gold:{tile.gold} | " +
                        $"Food:{tile.food} | " +
                        $"Production:{tile.production} | " +
                        $"Passable:{tile.Biome?.passable ?? false} | " +
                        $"Weight:{tile.Biome?.weight ?? 0}"
                    );
                }
            }
        }
    Console.WriteLine("Wrote to test file");

}


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

if(!debug){
    app.MapGet("/map", () =>{
        //StringBuilder board = new StringBuilder();
        var map = new object[height][];

        for(int y = 0; y < height; y++)
        {
            
            map[y] = new object[width];
            for(int x = 0; x < width; x++)
            {
                if(squad.x == x && squad.y == y)
                {
                    map[y][x] = new
                    {
                        symbol = squad.symbol,
                        color = squad.color
                    };
                }else{
                    Tile tile = generator.World[x,y];

                    map[y][x] = new
                    {
                        symbol = tile.Biome?.symbol ?? '?',
                        color = tile.Biome?.color ?? "#ffffff",
                        passable = tile.Biome?.passable ?? true,
                        weight = tile.Biome?.weight ?? 0

                    };
                }
                
            }
        }
        return Results.Json(map);
    });
}


app.Run();
