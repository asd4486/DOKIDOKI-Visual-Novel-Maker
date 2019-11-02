//using System;
//using System.Collections.Generic;

//namespace DokiVnMaker.MyEditor.Items
//{
//    [Serializable]
//    public class StoryLineActions
//    {
//        public List<CharacterSpriteInfos> CharcterSpriteInfoList;
//        public List<DialogBox> DialogBoxList;
//        public List<BrancheBox> BrancheBoxList;
//        public List<BackgroundImage> BackgroundImageList;
//        public List<CGImage> CGInfoItemList;
//        public List<Delayer> DelayerList;
//        public List<Music> AudioList;
//        public List<Sound> SoundList;

//        public List<CharacterOutInfos> CharacterOutInfoList;
//        public List<ChangeStoryLine> ChangeStoryLineList;
//        public List<ChangeScene> ChangeSceneList;

//        public EditorStartPoint StartPoint;

//        public StoryLineActions()
//        {
//            CharcterSpriteInfoList = new List<CharacterSpriteInfos>();
//            DialogBoxList = new List<DialogBox>();
//            BrancheBoxList = new List<BrancheBox>();
//            BackgroundImageList = new List<BackgroundImage>();
//            CGInfoItemList = new List<CGImage>();
//            DelayerList = new List<Delayer>();
//            AudioList = new List<Music>();
//            SoundList = new List<Sound>();

//            CharacterOutInfoList = new List<CharacterOutInfos>();
//            ChangeStoryLineList = new List<ChangeStoryLine>();
//            ChangeSceneList = new List<ChangeScene>();
//        }

//        public static StoryLineActions Create(List<NodeBase> nodes)
//        {
//            var actions = new StoryLineActions();

//            foreach (var n in nodes)
//            {
//                if (n is CharacterSpriteInfos) { actions.CharcterSpriteInfoList.Add(n as CharacterSpriteInfos); }
//                if (n is DialogBox) { actions.DialogBoxList.Add(n as DialogBox); }
//                if (n is BrancheBox) { actions.BrancheBoxList.Add(n as BrancheBox); }
//                if (n is BackgroundImage) { actions.BackgroundImageList.Add(n as BackgroundImage); }
//                if (n is CGImage) { actions.CGInfoItemList.Add(n as CGImage); }
//                if (n is Delayer) { actions.DelayerList.Add(n as Delayer); }
//                if (n is Music) { actions.AudioList.Add(n as Music); }
//                if (n is Sound) { actions.SoundList.Add(n as Sound); }
//                if (n is CharacterOutInfos) { actions.CharacterOutInfoList.Add(n as CharacterOutInfos); }
//                if (n is ChangeStoryLine) { actions.ChangeStoryLineList.Add(n as ChangeStoryLine); }
//                if (n is ChangeScene) { actions.ChangeSceneList.Add(n as ChangeScene); }
//                if (n is EditorStartPoint) { actions.StartPoint = n as EditorStartPoint; }
//            }

//            return actions;
//        }
//    }
//}
