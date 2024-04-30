using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using ZeroElectric.Vinculum;

internal class Refresh
{
    Music background;
    Player player = null;
    public Refresh(Player player)
    {
        this.player = player;
    }
    public void UnloadMusic()
    {
        Raylib.UnloadMusicStream(background);
    }
    public void Update()
    {
        Random r = new Random();
        double delay = 0;
        DateTime start = DateTime.Now;
        DateTime check;
        bool go = true;
        if (r.Next(1, 3) == 1)
        {
            background = Raylib.LoadMusicStream("Sounds/E1-Fluffing-a-Duck.mp3");
        }
        else
        {
            background = Raylib.LoadMusicStream("Sounds/E2-Monkeys-Spinning-Monkeys.mp3");
        }
        background.looping = true;
        Raylib.PlayMusicStream(background);
        while (true)
        {
            Raylib.UpdateMusicStream(background);
            if (delay > 0) { delay -= 0.1; }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_9) && delay <= 0)
            {
                player.Move(1, -1);
                delay = 0.5;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_8) && delay <= 0)
            {
                player.Move(0, -1);
                delay = 0.5;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_7) && delay <= 0)
            {
                player.Move(-1, -1);
                delay = 0.5;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_6) && delay <= 0)
            {
                player.Move(1, 0);
                delay = 0.5;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_4) && delay <= 0)
            {
                player.Move(-1, 0);
                delay = 0.5;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_3) && delay <= 0)
            {
                player.Move(1, 1);
                delay = 0.5;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_2) && delay <= 0)
            {
                player.Move(0, 1);
                delay = 0.5;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_1) && delay <= 0)
            {
                player.Move(-1, 1);
                delay = 0.5;
            }
            if (go)
            {
                Raylib.BeginDrawing();
                Raylib.EndDrawing();
                Raylib.BeginDrawing();
                Raylib.EndDrawing();
                start = DateTime.Now;
                go = false;
            }
            check = DateTime.Now;
            if (check.Subtract(start).TotalMilliseconds >= 80)
            {
                go = true;
                
            }
        }
    }
}