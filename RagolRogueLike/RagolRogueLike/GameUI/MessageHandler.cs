using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RagolRogueLike.GameUI
{
    public static class MessageHandler
    {
        #region Field Region

        //Holds the messages and will be used to display and remove message from the list.
        public static List<string> messages = new List<string>();

        #endregion
        
        #region Property Region
        
        #endregion
        
        #region Method Region

        //Takes off the current message from the list after it is displayed.
        public static void RemoveMessage()
        {
            messages.RemoveAt(0);
        }
        
        //Adds a message to the end of the list to be processed.
        public static void AddMessage(string message)
        {
            messages.Add(message);
        }

        //TODO: Add in how to handle deleting messages from the list.

        #endregion
    }
}
