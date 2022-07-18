using System;
using System.Collections.Generic;

public class Generate
{
    public class Tile
    {
        public bool searched;
        public bool flagged;
        public bool bomb;
        public int xPos;
        public int yPos;
        public int number;

        public Tile(int x, int y)
        {
            searched = false;
            flagged = false;
            bomb = false;
            xPos = x;
            yPos = y;
            number = 0;
        }
    }

    public class Variables
    {
        private static Dictionary<string, Dictionary<string, int>> presets = new Dictionary<string, Dictionary<string, int>>()
        {
          {"test", new Dictionary<string, int>()
          {
            {"width", 3},
            {"height", 3},
            {"bombs", 3}
          }},
          {"easy", new Dictionary<string, int>()
          {
            {"width", 9},
            {"height", 9},
            {"bombs", 10}
          }},
          {"medium", new Dictionary<string, int>()
          {
            {"width", 16},
            {"height", 16},
            {"bombs", 40}
          }},
          {"expert", new Dictionary<string, int>()
          {
            {"width", 30},
            {"height", 16},
            {"bombs", 99}
          }}
        };

        public static Dictionary<string, Dictionary<string, int>> Presets
        {
            get { return presets; }
            set { presets = value; }
        }
    }
    public static List<List<Tile>> Init(Dictionary<string, Dictionary<string, int>> presets, string difficulty)
    {
        List<List<Tile>> tiles = new List<List<Tile>>();
        //initializes the tiles
        for (int h = 0; h < presets[difficulty]["height"]; h++)
        {
            List<Tile> row = new List<Tile>();
            for (int w = 0; w < presets[difficulty]["width"]; w++)
            {
                row.Add(new Tile(w, h));
            }
            tiles.Add(row);
        }
        //places the bombs
        Random rnd = new Random();
        int bombs = 0;
        while (bombs < presets[difficulty]["bombs"])
        {
            int x = rnd.Next(presets[difficulty]["width"]);
            int y = rnd.Next(presets[difficulty]["height"]);
            if (tiles[y][x].bomb == false)
            {
                tiles[y][x].bomb = true;
                bombs++;
            }
        }
        //determines the number of bombs adjacent to each tile
        for (int y = 0; y < tiles.Count; y++)
        {
            for (int x = 0; x < tiles[y].Count; x++)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int v = -1; v < 2; v++)
                    {
                        if (y + i >= 0 && y + i < tiles.Count && x + v >= 0 && x + v < tiles[y].Count)
                        {
                            if (tiles[y + i][x + v].bomb)
                            {
                                tiles[y][x].number += 1;
                            }
                        }
                    }
                }
            }
        }
        return tiles;
    }
    public Generate()
	{
        //List<List<string>> testDisplay = new List<List<string>>();
        //for (int i = 0; i < tiles.Count; i++)
        //{
        //    List<string> row = new List<string>();
        //    for (int v = 0; v < tiles[i].Count; v++)
        //    {
        //        if (tiles[i][v].bomb)
        //        {
        //            row.Add("■");
        //        }
        //        else if (tiles[i][v].number > 0)
        //        {
        //            row.Add(tiles[i][v].number.ToString());
        //        }
        //        else
        //        {
        //            row.Add(" ");
        //        }
        //    }
        //    testDisplay.Add(row);
        //    Console.WriteLine(String.Join(" ", testDisplay[i]));
        //}
    }
}
