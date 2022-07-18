namespace minesweeper
{
    public partial class Form : System.Windows.Forms.Form
    {
        public class Variables
        {
            public static List<List<Generate.Tile>> tiles = new List<List<Generate.Tile>>();
            public static Icon tileBomb = Properties.Resources.Tile_Bomb;
            public static Icon tileFlag = Properties.Resources.Tile_Flag;
            public static Icon tileUnsearched = Properties.Resources.Tile_Unsearched;
            private static Icon tile0 = Properties.Resources.Tile_Empty;
            private static Icon tile1 = Properties.Resources.Tile_1;
            private static Icon tile2 = Properties.Resources.Tile_2;
            private static Icon tile3 = Properties.Resources.Tile_3;
            private static Icon tile4 = Properties.Resources.Tile_4;
            private static Icon tile5 = Properties.Resources.Tile_5;
            private static Icon tile6 = Properties.Resources.Tile_6;
            private static Icon tile7 = Properties.Resources.Tile_7;
            private static Icon tile8 = Properties.Resources.Tile_8;
            public static Dictionary<int, Icon> tilesDict = new Dictionary<int, Icon>()
            {
                {0, tile0},
                {1, tile1},
                {2, tile2},
                {3, tile3},
                {4, tile4},
                {5, tile5},
                {6, tile6},
                {7, tile7},
                {8, tile8}
            };
            //public static List<List<Generate.Tile>> Tiles
            //{
            //    get { return tiles; }
            //    set { tiles = value; }
            //}
        }
        private void Refresh(string difficulty)
        {
            Variables.tiles = Generate.Init(Generate.Variables.Presets, difficulty);
            for (int i = 0; i < Variables.tiles.Count; i++)
            {
                GameGrid.Columns.Add(new DataGridViewImageColumn());
                GameGrid.Rows.Add();
                GameGrid.Columns[i].Width = 24;
                GameGrid.Rows[i].Height = 24;
            }
            GameGrid.Width = Variables.tiles.Count * GameGrid.Columns[0].Width + 1;
            GameGrid.Height = Variables.tiles.Count * GameGrid.Rows[0].Height + 1;
            this.Width = GameGrid.Width + 26;
            this.Height = GameGrid.Height + 49;
            for (int y = 0; y < Variables.tiles.Count; y++)
            {
                for (int x = 0; x < Variables.tiles[y].Count; x++)
                {
                    GameGrid.Rows[x].Cells[y].Value = Variables.tileUnsearched;
                }
            }
        }
        public Form()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Refresh("medium");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
        private void GameGrid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && !Variables.tiles[e.ColumnIndex][e.RowIndex].searched)
            {
                if (Variables.tiles[e.ColumnIndex][e.RowIndex].flagged)
                {
                    Variables.tiles[e.ColumnIndex][e.RowIndex].flagged = false;
                    GameGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Variables.tileUnsearched;
                }
                else
                {
                    Variables.tiles[e.ColumnIndex][e.RowIndex].flagged = true;
                    GameGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Variables.tileFlag;
                }
            }
            else
            {
                if (!Variables.tiles[e.ColumnIndex][e.RowIndex].searched && !Variables.tiles[e.ColumnIndex][e.RowIndex].flagged)
                {
                    if (Variables.tiles[e.ColumnIndex][e.RowIndex].bomb)
                    {
                        GameGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Variables.tileBomb;
                        GameGrid.Enabled = false;
                        Variables.tiles[e.ColumnIndex][e.RowIndex].searched = true;
                    }
                    else if (Variables.tiles[e.ColumnIndex][e.RowIndex].number == 0)
                    {
                        List<Generate.Tile> searchList = new List<Generate.Tile>();
                        bool searching = true;
                        for (int i = -1; i < 2; i++)
                        {
                            for (int v = -1; v < 2; v++)
                            {
                                if (e.ColumnIndex + i >= 0 && e.ColumnIndex + i < Variables.tiles.Count && e.RowIndex + v >= 0 && e.RowIndex + v < Variables.tiles[e.ColumnIndex].Count)
                                {
                                    searchList.Add(Variables.tiles[e.ColumnIndex + i][e.RowIndex + v]);
                                }
                            }
                        }
                        Variables.tiles[e.ColumnIndex][e.RowIndex].searched = true;
                        while (searching)
                        {
                            searching = false;
                            for (int t = 0; t < searchList.Count; t++)
                            {
                                Generate.Tile currentTile = searchList[t];
                                if (!currentTile.searched && currentTile.number < 1)
                                {
                                    currentTile.searched = true;
                                    searching = true;
                                    for (int i = -1; i < 2; i++)
                                    {
                                        for (int v = -1; v < 2; v++)
                                        {
                                            if (currentTile.yPos + i >= 0 && currentTile.yPos + i < Variables.tiles.Count && currentTile.xPos + v >= 0 && currentTile.xPos + v < Variables.tiles[currentTile.xPos].Count)
                                            {
                                                searchList.Add(Variables.tiles[currentTile.yPos + i][currentTile.xPos + v]);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    currentTile.searched = true;
                                }
                            }
                        }
                        for (int i = 0; i < searchList.Count; i++)
                        {
                            GameGrid.Rows[searchList[i].xPos].Cells[searchList[i].yPos].Value = Variables.tilesDict[searchList[i].number];
                        }
                    }
                    else
                    {
                        GameGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Variables.tilesDict[Variables.tiles[e.ColumnIndex][e.RowIndex].number];
                        Variables.tiles[e.ColumnIndex][e.RowIndex].searched = true;
                    }
                }
            }
        }
    }
}