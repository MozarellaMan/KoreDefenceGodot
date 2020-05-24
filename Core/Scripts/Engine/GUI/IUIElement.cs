using System;
using Godot;

namespace KoreDefenceGodot.Core.Scripts.Engine.GUI
{
    public interface IUIElement
    {
        bool UiVisible { get; set; }
        bool UiActive { get; set; }
        Control Screen { get; set; }
        
        Vector2 UiPosition { get; set; }
        Color UiColor { get; set; }
        Color UiBorderColor { get; set; }
        int UiBorderThickness { get; set; }

        Action<Vector2> OnClick { get; set; }
        Action<Vector2> OnClickEnd { get; set; }
        Action<Vector2> OnHover { get; set; }
        Action<Vector2> OnHoverEnd { get; set; }

        void ConnectClick(Vector2 pos);
        void ConnectClickEnd(Vector2 pos);
        void ConnectHover(Vector2 pos);
        void ConnectHoverEnd(Vector2 pos);
    }
}