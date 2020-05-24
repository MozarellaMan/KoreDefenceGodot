using System.Collections.Generic;
using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.GUI
{
    public class UiScreen : Control
    {
        private LinkedList<IUIElement> _popupQueue;
        public bool PopupInFocus { get; private set; }

        public UiScreen()
        {
            _popupQueue = new LinkedList<IUIElement>();
            PopupInFocus = false;
        }

        public void updatePopups()
        {
            IUIElement nextPopup = _popupQueue.First.Value;

            if (nextPopup != null)
            {
                PopupInFocus = true;
            }
            
        }
        
    }
}