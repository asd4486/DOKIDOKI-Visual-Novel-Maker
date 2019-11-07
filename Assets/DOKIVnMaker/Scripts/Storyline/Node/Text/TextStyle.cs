namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Text/Text style")]
    [NodeWidth(200)]
    [NodeTint("#99ffcc")]
    public class TextStyle : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Text style";
            base.Init();
        }

        public TextStyleObject style;
    }
}