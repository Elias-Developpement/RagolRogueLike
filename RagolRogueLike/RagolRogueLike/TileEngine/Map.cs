using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.Entities;



namespace RagolRogueLike.TileEngine
{
    internal struct Rect
    {
        //TODO: Possibly look at changing width and height
        //To an x2, y2 so that the for loops in the generation code looks better.
        public int x;
        public int y;
        public int width;
        public int height;
        public int center_x;
        public int center_y;

        public Rect(int x, int y, int w, int h)
        {
            this.x = x;
            this.y = y;
            width = w;
            height = h;
            center_x = (2 * x + w) / 2;
            center_y = (2 * y + h) / 2;
        }

    }

    public class Map
    {
        #region Field Region

        static int mapWidth;
        static int mapHeight;

        Tile[,] testMap;

        SpriteFont tileFont;

        List<Rect> rooms;
        const int roomMinSize = 6;
        const int roomMaxSize = 10;
        const int maxRooms = 150;

        #endregion

        #region Property Region

        public static int MapWidthInPixels
        {
            get { return Engine.TileWidth * mapWidth; }
        }

        public static int MapHeightInPixels
        {
            get { return Engine.TileHeight * mapHeight; }
        }

        #endregion

        #region Constructor Region

        public Map(int mWidth, int mHeight, SpriteFont tileFont)
        {
            mapWidth = mWidth;
            mapHeight = mHeight;

            testMap = new Tile[mapWidth, mapHeight];
            rooms = new List<Rect>();

            this.tileFont = tileFont;
            fillMap();
            CreateTestMap();
            //fillBlocked();
        }

        //Used to create the next level of the map.
        //Still needs to be implemented by adding stairs and what not.
        public Map(SpriteFont tileFont)
        {
            this.tileFont = tileFont;
        }


        #endregion

        #region Method Region

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Point cameraPoint = Engine.VectorToCell(camera.Position * (1 / camera.Zoom));
            Point viewPoint = Engine.VectorToCell(new Vector2((camera.Position.X + camera.ViewportRectangle.Width) * (1 / camera.Zoom), (camera.Position.Y + camera.ViewportRectangle.Height) * (1 / camera.Zoom)));

            Point min = new Point();
            Point max = new Point();

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);

            max.X = Math.Min(viewPoint.X + 1, mapWidth);
            max.Y = Math.Min(viewPoint.Y + 1, mapHeight);


            Rectangle destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);

            //TODO: Don't draw tiles that are being occupied.
            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * Engine.TileHeight;

                for (int x = min.X; x < max.X; x++)
                {
                    destination.X = x * Engine.TileWidth;

                    spriteBatch.DrawString(tileFont, testMap[x, y].Symbol, testMap[x, y].Position, testMap[x, y].Color);
                }
            }

        }

        private void fillMap()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    testMap[x, y] = new Tile("#", true, Color.White, new Vector2(x * 16, y * 16));
                }
            }
        }

        private void CreateTestMap()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int num_rooms = 0;
            

            while(num_rooms < maxRooms)
            {
                int w = random.Next(roomMinSize, roomMaxSize);
                int h = random.Next(roomMinSize, roomMaxSize);
                //Random position for the room without going out of the map
                int x = random.Next(1, mapWidth - w - 1);
                int y = random.Next(1, mapHeight - h - 1);
                //TODO: Figure out why the map draws rooms that off the map.

                CreateRoom(x, y, w, h);

                num_rooms++;
            }
        }

        private void CreateRoom(int x, int y, int w, int h)
        {
            Rect room = new Rect(x, y, w, h);

            if (rooms.Count == 0)
            {
                //This is the first room so draw that shit and add it to rooms.
                for (int i = room.x; i < (room.x + room.width); i++)
                {
                    for (int j = room.y; j < (room.y + room.height); j++)
                    {
                        testMap[i, j] = new Tile(".", false, Color.White, new Vector2(i * 16, j * 16));
                    }
                }
                rooms.Add(room);
            }
            else
            {
                //Check to see if the room intesects any other room
                bool intersection = false;
                foreach (Rect otherRoom in rooms)
                {
                    intersection = IntersectionOfRooms(room, otherRoom);
                    if (intersection)
                    {
                        break;
                    }
                }
                //If they do intersect then end the function
                if (intersection)
                {
                    return;
                }
                else
                {
                    //Otherwise draw the room and append it.
                    for (int i = room.x; i < (room.x + room.width); i++)
                    {
                        for (int j = room.y; j < (room.y + room.height); j++)
                        {
                            testMap[i, j] = new Tile(".", false, Color.White, new Vector2(i * 16, j * 16));
                        }
                    }
                    //Get the room count before adding the new room.
                    int roomCount = rooms.Count;
                    rooms.Add(room);
                    //TODO: Connecting rooms.
                    //Flip a coin to decide which direction to go first
                    Random coin = new Random(DateTime.Now.Millisecond);
                    if (coin.Next(0, 2) == 1)
                    {
                        //First horizontal then vertical
                        CreateHTunnel(rooms[roomCount - 1].center_x, rooms[roomCount - 1].center_y, room.center_x);
                        CreateVTunnel(room.center_x, rooms[roomCount - 1].center_y, room.center_y);
                    }
                    else
                    {
                        //First vertical then horizontal
                        CreateVTunnel(rooms[roomCount - 1].center_x, rooms[roomCount - 1].center_y, room.center_y);
                        CreateHTunnel(rooms[roomCount - 1].center_x, room.center_y, room.center_x);
                    }
                }
            }
        }

        private void CreateHTunnel(int x1, int y, int x2)
        {
            int min = Math.Min(x1, x2);
            int max = Math.Max(x1, x2);

            for (int i = min; i < max; i++)
            {
                testMap[i, y] = new Tile(".", false, Color.White, new Vector2(i * 16, y * 16));
            }
        }

        private void CreateVTunnel(int x, int y1, int y2)
        {
            int min = Math.Min(y1, y2);
            int max = Math.Max(y1, y2);

            for (int i = min; i < max; i++)
            {
                testMap[x, i] = new Tile(".", false, Color.White, new Vector2(x * 16, i * 16));
            }
        }

        private bool IntersectionOfRooms(Rect room1, Rect room2)
        {
            //Returns false if the two rooms don't intersect and returns true if they do.
            if (room1.x <= (room2.x + room2.width) && (room1.x + room1.width) >= room2.x && room1.y <= (room2.y + room2.height) && (room1.y + room1.height) >= room2.y)
            {
                return true;
            }

            return false;
        }

        public bool GetBlocked(int x, int y)
        {
            if (testMap[x, y].Block)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}
