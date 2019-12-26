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
using GUILib.Modern;
using GUILib.Events;

using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using GUILib.GUI.Animations.Transitions;
using GUILib.GUI.PixelConstraints;
using GUILib.GUI.Render.Fonts.Data;

namespace GUILib
{
    class ModernMenu
    {
        private GuiScene scene;
        public ModernMenu(GuiScene scene)
        {
            this.scene = scene;

            Font.defaultFont = FontFileParser.LoadFont("Arial");
            Font.defaultFont.xAdvance = 0.69f;

            LoadMenu();
        }

        private void LoadMenu()
        {
            LoadBackground();
            LoadTopMenu();
            LoadLevelBar();
            LoadLeftMenu();
            LoadChallengeMenu();
            LoadFriends();
        }

        private Quad minimizeQuad;
        private Button friendsButton;
        private Quad chat;
        private ScrollPane textChat;
        private Text userText;
        private Quad userIcon;


        private Dictionary<GuiElement, string> users = new Dictionary<GuiElement, string>();
        private Dictionary<GuiElement, Material> userMats = new Dictionary<GuiElement, Material>();

        private Random r = new Random(1);
        private int textHeight = 375;

        private Dictionary<Message, bool> messages = new Dictionary<Message, bool>();
         

        private void LoadFriends()
        {
            Quad friendsIcon = new Quad(0, 0, 48, 48, new Material(new Texture("Friends.png")));
            friendsIcon.xConstraints.Add(new CenterConstraint());
            friendsIcon.yConstraints.Add(new CenterConstraint());

            friendsButton = new Button(0, 5, 54, 54, "", false, 1, new Material(new Vector4(0.3f, 0.3f, 0.3f, 0.6f), new BorderData(new Vector4(0.4f, 0.4f, 0.4f, 0.8f), 1, true, 0.2f), new GradientData(1f, 0.5f, 0.4f)));
            friendsButton.xConstraints.Add(new MarginConstraint(5));
            friendsButton.mouseButtonReleasedEvent = FriendButtonClicked;

            friendsButton.AddChild(friendsIcon);
            scene.AddParent(friendsButton);

            chat = new Quad(0, 5, 0, 0, new Material(new Vector4(0.4f, 0.4f, 0.4f, 0.95f), new BorderData(new Vector4(0.6f, 0.6f, 0.6f, 0.95f), 1, true, 0.02f), new GradientData(1f, 0.01f, 0.3f)));
            chat.xConstraints.Add(new MarginConstraint(5));
            chat.ZIndex = 1;

            Quad verticalSeperator = new Quad(250, 0, 1, 1f, new Material(new Vector4(0.6f, 0.6f, 0.6f, 0.7f)));
            chat.AddChild(verticalSeperator);

            float brightnessDiv = 1.1f;

            Text leftText = new Text(65, 0, "Friend List", 0.8f);
            leftText.color = new Vector4(0.3f / brightnessDiv, 0.8f / brightnessDiv, 1f / brightnessDiv, 1f);
            leftText.yConstraints.Add(new MarginConstraint(10));
            chat.AddChild(leftText);


            Quad leftTextQuad = new Quad(120, 0, 120, 1, new Material(new Vector4(0.3f, 0.8f, 1f, 0.7f), null, new GradientData(1f, 1f, 110, false, false, false, true, false)));
            Quad leftTextQuad2 = new Quad(0, 0, 120, 1, new Material(new Vector4(0.3f, 0.8f, 1f, 0.7f), null, new GradientData(1f, 1f, 110, false, true, false, false, false)));
            leftTextQuad.yConstraints.Add(new MarginConstraint(40));
            leftTextQuad2.yConstraints.Add(new MarginConstraint(40));
            chat.AddChild(leftTextQuad);
            chat.AddChild(leftTextQuad2);

            ScrollPane users = new ScrollPane(0, 0, 250, 500 - 45, new Material(new Vector4(0)), new Material(new Vector4(0)), 5, new Material(new Vector4(0)), new Material(new Vector4(0.7f, 0.7f, 0.7f, 0.8f), new BorderData(new Vector4(0), 0, true, 0.5f)));
            users.overScroll = 0;
            users.yConstraints.Add(new MarginConstraint(45));

            CreateUser(0, "Username 1", users);
            CreateUser(70, "Username 2", users);
            CreateUser(140, "Username 3", users);
            CreateUser(210, "Username 4", users);
            CreateUser(280, "Username 5", users);
            CreateUser(350, "Username 6", users);
            CreateUser(420, "Username 7", users);
            CreateUser(490, "Username 8", users);
            CreateUser(560, "Username 9", users, true);

            chat.AddChild(users);


            userIcon = new Quad(265, 0, 48, 48, new Material(new Vector4((float)r.NextDouble() / 2 + 0.2f, (float)r.NextDouble() / 2 + 0.2f, (float)r.NextDouble() / 2 + 0.2f, 1f), new BorderData(new Vector4(0.8f, 0.8f, 0.8f, 1f), 2, true, 0.5f)));
            userIcon.yConstraints.Add(new MarginConstraint(10));

            userText = new Text(325, 32, "Username 1", 0.8f);
            userText.yConstraints.Add(new MarginConstraint(20));

            Text userClanText = new Text(325 + userText.curWidth + 5, 32, "#ClanTag", 0.6f);
            userClanText.color = new Vector4(0.3f, 0.8f, 1f, 0.7f);
            userClanText.yConstraints.Add(new MarginConstraint(25));

            chat.AddChild(userText);
            chat.AddChild(userClanText);

            Quad horizontal = new Quad(250, 0, 400, 1, new Material(new Vector4(0.6f, 0.6f, 0.6f, 0.7f)));
            horizontal.yConstraints.Add(new MarginConstraint(65));
            chat.AddChild(horizontal);
            chat.AddChild(userIcon);

            Button minimizeButton = new Button(0, 0, 20, 10, "", false, 1, new Material(new Vector4(0)));
            minimizeButton.defaultMaterial = new Material(new Vector4(0));
            minimizeButton.hoverMaterial = new Material(new Vector4(0));
            minimizeButton.clickMaterial = new Material(new Vector4(0));

            minimizeButton.yConstraints.Add(new MarginConstraint(15));
            minimizeButton.xConstraints.Add(new MarginConstraint(15));

            minimizeButton.startHoverEvent = StartHoverMinimize;
            minimizeButton.endHoverEvent = EndHoverMinimize;
            minimizeButton.mouseButtonReleasedEvent = EndClickMinimize;
            minimizeButton.mouseButtonDownEvent = StartClickMinimize;

            minimizeQuad = new Quad(0, 0, 15, 3, new Material(new Vector4(0.8f, 0.8f, 0.8f, 0.6f)));
            minimizeQuad.yConstraints.Add(new CenterConstraint());
            minimizeQuad.xConstraints.Add(new CenterConstraint());


            minimizeButton.AddChild(minimizeQuad);
            chat.AddChild(minimizeButton);

            TextField typeText = new TextField(255, 5, 390, 40, "Type here...", new Material(new Vector4(0.5f, 0.5f, 0.5f, 0.7f), new BorderData(new Vector4(0.7f, 0.7f, 0.7f, 0.7f))));
            typeText.keyEvent = TextFieldKeyPressed;

            chat.AddChild(typeText);

            textChat = new ScrollPane(255, 50, 390, 380, new Material(new Vector4(0)), new Material(new Vector4(0)), 5, new Material(new Vector4(0)), new Material(new Vector4(0.7f, 0.7f, 0.7f, 0.8f), new BorderData(new Vector4(0), 0, true, 0.5f)));
            AddMessage("hi", textChat);
            AddMessage("Hi", textChat, true);
            AddMessage("play a game?", textChat);
            AddMessage("5 mins", textChat, true);
            AddMessage("k", textChat);

            chat.AddChild(textChat);

            scene.AddParent(chat);





            AnimationKeyframe k1M = new AnimationKeyframe(0);
            AnimationKeyframe k2M = new AnimationKeyframe(0.2f);

            k1M.x = 0;
            k2M.x = -80;
            k2M.width = 54;
            k2M.height = 54;

            List<AnimationKeyframe> keyframesM = new List<AnimationKeyframe>();
            keyframesM.Add(k1M);
            keyframesM.Add(k2M);

            AnimationKeyframe c1M = new AnimationKeyframe(0);
            AnimationKeyframe c2M = new AnimationKeyframe(0.2f);

            c1M.x = 0;
            c2M.x = 80;
            c2M.width = -54;
            c2M.height = -54;

            List<AnimationKeyframe> cKeyframesM = new List<AnimationKeyframe>();
            cKeyframesM.Add(c1M);
            cKeyframesM.Add(c2M);

            AnimationClass animationStructM = new AnimationClass(AnimationType.PauseOnEnd, keyframesM, "PopUp");
            AnimationClass cAnimationStructM = new AnimationClass(AnimationType.PauseOnEnd, cKeyframesM, "PopUpReverse");
            animationStructM.transition = new SmoothstepTransition(3);
            cAnimationStructM.transition = new SmoothstepTransition(3);

            Animation aM = new Animation();
            aM.AddAnimation(animationStructM);
            aM.AddAnimation(cAnimationStructM);

            friendsButton.animation = aM;




            AnimationKeyframe k1 = new AnimationKeyframe(0);
            AnimationKeyframe k2 = new AnimationKeyframe(0.2f);

            k1.x = 0;
            k2.x = -650;
            k2.width = 650;
            k2.height = 500;

            List<AnimationKeyframe> keyframes = new List<AnimationKeyframe>();
            keyframes.Add(k1);
            keyframes.Add(k2);

            AnimationKeyframe c1 = new AnimationKeyframe(0);
            AnimationKeyframe c2 = new AnimationKeyframe(0.2f);

            c1.x = 0;
            c2.x = 650;
            c2.width = -650;
            c2.height = -500;

            List<AnimationKeyframe> cKeyframes = new List<AnimationKeyframe>();
            cKeyframes.Add(c1);
            cKeyframes.Add(c2);

            AnimationClass animationStruct = new AnimationClass(AnimationType.PauseOnEnd, keyframes, "PopUp");
            AnimationClass cAnimationStruct = new AnimationClass(AnimationType.PauseOnEnd, cKeyframes, "PopUpReverse");
            animationStruct.transition = new SmoothstepTransition(3);
            cAnimationStruct.transition = new SmoothstepTransition(3);


            Animation a = new Animation();
            a.AddAnimation(animationStruct);
            a.AddAnimation(cAnimationStruct);

            chat.animation = a;
            chat.debugIdentifier = "Y";
        }
        
        private void TextFieldKeyPressed(KeyEvent e, GuiElement el)
        {
            if (e.pressed.Contains(Key.Enter))
            {
                TextField field = ((TextField)el);

                if (field.IsSelected())
                {
                    string newText = field.GetText();
                    field.SetText("");
                    AddMessage(newText, textChat, true);
                }
            }
        }

        class Message
        {
            public string msg;

        }

        private void AddMessage(string text, GuiElement chat, bool right = false)
        {
            messages.Add(new Message { msg = text }, right);

            if (!right)
            {
                Quad textQuad = new Quad(0, textHeight - 40, 200, 35, new Material(new Vector4(0.7f, 0.7f, 0.7f, 0.7f), new BorderData(new Vector4(0.9f, 0.9f, 0.9f, 1f), 2, true, 0.2f)));
                Text textE = new Text(5, 0, text, 0.7f);
                textE.yConstraints.Add(new CenterConstraint());
                textQuad.AddChild(textE);
                chat.AddChild(textQuad);
            }
            else
            {
                Quad textQuad = new Quad(0, textHeight - 40, 200, 35, new Material(new Vector4(0.6f, 0.7f, 0.9f, 0.7f), new BorderData(new Vector4(0.9f, 0.9f, 0.9f, 1f), 2, true, 0.2f)));
                textQuad.xConstraints.Add(new MarginConstraint(10));
                Text textE = new Text(5, 0, text, 0.7f);
                textE.yConstraints.Add(new CenterConstraint());
                textQuad.AddChild(textE);
                chat.AddChild(textQuad);
            }
            textHeight -= 45;
        }

        private void CreateUser(int y, string user, GuiElement chat, bool last = false)
        {
            Button userQuad = new Button(0, y, 250, 70, "", false, 1, new Material(new Vector4(0)));
            userQuad.defaultMaterial = new Material(new Vector4(0));
            userQuad.hoverMaterial = new Material(new Vector4(1f, 1f, 1f, 0.2f), new BorderData(new Vector4(0f), 0, true, 0.19f));
            userQuad.clickMaterial = new Material(new Vector4(1f, 1f, 1f, 0.4f), new BorderData(new Vector4(0f), 0, true, 0.19f));
            userQuad.mouseButtonReleasedEvent = UserClicked;

            users.Add(userQuad, user);

            userQuad.yConstraints.Add(new MarginConstraint(y));

            Material userMaterial = new Material(new Vector4((float)r.NextDouble() / 2 + 0.2f, (float)r.NextDouble() / 2 + 0.2f, (float)r.NextDouble() / 2 + 0.2f, 1f), new BorderData(new Vector4(0.8f, 0.8f, 0.8f, 1f), 2, true, 0.5f));

            Quad icon = new Quad(20, 0, 48, 48, userMaterial);
            icon.yConstraints.Add(new CenterConstraint());

            userMats.Add(userQuad, userMaterial);

            Text userText = new Text(100, 32, user, 0.8f);
            userText.xConstraints.Add(new CenterConstraint());
            userText.xConstraints.Add(new AddConstraint(30));
            userQuad.AddChild(userText);

            Text userSubText = new Text(100, 10, "Some subtitle text", 0.6f);
            userSubText.color = new Vector4(0.8f, 0.8f, 0.8f, 1f);
            userSubText.xConstraints.Add(new CenterConstraint());
            userSubText.xConstraints.Add(new AddConstraint(30));
            userQuad.AddChild(userSubText);

            userQuad.AddChild(icon);

            if (!last)
            {
                Quad nextSeperator = new Quad(10, 0, 230, 1, new Material(new Vector4(0.8f, 0.8f, 0.8f, 1f)));
                userQuad.AddChild(nextSeperator);
            }

            chat.AddChild(userQuad);
        }

        private void UserClicked(MouseEvent e, GuiElement el)
        {
            userText.SetText(users[el]);
            userIcon.SetMaterial(userMats[el]);
        }

        private void LoadChallengeMenu()
        {
            Material quadMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 0.6f), new BorderData(new Vector4(0.6f, 0.6f, 0.6f, 0.5f), 1, true, 0.05f), new GradientData(2f, 0.4f, 0.4f));

            Vector4 darkColor = new Vector4(0.6f, 0.6f, 0.6f, 1f);

            Quad challengeQuad = new Quad(0, 0, 380, 500, quadMaterial);
            challengeQuad.yConstraints.Add(new CenterConstraint());
            challengeQuad.xConstraints.Add(new MarginConstraint(110));
            scene.AddParent(challengeQuad);

            Quad titleQuad = new Quad(0, 0, 250, 1, new Material(new Vector4(0.3f, 0.8f, 1f, 0.7f), null, new GradientData(1f, 1f, 250, false, false, false, true, false)));
            titleQuad.yConstraints.Add(new MarginConstraint(45));
            titleQuad.ZIndex = 1;
            challengeQuad.AddChild(titleQuad);

            Material refreshTexture = new Material(new Texture("Refresh.png"));
            refreshTexture.SetColor(darkColor);

            Quad refreshQuad = new Quad(0, 0, 24, 24, refreshTexture);
            refreshQuad.xConstraints.Add(new MarginConstraint(100));
            refreshQuad.yConstraints.Add(new MarginConstraint(18));
            challengeQuad.AddChild(refreshQuad);

            Text refreshText = new Text(0, 0, "23h 22m", 0.6f);
            refreshText.color = darkColor;
            refreshText.xConstraints.Add(new MarginConstraint(25));
            refreshText.yConstraints.Add(new MarginConstraint(20));
            challengeQuad.AddChild(refreshText);

            Vector4 textDefaultColor = new Vector4(0.3f, 0.8f, 1f, 0.7f);
            
            Text title = new Text(20, 0, "Daily Challenges", 0.7f);
            title.yConstraints.Add(new MarginConstraint(20));
            title.color = textDefaultColor;
            title.fontWidth = 0.38f;
            challengeQuad.AddChild(title);


            Material xpQuad = new Material(new Texture("XP.png"));
            Material epQuad = new Material(new Texture("EPGold.png"));


            Vector4 challengeTextColor = new Vector4(0.6f, 0.6f, 0.6f, 1f);

            Material progressBarMaterial = new Material(new Vector4(0.5f, 0.5f, 0.5f, 0.6f), new BorderData(new Vector4(0.5f, 0.5f, 0.5f, 0.7f), 1));

            Text challenge1 = new Text(20, 0, "Get 2 Kills with a pistol while being blinded", 0.5f);
            challenge1.color = challengeTextColor;
            challenge1.yConstraints.Add(new MarginConstraint(60));

            Quad progressBar1 = new Quad(20, 0, 1f, 10, progressBarMaterial);
            progressBar1.yConstraints.Add(new MarginConstraint(95));
            progressBar1.widthConstraints.Add(new SubtractConstraint(40));

            Quad xpQuad1 = new Quad(0, 0, 24, 24, xpQuad);
            xpQuad1.xConstraints.Add(new MarginConstraint(20));
            xpQuad1.yConstraints.Add(new MarginConstraint(66));

            Text progressText1 = new Text(20, 0, "0/2", 0.5f);
            progressText1.color = challengeTextColor;
            progressText1.yConstraints.Add(new MarginConstraint(110));

            challengeQuad.AddChild(xpQuad1);
            challengeQuad.AddChild(progressText1);
            challengeQuad.AddChild(progressBar1);
            challengeQuad.AddChild(challenge1);





            Text challenge2 = new Text(20, 0, "Finish 5 Games with less than 4 Deaths", 0.5f);
            challenge2.color = challengeTextColor;
            challenge2.yConstraints.Add(new MarginConstraint(140));

            Quad progressBar2 = new Quad(20, 0, 1f, 10, progressBarMaterial);
            progressBar2.yConstraints.Add(new MarginConstraint(175));
            progressBar2.widthConstraints.Add(new SubtractConstraint(40));

            Quad xpQuad2 = new Quad(0, 0, 24, 24, xpQuad);
            xpQuad2.xConstraints.Add(new MarginConstraint(20));
            xpQuad2.yConstraints.Add(new MarginConstraint(146));

            Text progressText2 = new Text(20, 0, "0/5", 0.5f);
            progressText2.color = challengeTextColor;
            progressText2.yConstraints.Add(new MarginConstraint(190));

            challengeQuad.AddChild(xpQuad2);
            challengeQuad.AddChild(progressText2);
            challengeQuad.AddChild(progressBar2);
            challengeQuad.AddChild(challenge2);






            Text challenge3 = new Text(20, 0, "Kill 3 enemies upside down", 0.5f);
            challenge3.color = challengeTextColor;
            challenge3.yConstraints.Add(new MarginConstraint(220));

            Quad progressBar3 = new Quad(20, 0, 1f, 10, progressBarMaterial);
            progressBar3.yConstraints.Add(new MarginConstraint(255));
            progressBar3.widthConstraints.Add(new SubtractConstraint(40));

            Quad xpQuad3 = new Quad(0, 0, 24, 24, xpQuad);
            xpQuad3.xConstraints.Add(new MarginConstraint(20));
            xpQuad3.yConstraints.Add(new MarginConstraint(226));

            Text progressText3 = new Text(20, 0, "0/3", 0.5f);
            progressText3.color = challengeTextColor;
            progressText3.yConstraints.Add(new MarginConstraint(270));

            challengeQuad.AddChild(xpQuad3);
            challengeQuad.AddChild(progressText3);
            challengeQuad.AddChild(progressBar3);
            challengeQuad.AddChild(challenge3);




            Text weekly = new Text(20, 0, "Weekly Challenges", 0.7f);
            weekly.yConstraints.Add(new MarginConstraint(300));
            weekly.color = textDefaultColor;
            weekly.fontWidth = 0.38f;
            challengeQuad.AddChild(weekly);

            Quad weeklyQuad = new Quad(0, 0, 250, 1, new Material(new Vector4(0.3f, 0.8f, 1f, 0.7f), null, new GradientData(1f, 1f, 250, false, false, false, true, false)));
            weeklyQuad.yConstraints.Add(new MarginConstraint(325));
            weeklyQuad.ZIndex = 1;
            challengeQuad.AddChild(weeklyQuad);




            Quad refreshWeeklyQuad = new Quad(0, 0, 24, 24, refreshTexture);
            refreshWeeklyQuad.xConstraints.Add(new MarginConstraint(85));
            refreshWeeklyQuad.yConstraints.Add(new MarginConstraint(298));
            challengeQuad.AddChild(refreshWeeklyQuad);

            Text refreshWeeklyText = new Text(0, 0, "4d 11h", 0.6f);
            refreshWeeklyText.color = darkColor;
            refreshWeeklyText.xConstraints.Add(new MarginConstraint(25));
            refreshWeeklyText.yConstraints.Add(new MarginConstraint(300));
            challengeQuad.AddChild(refreshWeeklyText);

            challengeQuad.AddChild(refreshWeeklyQuad);
            challengeQuad.AddChild(refreshWeeklyText);





            Text challenge4 = new Text(20, 0, "Kill 42 Enemies", 0.5f);
            challenge4.color = challengeTextColor;

            challenge4.yConstraints.Add(new MarginConstraint(340));

            Quad progressBar4 = new Quad(20, 0, 1f, 10, progressBarMaterial);
            progressBar4.yConstraints.Add(new MarginConstraint(375));
            progressBar4.widthConstraints.Add(new SubtractConstraint(40));

            Quad xpQuad4 = new Quad(0, 0, 24, 24, xpQuad);
            xpQuad4.xConstraints.Add(new MarginConstraint(50));
            xpQuad4.yConstraints.Add(new MarginConstraint(346));

            Quad epQuad4 = new Quad(0, 0, 24, 24, epQuad);
            epQuad4.xConstraints.Add(new MarginConstraint(20));
            epQuad4.yConstraints.Add(new MarginConstraint(346));

            Text progressText4 = new Text(20, 0, "0/3", 0.5f);
            progressText4.color = challengeTextColor;
            progressText4.yConstraints.Add(new MarginConstraint(390));

            challengeQuad.AddChild(xpQuad4);
            challengeQuad.AddChild(epQuad4);
            challengeQuad.AddChild(progressText4);
            challengeQuad.AddChild(progressBar4);
            challengeQuad.AddChild(challenge4);






            Text challenge5 = new Text(20, 0, "Finish 2 games with a K/D of 10 or greater", 0.5f);
            challenge5.color = challengeTextColor;
            challenge5.yConstraints.Add(new MarginConstraint(420));

            Quad progressBar5 = new Quad(20, 0, 1f, 10, progressBarMaterial);
            progressBar5.yConstraints.Add(new MarginConstraint(455));
            progressBar5.widthConstraints.Add(new SubtractConstraint(40));

            Quad xpQuad5 = new Quad(0, 0, 24, 24, xpQuad);
            xpQuad5.xConstraints.Add(new MarginConstraint(50));
            xpQuad5.yConstraints.Add(new MarginConstraint(426));

            Quad epQuad5 = new Quad(0, 0, 24, 24, epQuad);
            epQuad5.xConstraints.Add(new MarginConstraint(20));
            epQuad5.yConstraints.Add(new MarginConstraint(426));

            Text progressText5 = new Text(20, 0, "0/3", 0.5f);
            progressText5.color = challengeTextColor;
            progressText5.yConstraints.Add(new MarginConstraint(470));

            challengeQuad.AddChild(xpQuad5);
            challengeQuad.AddChild(epQuad5);
            challengeQuad.AddChild(progressText5);
            challengeQuad.AddChild(progressBar5);
            challengeQuad.AddChild(challenge5);
        }

        private void LoadLeftMenu()
        {
            Material quadMaterial = new Material(new Vector4(0.3f, 0.3f, 0.3f, 0.3f), new BorderData(new Vector4(0.6f, 0.6f, 0.6f, 0.5f), 1, true, 0.05f), new GradientData(2f, 1f, 4f));

            VerticalList menuList = new VerticalList(80, 0, 380, 0, 10);
            menuList.yConstraints.Add(new CenterConstraint());

            int height = 60;
            Vector4 textColor = new Vector4(0.28f, 0.74f, 0.9f, 0.8f);
            float fontWidth = 0.36f;
            float fontSize = 0.9f;
            Animation animation = GetLeftMenuAnimation();

            Quad rankedQuad = new Quad(0, 0, 380, height, quadMaterial);
            Text rankedText = new Text(20, 0, "RANKED GAME", fontSize);
            rankedText.color = textColor;
            rankedText.yConstraints.Add(new CenterConstraint());
            rankedText.fontWidth = fontWidth;
            rankedQuad.AddChild(rankedText);

            Quad normalGameQuad = new Quad(0, 0, 380, height, quadMaterial);
            Text normalGameText = new Text(20, 0, "NORMAL GAME", fontSize);
            normalGameText.color = textColor;
            normalGameText.yConstraints.Add(new CenterConstraint());
            normalGameText.fontWidth = fontWidth;
            normalGameQuad.AddChild(normalGameText);

            Quad gunfightQuad = new Quad(0, 0, 380, height, quadMaterial);
            Text gunfightText = new Text(20, 0, "GUNFIGHT", fontSize);
            gunfightText.color = textColor;
            gunfightText.yConstraints.Add(new CenterConstraint());
            gunfightText.fontWidth = fontWidth;
            gunfightQuad.AddChild(gunfightText);

            Quad deathmatchQuad = new Quad(0, 0, 380, height, quadMaterial);
            Text deathmatchText = new Text(20, 0, "DEATHMATCH", fontSize);
            deathmatchText.color = textColor;
            deathmatchText.yConstraints.Add(new CenterConstraint());
            deathmatchText.fontWidth = fontWidth;
            deathmatchQuad.AddChild(deathmatchText);

            Quad freeForAllQuad = new Quad(0, 0, 380, height, quadMaterial);
            Text freeForAllText = new Text(20, 0, "FREE FOR ALL", fontSize);
            freeForAllText.color = textColor;
            freeForAllText.yConstraints.Add(new CenterConstraint());
            freeForAllText.fontWidth = fontWidth;
            freeForAllQuad.AddChild(freeForAllText);

            Quad futureFightQuad = new Quad(0, 0, 380, height, quadMaterial);
            Text futureFightText = new Text(20, 0, "FUTURE FIGHT", fontSize);
            futureFightText.color = textColor;
            futureFightText.yConstraints.Add(new CenterConstraint());
            futureFightText.fontWidth = fontWidth;
            futureFightQuad.AddChild(futureFightText);

            menuList.AddChild(rankedQuad);
            menuList.AddChild(normalGameQuad);
            menuList.AddChild(gunfightQuad);
            menuList.AddChild(deathmatchQuad);
            menuList.AddChild(freeForAllQuad);
            menuList.AddChild(futureFightQuad);

            SetAnimation(rankedQuad, animation);
            SetAnimation(normalGameQuad, animation);
            SetAnimation(gunfightQuad, animation);
            SetAnimation(deathmatchQuad, animation);
            SetAnimation(freeForAllQuad, animation);
            SetAnimation(futureFightQuad, animation);

            scene.AddParent(menuList);
        }

        private void SetAnimation(Quad quad, Animation a)
        {
            quad.animation = a;

            quad.SetStartHoverAnimation("Fade", AnimationRunType.Run);
            quad.SetEndHoverAnimation("Fade", AnimationRunType.Swing);
            quad.SetLeftMouseButtonDownAnimation("Click", AnimationRunType.Run);
        }

        private Animation GetLeftMenuAnimation()
        {
            AnimationKeyframe k1 = new AnimationKeyframe(0);
            AnimationKeyframe k2 = new AnimationKeyframe(0.2f);

            k1.x = 0;
            k2.x = 0;
            k2.width = 5;
            k2.height = 5;
            k2.opacity = 0.3f;

            AnimationKeyframe c1 = new AnimationKeyframe(0);
            AnimationKeyframe c2 = new AnimationKeyframe(0.07f);

            c2.width = 2;
            c2.height = 2;
            c2.x = -1;
            c2.y = -1;

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

            return a;
        }

        int topBarHeight = 130;
        int levelHeight = 49;

        private void LoadLevelBar()
        {
            Quad levelProgressBackgroundQuad = new Quad(0, 0, 200, 40, new Material(new Vector4(0.3f, 0.3f, 0.3f, 1f), new BorderData(new Vector4(0.19f, 0.69f, 0.83f, 1f)), new GradientData(1f, 0.2f, 0.5f)));
            levelProgressBackgroundQuad.yConstraints.Add(new MarginConstraint(levelHeight));
            levelProgressBackgroundQuad.xConstraints.Add(new MarginConstraint(90));

            Quad levelProgressQuad = new Quad(2, 2, 42, 38, new Material(new Vector4(0.19f, 0.69f, 0.83f, 0.8f), null, new GradientData(1f, 0.2f, 0.5f)));
            levelProgressQuad.ZIndex = 1;
            levelProgressQuad.yConstraints.Add(new MarginConstraint(levelHeight));
            levelProgressQuad.xConstraints.Add(new MarginConstraint(90));

            Text percentageQuad = new Text(0, 0, "21%", 0.6f);
            percentageQuad.ZIndex = 2;
            percentageQuad.xConstraints.Add(new CenterConstraint());
            percentageQuad.yConstraints.Add(new CenterConstraint());

            levelProgressBackgroundQuad.AddChild(percentageQuad);

            scene.AddParent(levelProgressBackgroundQuad);
            scene.AddParent(levelProgressQuad);

            Text levelText = new Text(0, 0, "42", 0.8f);
            levelText.xConstraints.Add(new CenterConstraint());
            levelText.yConstraints.Add(new CenterConstraint());
            levelText.yConstraints.Add(new SubtractConstraint(1));

            Quad levelQuad = new Quad(0, 0, 75, 75, new Material(new Texture("LevelTexture.png")));
            levelQuad.ZIndex = 2;
            levelQuad.yConstraints.Add(new MarginConstraint(levelHeight - 19));
            levelQuad.xConstraints.Add(new MarginConstraint(50));

            levelQuad.AddChild(levelText);

            scene.AddParent(levelQuad);


            Text username = new Text(0, 0, "Username", 0.7f);
            username.yConstraints.Add(new MarginConstraint(levelHeight - 20));
            username.xConstraints.Add(new MarginConstraint(200));
            username.ZIndex = 1;
            username.fontWidth = 0.4f;

            //scene.AddParent(username);

            /*Quad friendIcon = new Quad(0, 0, 40, 40, new Material(new Texture("Settings.png")));
            friendIcon.yConstraints.Add(new MarginConstraint(levelHeight - 5));
            friendIcon.xConstraints.Add(new MarginConstraint(20));
            friendIcon.ZIndex = 1;
            scene.AddParent(friendIcon);*/


            Quad epIcon = new Quad(0, 0, 35, 35, new Material(new Texture("EPGold.png")));
            epIcon.yConstraints.Add(new MarginConstraint(levelHeight));
            epIcon.xConstraints.Add(new MarginConstraint(440));
            epIcon.ZIndex = 1;

            Text epText = new Text(0, 0, "23196", 0.8f);
            epText.yConstraints.Add(new MarginConstraint(levelHeight + 5));
            epText.xConstraints.Add(new MarginConstraint(365));
            epText.color = new Vector4(0.8f, 0.8f, 0.8f, 1f);
            epText.ZIndex = 1;

            scene.AddParent(epIcon);
            scene.AddParent(epText);


            Quad crystalIcon = new Quad(0, 0, 35, 35, new Material(new Texture("Crystal.png")));
            crystalIcon.yConstraints.Add(new MarginConstraint(levelHeight));
            crystalIcon.xConstraints.Add(new MarginConstraint(570));
            crystalIcon.ZIndex = 1;

            Text crystalText = new Text(0, 0, "120", 0.8f);
            crystalText.yConstraints.Add(new MarginConstraint(levelHeight + 5));
            crystalText.xConstraints.Add(new MarginConstraint(523));
            crystalText.color = new Vector4(0.8f, 0.8f, 0.8f, 1f);
            crystalText.ZIndex = 1;

            scene.AddParent(crystalIcon);
            scene.AddParent(crystalText);
        }

        private void LoadTopMenu()
        {
            Text title = new Text(80, 0, "MULTIPLAYER", 1.4f);
            title.color = new Vector4(0.3f, 0.8f, 1f, 1f);
            title.yConstraints.Add(new MarginConstraint(20));
            title.fontWidth = 0.38f;
            title.ZIndex = 1;

            scene.AddParent(title);

            Quad titleQuad = new Quad(0, 0, 430, 1, new Material(new Vector4(0.3f, 0.8f, 1f, 1f), null, new GradientData(1f, 1f, 430f, false, false, false, true, false)));
            titleQuad.yConstraints.Add(new MarginConstraint(64));
            titleQuad.ZIndex = 1;
            scene.AddParent(titleQuad);

            Quad topQuad = new Quad(0, 0, 1f, topBarHeight, new Material(new Vector4(0.1f, 0.1f, 0.1f, 0.6f), null, new GradientData(1f, 0.3f, 0.6f)));
            topQuad.yConstraints.Add(new MarginConstraint(0));
            scene.AddParent(topQuad);

            Quad topTextHighlighter = new Quad(0, 0, 1f, 2, new Material(new Vector4(0.7f, 0.7f, 0.7f, 0.4f), null, new GradientData(1f, 0.7f, GameSettings.Width / 2f, false, false, false, true, false)));
            topTextHighlighter.yConstraints.Add(new MarginConstraint(topBarHeight));
            scene.AddParent(topTextHighlighter);

            Material topBarMenuDefaultMaterial = new Material(new Vector4(0));
            Material topBarMenuHoverMaterial = new Material(new Vector4(0.8f, 0.8f, 0.8f, 0.15f), new BorderData(new Vector4(0), 0, true, 0.1f), new GradientData(1f, 0.5f, 0.2f, true));
            Material topBarMenuClickMaterial = new Material(new Vector4(0.8f, 0.8f, 0.8f, 0.25f), new BorderData(new Vector4(0), 0, true, 0.1f), new GradientData(1f, 0.6f, 0.2f, true));
            Material underlineDefaultMaterial = new Material(new Vector4(0));
            Material underlineHoverMaterial = new Material(new Vector4(0.8f, 0.8f, 0.8f, 0.7f));
            Material underlineClickMaterial = new Material(new Vector4(0.8f, 0.8f, 0.8f, 0.9f));

            float fontSizeMenu = 0.6f;
            Vector4 texColor = new Vector4(0.8f, 0.8f, 0.8f, 1f);

            UnderlinedButton play = new UnderlinedButton(100, 0, 150, 50, "PLAY", true, fontSizeMenu, topBarMenuDefaultMaterial, topBarMenuHoverMaterial, topBarMenuClickMaterial, underlineHoverMaterial, underlineHoverMaterial, underlineClickMaterial);
            play.SetTextColor(new Vector4(0.3f, 0.8f, 1f, 1f));
            play.defaultTextColor = new Vector4(0.3f, 0.8f, 1f, 1f);
            play.hoverTextColor  = new Vector4(0.3f, 0.8f, 1f, 1f);
            UnderlinedButton weapons = new UnderlinedButton(260, 0, 150, 50, "WEAPONS", true, fontSizeMenu, topBarMenuDefaultMaterial, topBarMenuHoverMaterial, topBarMenuClickMaterial, underlineDefaultMaterial, underlineHoverMaterial, underlineClickMaterial);
            weapons.SetTextColor(texColor);
            weapons.defaultTextColor = texColor;
            UnderlinedButton campaign = new UnderlinedButton(420, 0, 150, 50, "CAMPAIGN", true, fontSizeMenu, topBarMenuDefaultMaterial, topBarMenuHoverMaterial, topBarMenuClickMaterial, underlineDefaultMaterial, underlineHoverMaterial, underlineClickMaterial);
            campaign.SetTextColor(texColor);
            campaign.defaultTextColor = texColor;
            UnderlinedButton airplanes = new UnderlinedButton(580, 0, 150, 50, "AIRPLANES", true, fontSizeMenu, topBarMenuDefaultMaterial, topBarMenuHoverMaterial, topBarMenuClickMaterial, underlineDefaultMaterial, underlineHoverMaterial, underlineClickMaterial);
            airplanes.SetTextColor(texColor);
            airplanes.defaultTextColor = texColor;
            UnderlinedButton store = new UnderlinedButton(740, 0, 150, 50, "STORE", true, fontSizeMenu, topBarMenuDefaultMaterial, topBarMenuHoverMaterial, topBarMenuClickMaterial, underlineDefaultMaterial, underlineHoverMaterial, underlineClickMaterial);
            store.SetTextColor(texColor);
            store.defaultTextColor = texColor;
            UnderlinedButton more = new UnderlinedButton(900, 0, 150, 50, "MORE", true, fontSizeMenu, topBarMenuDefaultMaterial, topBarMenuHoverMaterial, topBarMenuClickMaterial, underlineDefaultMaterial, underlineHoverMaterial, underlineClickMaterial);
            more.SetTextColor(texColor);
            more.defaultTextColor = texColor;

            play.yConstraints.Add(new MarginConstraint(topBarHeight - 48));
            weapons.yConstraints.Add(new MarginConstraint(topBarHeight - 48));
            campaign.yConstraints.Add(new MarginConstraint(topBarHeight - 48));
            airplanes.yConstraints.Add(new MarginConstraint(topBarHeight - 48));
            store.yConstraints.Add(new MarginConstraint(topBarHeight - 48));
            more.yConstraints.Add(new MarginConstraint(topBarHeight - 48));

            play.ZIndex = 1;
            weapons.ZIndex = 1;
            campaign.ZIndex = 1;
            airplanes.ZIndex = 1;
            store.ZIndex = 1;
            more.ZIndex = 1;

            scene.AddParent(play);
            scene.AddParent(weapons);
            scene.AddParent(campaign);
            scene.AddParent(airplanes);
            scene.AddParent(store);
            scene.AddParent(more);
        }

        private void LoadBackground()
        {
            Quad background = new Quad(0, 0, 2560, 1440, new Material(new Texture("ModernBackground.jpg")));
            background.ZIndex = -4;
            background.xConstraints.Add(new CenterConstraint());
            background.yConstraints.Add(new CenterConstraint());
            Quad backgroundOverlay = new Quad(0, 0, 1f, 1f, new Material(new Vector4(0.1f, 0.17f, 0.19f, 0.7f), null, new GradientData(1f, 0.2f, 0.2f)));
            backgroundOverlay.ZIndex = -3;
            backgroundOverlay.xConstraints.Add(new CenterConstraint());
            backgroundOverlay.yConstraints.Add(new CenterConstraint());

            scene.AddParent(background);
            scene.AddParent(backgroundOverlay);
        }






        private void StartClickMinimize(MouseEvent arg1, GuiElement arg2)
        {
            minimizeQuad.SetMaterial(new Material(new Vector4(0.95f, 0.95f, 0.95f, 0.95f)));
        }
        private void EndHoverMinimize(MouseEvent arg1, GuiElement arg2)
        {
            minimizeQuad.SetMaterial(new Material(new Vector4(0.8f, 0.8f, 0.8f, 0.6f)));
        }
        private void StartHoverMinimize(MouseEvent arg1, GuiElement arg2)
        {
            minimizeQuad.SetMaterial(new Material(new Vector4(0.85f, 0.85f, 0.85f, 0.85f)));
        }
        private void FriendButtonClicked(MouseEvent arg1, GuiElement arg2)
        {
            chat.StartAnimation("PopUp");
            friendsButton.StartAnimation("PopUpReverse");
        }
        private void EndClickMinimize(MouseEvent e, GuiElement arg2)
        {
            if (e.leftButtonDown)
            {
                chat.StartAnimation("PopUpReverse");
                friendsButton.StartAnimation("PopUp");
            }
        }
    }
}
