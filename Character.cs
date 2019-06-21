using System;
namespace Character
{
    interface IGamePoint
    {

        int x { get; set; }
        int y { get; set; }

    }

    public class Point :IGamePoint{
        public int x { get; set; }
        public int y { get; set; }
    }
    interface IGameObject:IGamePoint
    {
        int width { get; set; }
        int height { get; set; }

    }

    public class movement:IGameObject{
        public int x { get; set; }
        public int y { get; set; }

        public int width { get; set; }
        public  int height { get; set; }

        public int xVelocity { get; set; }
        public int yVelocity { get; set; }
    }

    public class BlobDude : movement
    {
        public string userName {get;set;}

        public BlobDude(string username, int x, int y, int width, int height){
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.xVelocity = 0;
            this.yVelocity = 0;
            this.userName = username;
        }

    }


}