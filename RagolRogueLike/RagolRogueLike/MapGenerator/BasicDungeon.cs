using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using RagolRogueLike.TileEngine;
using RagolRogueLike.PlayerClasses;

namespace RagolRogueLike.MapGenerator
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

    public class BasicDungeon
    {
        #region Field Region

        Tile[,] dungeon;
        Player player;

        List<Rect> rooms;
        const int roomMinSize = 6;
        const int roomMaxSize = 10;
        const int maxRooms = 150;

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        //TODO: Take the last room and add a stairwell to it down to the next floor.
        public BasicDungeon(Tile[,] dungeon, Player player)
        {
            this.dungeon = dungeon;
            this.player = player;

            rooms = new List<Rect>();
        }

        #endregion
        
        #region Method Region

        public Tile[,] CreateBasicDungeon()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int num_rooms = 0;


            while (num_rooms < maxRooms)
            {
                int w = random.Next(roomMinSize, roomMaxSize);
                int h = random.Next(roomMinSize, roomMaxSize);
                //Random position for the room without going out of the map
                int x = random.Next(1, dungeon.GetLength(0) - w - 1);
                int y = random.Next(1, dungeon.GetLength(1) - h - 1);

                CreateRoom(x, y, w, h);

                num_rooms++;
            }

            return dungeon;
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
                        dungeon[i, j] = new Tile(".", false, Color.White, new Vector2(i * 16, j * 16));
                    }
                }
                rooms.Add(room);

                Random rnd = new Random(DateTime.Now.Millisecond);
                int playerX = rnd.Next(x, x + w + 1);
                int playerY = rnd.Next(y, y + h + 1);

                player.Position = new Vector2(playerX * 16, playerY * 16);
                player.Camera.LockToPlayer(player);
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
                            dungeon[i, j] = new Tile(".", false, Color.White, new Vector2(i * 16, j * 16));
                        }
                    }
                    //Get the room count before adding the new room.
                    int roomCount = rooms.Count;
                    rooms.Add(room);

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

            for (int i = min; i < max + 1; i++)
            {
                dungeon[i, y] = new Tile(".", false, Color.White, new Vector2(i * 16, y * 16));
            }
        }

        private void CreateVTunnel(int x, int y1, int y2)
        {
            int min = Math.Min(y1, y2);
            int max = Math.Max(y1, y2);

            for (int i = min; i < max + 1; i++)
            {
                dungeon[x, i] = new Tile(".", false, Color.White, new Vector2(x * 16, i * 16));
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


        #endregion
    }
}
