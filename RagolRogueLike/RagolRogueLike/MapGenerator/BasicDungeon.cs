using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RagolRogueLike.TileEngine;
using RagolRogueLike.PlayerClasses;
using RagolRogueLike.Entities;
using RagolRogueLike.GameObject;

namespace RagolRogueLike.MapGenerator
{
    internal struct Rect
    {
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

        SpriteFont entityFont;

        int stairsDownX;
        int stairsDownY;
        int stairsUpX;
        int stairsUpY;

        Tile[,] dungeon;
        Player player;
        EntityManager entities;
        ItemManager items;

        Random random;

        List<Rect> rooms;
        const int roomMinSize = 6;
        const int roomMaxSize = 10;
        const int maxRooms = 70;
        
        //Subtract 1 from these numbers to get the actual max number of items and monsters.
        const int maxItems = 4;
        const int maxMonsters = 3;

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        public BasicDungeon(Tile[,] dungeon, Player player, SpriteFont entityFont)
        {
            this.dungeon = dungeon;
            this.player = player;
            this.entityFont = entityFont;

            random = new Random(DateTime.Now.Millisecond);

            entities = new EntityManager();
            items = new ItemManager();

            rooms = new List<Rect>();
        }

        #endregion
        
        #region Method Region

        public Tile[,] CreateBasicDungeon()
        {
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

            //Place the stairs up into the first room.
            dungeon[stairsUpX, stairsUpY] = new Tile("<", false, Color.White, Color.LightGray, new Vector2(stairsUpX * 16, stairsUpY * 16));

            //Place the stairs down into the last room. 
            int lastRoom = rooms.Count - 1;
            stairsDownX = random.Next(rooms[lastRoom].x, rooms[lastRoom].x + rooms[lastRoom].width);
            stairsDownY = random.Next(rooms[lastRoom].y, rooms[lastRoom].y + rooms[lastRoom].height);
            dungeon[stairsDownX, stairsDownY] = new Tile(">", false, Color.White, Color.LightGray, new Vector2(stairsDownX * 16, stairsDownY * 16));

            //Initialize the pathfinder for this floor of the dungeon.
            entities.StartPathfinder(dungeon);

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
                        dungeon[i, j] = new Tile(".", false, Color.White, Color.LightGray, new Vector2(i * 16, j * 16));
                    }
                }
                rooms.Add(room);

                int playerX = random.Next(x, x + w);
                int playerY = random.Next(y, y + h);

                player.Position = new Vector2(playerX * 16, playerY * 16);
                player.Camera.LockToPlayer(player);

                //Entity testentity = new Entity("@", Color.Green, entityFont, new Vector2((playerX + 1) * 16, (playerY + 1) * 16));
                //entities.AddEntity(testentity);

                stairsUpX = playerX;
                stairsUpY = playerY;
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
                            dungeon[i, j] = new Tile(".", false, Color.White, Color.LightGray, new Vector2(i * 16, j * 16));
                        }
                    }
                    //Decide on the number of monsters and items to add to the room.

                    int NumItems = random.Next(0, maxItems);
                    int NumMonsters = random.Next(0, maxMonsters);

                    //Loop for the items to create in the room
                    for (int i = 0; i < NumItems; i ++)
                    {
                        int itemX = random.Next(x, x + w);
                        int itemY = random.Next(y, y + h);

                        Item item = new Item("Potion", "!", Color.Orange, entityFont, new Vector2(itemX * 16, itemY * 16));
                        items.AddItem(item);
                    }

                    for (int m = 0; m < NumMonsters; m++)
                    {
                        int monsterX = random.Next(x, x + w);
                        int monsterY = random.Next(y, y + h);

                        Entity entity = new Entity("O", Color.Green, entityFont, new Vector2(monsterX * 16, monsterY * 16));
                        entities.AddEntity(entity);
                    }

                    //Get the room count before adding the new room.
                    int roomCount = rooms.Count;
                    rooms.Add(room);

                    //Flip a coin to decide which direction to go first
                    if (random.Next(0, 2) == 1)
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
                dungeon[i, y] = new Tile(".", false, Color.White, Color.LightGray, new Vector2(i * 16, y * 16));
            }
        }

        private void CreateVTunnel(int x, int y1, int y2)
        {
            int min = Math.Min(y1, y2);
            int max = Math.Max(y1, y2);

            for (int i = min; i < max + 1; i++)
            {
                dungeon[x, i] = new Tile(".", false, Color.White, Color.LightGray, new Vector2(x * 16, i * 16));
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

        public EntityManager GetEntities()
        {
            return entities;
        }

        public ItemManager getItems()
        {
            return items;
        }

        #endregion
    }
}
