using System.Collections.Generic;
using Character;
namespace GameManager
{
    public class Game{
        public IDictionary<string, BlobDude> Characterz;

        public Game(){
           Characterz = new Dictionary<string, BlobDude>();
        }

        public void AddBlobDude(BlobDude b){
            System.Console.WriteLine(b.userName +"USERNAME");
            Characterz.Add(b.userName, b);
        }

        public void updateParameters(movement m, string UserName){
            Characterz[UserName].x = m.x;
            Characterz[UserName].y = m.x;
        }

    }    
}