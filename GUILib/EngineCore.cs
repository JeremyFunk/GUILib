using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.Util;
using GUILib.GUI.Render;
using GUILib.GUI.GuiElements;
using GUILib.GUI;
using GUILib.GUI.Constraints;
using GUILib.GUI.Animations;
using GUILib.Events;

using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using GUILib.GUI.Animations.Transitions;
using GUILib.GUI.PixelConstraints;

namespace GUILib
{
    class EngineCore : GameWindow
    {
        private GuiRenderer guiRenderer;
        private GuiScene scene;
        private Window window;

        public EngineCore(int widthP, int heightP, string title) : base(widthP, heightP, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8, 8, 8, 8), 24, 8, 4))
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            GameSettings.Width = Width;
            GameSettings.Height = Height;

            guiRenderer = new GuiRenderer();
            scene = new GuiScene();

            LoadGameMenuExample();

            GameInput.Initialize();

            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
        }
        private void LoadGameMenuExample()
        {
            LoadGameMenu();

            LoadBackground();

            LoadWindow();

            scene.FirstUpdate(0.0001f);
        }

        private void LoadBackground()
        {
            GuiElement background = new Quad(new Material(new Texture("Background.jpg")), 0, 0, 0, 0);
            background.zIndex = -2;
            background.generalConstraint = new FillConstraintGeneral();

            Text text = new Text(0.5f, 0.5f, "Great Game", 2f);
            Text text2 = new Text(0.5f, 5, "Copyright Stuff", 0.7f);

            text.xConstraints.Add(new CenterConstraint());
            text.yConstraints.Add(new MarginConstraint(10));
            text2.xConstraints.Add(new MarginConstraint(10));

            scene.AddParent(background);
            scene.AddParent(text);
            scene.AddParent(text2);
        }

        private void LoadWindow()
        {
            window = new Window(0.2f, 0.2f, 0.6f, 0.6f, "New Game...", 1);
            window.heightConstraints.Add(new MinConstraint(540));
            window.visible = false;

            BorderedButton confirmButton = new BorderedButton(0, 10, 140, 40, "Confirm", true, 0.9f);
            confirmButton.xConstraints.Add(new MarginConstraint(10));

            BorderedButton cancelButton = new BorderedButton(0, 10, 140, 40, "Cancel", true, 0.9f);
            cancelButton.xConstraints.Add(new MarginConstraint(10 + 140 + 5));

            TextField nameField = new TextField(255, 10, 1f, 40, "Enter Map Name...");
            nameField.widthConstraints.Add(new SubtractConstraint(260 + 10 + 140 + 5 + 140));

            window.AddChild(confirmButton);
            window.AddChild(cancelButton);
            window.AddChild(nameField);

            BorderedQuad mapIcon = new BorderedQuad(10, 10, 240, 240, new Material(new Texture("Map.png")), new Material(new Vector4(0.2f, 0.2f, 0.2f, 1f)), 2);
            mapIcon.yConstraints.Add(new MarginConstraint(40));

            TextArea mapInfo = new TextArea(10, 10, 240, 1f, "This is very important information about the game map. Lorem ipsum dolor sit amet.", 0.7f);
            mapInfo.heightConstraints.Add(new SubtractConstraint(300));

            TabPane tabPane = new TabPane(255, 60, 1f, 1f, new Material(new Vector4(0.5f, 0.5f, 0.5f, 0.6f)));
            tabPane.yConstraints.Add(new MarginConstraint(40));
            tabPane.widthConstraints.Add(new SubtractConstraint(265));
            tabPane.heightConstraints.Add(new SubtractConstraint(window.GetTopBarSize() + 5 + 57));

            TabData generalData = new TabData("General");

            tabPane.AddTab(generalData);
            tabPane.AddTab(new TabData("Map"));
            tabPane.AddTab(new TabData("Advanced", 160));

            LoadGeneralTab(tabPane);

            window.AddChild(mapIcon);
            window.AddChild(mapInfo);
            window.AddChild(tabPane);
            scene.AddParent(window);

            LoadMapTab(tabPane);
        }

        private void LoadGameMenu()
        {
            VerticalList vList = new VerticalList(50, 0.5f, 100, 320, 5, 0);
            vList.yConstraints.Add(new CenterConstraint());

            GuiElement quad = new BorderedButton(0, 0, 250, 60, "New Game");
            quad.mouseButtonReleasedEvent = NewGame;
            GuiElement quad2 = new BorderedButton(0, 0, 250, 60, "Load Game");
            GuiElement quad3 = new BorderedButton(0, 0, 250, 60, "Options");
            GuiElement quad4 = new BorderedButton(0, 0, 250, 60, "Credits");
            GuiElement quad5 = new BorderedButton(0, 0, 250, 60, "Quit Game");

            AnimationKeyframe k1 = new AnimationKeyframe(0);
            AnimationKeyframe k2 = new AnimationKeyframe(0.2f);

            k1.x = 0;
            k2.x = 30;
            k2.width = 10;
            k2.height = 5;
            k2.opacity = 0.3f;

            AnimationKeyframe c1 = new AnimationKeyframe(0);
            AnimationKeyframe c2 = new AnimationKeyframe(0.07f);

            c2.width = 8;
            c2.height = 4;
            c2.x = -4;
            c2.y = -2;

            List<AnimationKeyframe> keyframes = new List<AnimationKeyframe>();
            keyframes.Add(k1);
            keyframes.Add(k2);

            List<AnimationKeyframe> cKeyframes = new List<AnimationKeyframe>();
            cKeyframes.Add(c1);
            cKeyframes.Add(c2);

            AnimationClass animationStruct = new AnimationClass(AnimationType.PauseOnEnd, keyframes, "Fade");
            animationStruct.transition = new CatmullRomSplineTransition(-15, 1);

            AnimationClass cAnimationStruct = new AnimationClass(AnimationType.SwingBack, cKeyframes, "Click");
            cAnimationStruct.transition = new SmoothstepTransition(1);


            Animation a = new Animation();
            a.AddAnimation(animationStruct);
            a.AddAnimation(cAnimationStruct);

            quad.animation = a;
            quad2.animation = a;
            quad3.animation = a;
            quad4.animation = a;
            quad5.animation = a;

            quad.SetStartHoverAnimation("Fade", AnimationRunType.Run);
            quad2.SetStartHoverAnimation("Fade", AnimationRunType.Run);
            quad3.SetStartHoverAnimation("Fade", AnimationRunType.Run);
            quad4.SetStartHoverAnimation("Fade", AnimationRunType.Run);
            quad5.SetStartHoverAnimation("Fade", AnimationRunType.Run);

            quad.SetEndHoverAnimation("Fade", AnimationRunType.Swing);
            quad2.SetEndHoverAnimation("Fade", AnimationRunType.Swing);
            quad3.SetEndHoverAnimation("Fade", AnimationRunType.Swing);
            quad4.SetEndHoverAnimation("Fade", AnimationRunType.Swing);
            quad5.SetEndHoverAnimation("Fade", AnimationRunType.Swing);

            quad.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);
            quad2.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);
            quad3.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);
            quad4.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);
            quad5.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);

            quad.opacity = 0.6f;
            quad2.opacity = 0.6f;
            quad3.opacity = 0.6f;
            quad4.opacity = 0.6f;
            quad5.opacity = 0.6f;

            vList.AddChild(quad);
            vList.AddChild(quad2);
            vList.AddChild(quad3);
            vList.AddChild(quad4);
            vList.AddChild(quad5);

            scene.AddParent(vList);
        }

        private void LoadGeneralTab(TabPane tabPane)
        {
            Text mapTypeText = new Text(0, 0, "Map Type:", 1f);

            mapTypeText.yConstraints.Add(new MarginConstraint(10));
            mapTypeText.xConstraints.Add(new CenterConstraint());
            mapTypeText.xConstraints.Add(new SubtractConstraint(145));

            Text difficultyText = new Text(0, 0, "Difficulty:", 1f);

            difficultyText.yConstraints.Add(new MarginConstraint(60));
            difficultyText.xConstraints.Add(new CenterConstraint());
            difficultyText.xConstraints.Add(new SubtractConstraint(145));

            Text seedText = new Text(0, 0, "Seed:", 1f);

            seedText.yConstraints.Add(new MarginConstraint(110));
            seedText.xConstraints.Add(new CenterConstraint());
            seedText.xConstraints.Add(new SubtractConstraint(145));

            ChoiceBox mapType = new ChoiceBox(0, 0, 250, 40, 5, "Map Type", 2);
            ChoiceBox difficulty = new ChoiceBox(0, 0, 250, 40, 5, "Difficulty", 1);
            NumberField seed = new NumberField(0, 0, 210, 40, 0);

            seed.yConstraints.Add(new MarginConstraint(110));
            seed.xConstraints.Add(new CenterConstraint());
            seed.xConstraints.Add(new AddConstraint(130));

            Button seedButton = new Button(0, 0, 30, 30, new Material(new Texture("Loop.png")));
            seedButton.yConstraints.Add(new MarginConstraint(115));
            seedButton.xConstraints.Add(new CenterConstraint());
            seedButton.xConstraints.Add(new AddConstraint(250 + 5));

            mapType.yConstraints.Add(new MarginConstraint(10));
            mapType.xConstraints.Add(new CenterConstraint());
            mapType.xConstraints.Add(new AddConstraint(150));
            mapType.AddChild(GetHoverText("Farmers Dream"));
            mapType.AddChild(GetHoverText("Desert"));
            mapType.AddChild(GetHoverText("Winterworld"));
            mapType.AddChild(GetHoverText("Iceland"));
            mapType.AddChild(GetHoverText("Moonbase"));
            mapType.AddChild(GetHoverText("Afterearth"));
            mapType.AddChild(GetHoverText("Rogue"));
            mapType.AddChild(GetHoverText("Riverful"));
            mapType.AddChild(GetHoverText("Oasis"));
            mapType.AddChild(GetHoverText("Steppe"));
            mapType.AddChild(GetHoverText("Tundra"));
            mapType.AddChild(GetHoverText("Darkness"));
            mapType.AddChild(GetHoverText("Islands"));

            difficulty.yConstraints.Add(new MarginConstraint(60));
            difficulty.xConstraints.Add(new CenterConstraint());
            difficulty.xConstraints.Add(new AddConstraint(150));
            difficulty.AddChild(GetHoverText("Easy"));
            difficulty.AddChild(GetHoverText("Not AS Easy"));
            difficulty.AddChild(GetHoverText("Medium"));
            difficulty.AddChild(GetHoverText("Hard"));
            difficulty.AddChild(GetHoverText("Nightmare"));

            tabPane.AddElementToTab(mapType, "General");
            tabPane.AddElementToTab(mapTypeText, "General");
            tabPane.AddElementToTab(difficulty, "General");
            tabPane.AddElementToTab(difficultyText, "General");
            tabPane.AddElementToTab(seedText, "General");
            tabPane.AddElementToTab(seed, "General");
            tabPane.AddElementToTab(seedButton, "General");
        }

        private void LoadMapTab(TabPane tabPane)
        {
            ScrollPane contentPane = new ScrollPane(0, 5, 730, 1f);
            contentPane.xConstraints.Add(new CenterConstraint());
            contentPane.heightConstraints.Add(new SubtractConstraint(10));

            tabPane.AddElementToTab(contentPane, "Map");

            Text resources = new Text(0, 0, "Resources", 1.3f);
            resources.yConstraints.Add(new MarginConstraint(10));
            resources.xConstraints.Add(new CenterConstraint());

            contentPane.AddChild(resources);

            Table table = new Table(10, 10, 700, 330);
            table.widthConstraints.Add(new SubtractConstraint(20));
            table.heightConstraints.Add(new SubtractConstraint(20));
            table.yConstraints.Add(new MarginConstraint(60));
            table.xConstraints.Add(new CenterConstraint());
            table.SetColumnCount(0.3f, 0.5333f, 0.7666f);
            table.SetRowCount(60, 110, 160, 210, 260, 310);



            contentPane.AddChild(table);

            Text size = new Text(0, 0, "Size", 0.8f);
            size.xConstraints.Add(new CenterConstraint());
            size.yConstraints.Add(new CenterConstraint());
            Text frequency = new Text(0, 0, "Frequency", 0.8f);
            frequency.xConstraints.Add(new CenterConstraint());
            frequency.yConstraints.Add(new CenterConstraint());
            Text richness = new Text(0, 0, "Richness", 0.8f);
            richness.xConstraints.Add(new CenterConstraint());
            richness.yConstraints.Add(new CenterConstraint());
            table.SetCell(size, 1, 0);
            table.SetCell(frequency, 2, 0);
            table.SetCell(richness, 3, 0);

            Text coal = new Text(0, 0, "Coal", 0.8f);
            coal.xConstraints.Add(new CenterConstraint());
            coal.yConstraints.Add(new CenterConstraint());
            Text iron = new Text(0, 0, "Iron", 0.8f);
            iron.xConstraints.Add(new CenterConstraint());
            iron.yConstraints.Add(new CenterConstraint());
            Text copper = new Text(0, 0, "Copper", 0.8f);
            copper.xConstraints.Add(new CenterConstraint());
            copper.yConstraints.Add(new CenterConstraint());
            Text gold = new Text(0, 0, "Gold", 0.8f);
            gold.xConstraints.Add(new CenterConstraint());
            gold.yConstraints.Add(new CenterConstraint());
            Text uranium = new Text(0, 0, "Uranium", 0.8f);
            uranium.xConstraints.Add(new CenterConstraint());
            uranium.yConstraints.Add(new CenterConstraint());
            table.SetCell(coal, 0, 1);
            table.SetCell(iron, 0, 2);
            table.SetCell(copper, 0, 3);
            table.SetCell(gold, 0, 4);
            table.SetCell(uranium, 0, 5);

            Slider coalSize = GetSlider();
            table.SetCell(coalSize, 1, 1);
            Slider coalFreq = GetSlider();
            table.SetCell(coalFreq, 2, 1);
            Slider coalRich = GetSlider();
            table.SetCell(coalRich, 3, 1);

            Slider ironSize = GetSlider();
            table.SetCell(ironSize, 1, 2);
            Slider ironFreq = GetSlider();
            table.SetCell(ironFreq, 2, 2);
            Slider ironRich = GetSlider();
            table.SetCell(ironRich, 3, 2);

            Slider copperSize = GetSlider();
            table.SetCell(copperSize, 1, 3);
            Slider copperFreq = GetSlider();
            table.SetCell(copperFreq, 2, 3);
            Slider copperRich = GetSlider();
            table.SetCell(copperRich, 3, 3);

            Slider goldSize = GetSlider();
            table.SetCell(goldSize, 1, 4);
            Slider goldFreq = GetSlider();
            table.SetCell(goldFreq, 2, 4);
            Slider goldRich = GetSlider();
            table.SetCell(goldRich, 3, 4);

            Slider uraniumSize = GetSlider();
            table.SetCell(uraniumSize, 1, 5);
            Slider uraniumFreq = GetSlider();
            table.SetCell(uraniumFreq, 2, 5);
            Slider uraniumRich = GetSlider();
            table.SetCell(uraniumRich, 3, 5);

            Text mapSettings = new Text(0, 0, "Map Settings", 1.3f);
            mapSettings.yConstraints.Add(new MarginConstraint(390));
            mapSettings.xConstraints.Add(new CenterConstraint());

            contentPane.AddChild(mapSettings);

            Table mapSettingsTable = new Table(10, 10, 700, 330);
            mapSettingsTable.widthConstraints.Add(new SubtractConstraint(20));
            mapSettingsTable.heightConstraints.Add(new SubtractConstraint(20));
            mapSettingsTable.yConstraints.Add(new MarginConstraint(440));
            mapSettingsTable.xConstraints.Add(new CenterConstraint());
            mapSettingsTable.SetColumnCount(0.3f, 0.5333f, 0.7666f);
            mapSettingsTable.SetRowCount(60, 110, 160, 210, 260, 310);

            Text mapSize = new Text(0, 0, "Size", 0.8f);
            mapSize.xConstraints.Add(new CenterConstraint());
            mapSize.yConstraints.Add(new CenterConstraint());
            Text mapFrequency = new Text(0, 0, "Frequency", 0.8f);
            mapFrequency.xConstraints.Add(new CenterConstraint());
            mapFrequency.yConstraints.Add(new CenterConstraint());
            Text mapStrength = new Text(0, 0, "Strength", 0.8f);
            mapStrength.xConstraints.Add(new CenterConstraint());
            mapStrength.yConstraints.Add(new CenterConstraint());
            mapSettingsTable.SetCell(mapSize, 1, 0);
            mapSettingsTable.SetCell(mapFrequency, 2, 0);
            mapSettingsTable.SetCell(mapStrength, 3, 0);


            Text mountains = new Text(0, 0, "Mountains", 0.8f);
            mountains.xConstraints.Add(new CenterConstraint());
            mountains.xConstraints.Add(new AddConstraint(20));
            mountains.yConstraints.Add(new CenterConstraint());
            Text water = new Text(0, 0, "Water", 0.8f);
            water.xConstraints.Add(new CenterConstraint());
            water.xConstraints.Add(new AddConstraint(20));
            water.yConstraints.Add(new CenterConstraint());
            Text vegetation = new Text(0, 0, "Vegetation", 0.8f);
            vegetation.xConstraints.Add(new CenterConstraint());
            vegetation.xConstraints.Add(new AddConstraint(20));
            vegetation.yConstraints.Add(new CenterConstraint());
            Text creatures = new Text(0, 0, "Creatures", 0.8f);
            creatures.xConstraints.Add(new CenterConstraint());
            creatures.xConstraints.Add(new AddConstraint(20));
            creatures.yConstraints.Add(new CenterConstraint());
            Text structures = new Text(0, 0, "Structures", 0.8f);
            structures.xConstraints.Add(new CenterConstraint());
            structures.xConstraints.Add(new AddConstraint(20));
            structures.yConstraints.Add(new CenterConstraint());


            TickBox mountainsBox = new TickBox(20, 0, 20, 20, true);
            mountainsBox.yConstraints.Add(new CenterConstraint());
            TickBox waterBox = new TickBox(20, 0, 20, 20, true);
            waterBox.yConstraints.Add(new CenterConstraint());
            TickBox vegetationBox = new TickBox(20, 0, 20, 20, true);
            vegetationBox.yConstraints.Add(new CenterConstraint());
            TickBox creaturesBox = new TickBox(20, 0, 20, 20, true);
            creaturesBox.yConstraints.Add(new CenterConstraint());
            TickBox structuresBox = new TickBox(20, 0, 20, 20, true);
            structuresBox.yConstraints.Add(new CenterConstraint());

            mapSettingsTable.SetCell(mountains, 0, 1);
            mapSettingsTable.SetCell(mountainsBox, 0, 1);
            mapSettingsTable.SetCell(water, 0, 2);
            mapSettingsTable.SetCell(waterBox, 0, 2);
            mapSettingsTable.SetCell(vegetation, 0, 3);
            mapSettingsTable.SetCell(vegetationBox, 0, 3);
            mapSettingsTable.SetCell(creatures, 0, 4);
            mapSettingsTable.SetCell(creaturesBox, 0, 4);
            mapSettingsTable.SetCell(structures, 0, 5);
            mapSettingsTable.SetCell(structuresBox, 0, 5);


            Slider mountainsSize = GetSlider();
            mapSettingsTable.SetCell(mountainsSize, 1, 1);
            mapSettingsTable.SetCellHoverText("Physical size of mountains", 1, 1);
            Slider mountainsFreq = GetSlider();
            mapSettingsTable.SetCell(mountainsFreq, 2, 1);
            mapSettingsTable.SetCellHoverText("Amount of mountains in the world", 2, 1);
            Slider mountainsStre = GetSlider();
            mapSettingsTable.SetCell(mountainsStre, 3, 1);
            mapSettingsTable.SetCellHoverText("Height of mountains", 3, 1);

            Slider waterSize = GetSlider();
            mapSettingsTable.SetCell(waterSize, 1, 2);
            mapSettingsTable.SetCellHoverText("Physical size of waters", 1, 2);
            Slider waterFreq = GetSlider();
            mapSettingsTable.SetCell(waterFreq, 2, 2);
            mapSettingsTable.SetCellHoverText("Amount of waters in the world", 2, 2);
            Slider waterStre = GetSlider();
            mapSettingsTable.SetCell(waterStre, 3, 2);
            mapSettingsTable.SetCellHoverText("The physical force water exerts", 3, 2);

            Slider vegetationSize = GetSlider();
            mapSettingsTable.SetCell(vegetationSize, 1, 3);
            mapSettingsTable.SetCellHoverText("Physical size of mountains", 1, 3);
            Slider vegetationFreq = GetSlider();
            mapSettingsTable.SetCell(vegetationFreq, 2, 3);
            mapSettingsTable.SetCellHoverText("Amount of mountains in the world", 2, 3);
            Slider vegetationStre = GetSlider();
            mapSettingsTable.SetCell(vegetationStre, 3, 3);
            mapSettingsTable.SetCellHoverText("Height of mountains", 3, 3);

            Slider creaturesSize = GetSlider();
            mapSettingsTable.SetCell(creaturesSize, 1, 4);
            mapSettingsTable.SetCellHoverText("The amount of creatures in each group", 1, 4);
            Slider creaturesFreq = GetSlider();
            mapSettingsTable.SetCell(creaturesFreq, 2, 4);
            mapSettingsTable.SetCellHoverText("Amount of creature groups in the world", 2, 4);
            Slider creaturesStre = GetSlider();
            mapSettingsTable.SetCell(creaturesStre, 3, 4);
            mapSettingsTable.SetCellHoverText("Strength of creatures", 3, 4);

            Slider structuresSize = GetSlider();
            mapSettingsTable.SetCell(structuresSize, 1, 5);
            mapSettingsTable.SetCellHoverText("Physical size of structures", 1, 5);
            Slider structuresFreq = GetSlider();
            mapSettingsTable.SetCell(structuresFreq, 2, 5);
            mapSettingsTable.SetCellHoverText("Amount of structures in the world", 2, 5);
            Slider structuresStre = GetSlider();
            mapSettingsTable.SetCell(structuresStre, 3, 5);
            mapSettingsTable.SetCellHoverText("Resistance of structures against attacks", 3, 5);


            contentPane.AddChild(mapSettingsTable);

        }

        private Slider GetSlider()
        {
            Slider slider = new Slider(8, 0, 1f, 20, 0, 200, 100);
            slider.widthConstraints.Add(new SubtractConstraint(13));
            slider.xConstraints.Add(new CenterConstraint());
            slider.yConstraints.Add(new CenterConstraint());
            return slider;
        }

        private TextSelectable GetHoverText(string text)
        {
            TextSelectable curText = new TextSelectable(0, 0, 250, 0, text, 0.7f, new Material(new Vector4(1, 1, 1, 0.5f)), new Material(new Vector4(1, 1, 1, 0.3f)));
            return curText;
        }

        private void NewGame(MouseEvent e, GuiElement el)
        {
            if (e.leftButtonDown)
                window.visible = true;
        }

        protected override void OnResize(EventArgs e)
        {
            GameSettings.Width = Width;
            GameSettings.Height = Height;

            GL.Viewport(0, 0, Width, Height);

            base.OnResize(e);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            scene.Update((float)e.Time);

            GameInput.Update();

        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            guiRenderer.PrepareRender();
            guiRenderer.Render(scene);

            this.SwapBuffers();
        }






        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            guiRenderer.CleanUp();
        }










        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            GameInput.UpdateKey(e.Key, false);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            GameInput.UpdateKey(e.Key, true);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            GameInput.mouseDx = GameInput.mouseX - e.X;
            GameInput.mouseDy = GameInput.mouseY - e.Y;

            GameInput.mouseX = e.X;
            GameInput.mouseY = e.Y;

            GameInput.normalizedMouseX = (2f * e.X) / Width - 1;
            GameInput.normalizedMouseY = -1 * ((2f * e.Y) / Height - 1);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            GameInput.mouseWheel = e.Delta;
            GameInput.mouseWheelF = e.DeltaPrecise;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            GameInput.mouseInside = false;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseLeave(e);
            GameInput.mouseInside = true;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            GameInput.UpdateMouseButton(e.Button, true);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            GameInput.UpdateMouseButton(e.Button, false);
        }
    }
}
