using System;
using System.Collections.Generic;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class StoryLineActions
    {
        public List<CharcterSpriteInfos> CharcterSpriteInfoList;
        public List<DialogBox> DialogBoxList;
        public List<BrancheBox> BrancheBoxList;
        public List<BackgroundItem> BackgroundItemList;
        public List<CGInfoItem> CGInfoItemList;
        public List<Delayer> DelayerList;
        public List<Audio> AudioList;
        public List<Sound> SoundList;

        public List<CharacterOutInfos> CharacterOutInfoList;
        public List<ChangeStoryLine> ChangeStoryLineList;
        public List<ChangeScene> ChangeSceneList;

        public EditorStartPoint StartPoint;

        public StoryLineActions()
        {
            CharcterSpriteInfoList = new List<CharcterSpriteInfos>();
            DialogBoxList = new List<DialogBox>();
            BrancheBoxList = new List<BrancheBox>();
            BackgroundItemList = new List<BackgroundItem>();
            CGInfoItemList = new List<CGInfoItem>();
            DelayerList = new List<Delayer>();
            AudioList = new List<Audio>();
            SoundList = new List<Sound>();

            CharacterOutInfoList = new List<CharacterOutInfos>();
            ChangeStoryLineList = new List<ChangeStoryLine>();
            ChangeSceneList = new List<ChangeScene>();
        }

        public static StoryLineActions InitActions(List<NodeBase> nodes)
        {
            var actions = new StoryLineActions();

            foreach (var n in nodes)
            {
                if (n is CharcterSpriteInfos) { actions.CharcterSpriteInfoList.Add(n as CharcterSpriteInfos); }
                if (n is DialogBox) { actions.DialogBoxList.Add(n as DialogBox); }
                if (n is BrancheBox) { actions.BrancheBoxList.Add(n as BrancheBox); }
                if (n is BackgroundItem) { actions.BackgroundItemList.Add(n as BackgroundItem); }
                if (n is CGInfoItem) { actions.CGInfoItemList.Add(n as CGInfoItem); }
                if (n is Delayer) { actions.DelayerList.Add(n as Delayer); }
                if (n is Audio) { actions.AudioList.Add(n as Audio); }
                if (n is Sound) { actions.SoundList.Add(n as Sound); }
                if (n is CharacterOutInfos) { actions.CharacterOutInfoList.Add(n as CharacterOutInfos); }
                if (n is ChangeStoryLine) { actions.ChangeStoryLineList.Add(n as ChangeStoryLine); }
                if (n is ChangeScene) { actions.ChangeSceneList.Add(n as ChangeScene); }
                if (n is EditorStartPoint) { actions.StartPoint = n as EditorStartPoint; }
            }

            return actions;
        }
    }
}
