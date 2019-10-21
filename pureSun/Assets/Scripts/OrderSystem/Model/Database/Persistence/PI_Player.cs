

namespace Assets.Scripts.OrderSystem.Model.Database.Persistence
{
    public class PI_Player
    {
        //主要ID
        public string playerCode
        {
            get;  set;
        }
        //
        public string shipCardCode
        {
            get;  set;
        }
        public string[] handCard { get; set; }
        public string[] deckCard { get; set; }
    }
}
