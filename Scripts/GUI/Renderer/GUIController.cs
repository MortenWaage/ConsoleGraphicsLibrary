namespace GUILib
{
    class GUIController
    {
        public static GUIController Instance { get; set; }
        public EventLog EventLog { get; set; }
        public ScreenBuffer ScreenBuffer { get; set; }
        public GUIRenderer Renderer { get; set; }
        public ElementController ElementController { get; set; }
    }
}