using System;
using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.GUI
{
    public class UiPanel : Node2D, IUIElement
    {
        public bool UiVisible { get; set; }
        public bool UiActive { get; set; }
        public Control Screen { get; set; }
        public Vector2 UiPosition { get; set; }
        public Color UiColor { get; set; }
        public Color UiBorderColor { get; set; }
        public int UiBorderThickness { get; set; }
        public Action<Vector2> OnClick { get; set; }
        public Action<Vector2> OnClickEnd { get; set; }
        public Action<Vector2> OnHover { get; set; }
        public Action<Vector2> OnHoverEnd { get; set; }
        
        
        public void ConnectClick(Vector2 pos)
        {
            OnClick?.Invoke(pos);
        }

        public void ConnectClickEnd(Vector2 pos)
        {
            OnClickEnd?.Invoke(pos);
        }

        public void ConnectHover(Vector2 pos)
        {
            OnHover?.Invoke(pos);
        }

        public void ConnectHoverEnd(Vector2 pos)
        {
            OnHoverEnd?.Invoke(pos);
        }
    }
}