using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using DenWild;
using DenWild.Source.Engine;
using DenWild.World;
using GameProject.Source.Engine;
using GameProject.Source.GamePlay.world.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using GameProject.Source.GamePlay.world.Entities.FriendlyEntities;
using GameProject.Source.GamePlay.world.Entities.EnemyEntities;
using GameProject.Source.GamePlay.world.Entities.NeutralEntities;
using GameProject.Source.GamePlay.world.Entities.FriendlyBuilders;
using System.Collections;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace GameProject.Source.GamePlay
{
    public record Education
    {
        SpriteFont Font;
        Basic2d NextButton, BackButton, EducationBackground;
        List<List<string>> EducationLists = new List<List<string>> { };
        int EducationNumber;
        public Education()
        {
            Font = Globals.Content.Load<SpriteFont>("Fonts\\EducationFont");
            EducationBackground = new Basic2d("2d\\Hp", new Vector2(1725, 225), new Vector2(400, 400), 0.21f);
            EducationBackground.Color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
            NextButton = new Basic2d("2d\\Next", new Vector2(1860, 400), new Vector2(100, 25), 0.2f);
            BackButton = new Basic2d("2d\\Back", new Vector2(1590, 400), new Vector2(100, 25), 0.2f);
            EducationNumber = 0;
            AddEducationList();
        }

        public void AddEducationList()
        {
            var text = "Для начала давайте научимся управлять камерой. Перемещать камеру по карте можно с помощью нажатия левой кнопкой мыши  на миникарту в левом нижнем углу, либо перемещая мышь на края экрана в направления желаемого движения камеры";
            var i = 0;
            AddList(text, i);
            text = "Давайте построим первую базу. Выберите билдера (желтого юнита на экране), щелкнув по нему левой кнопкой мыши или выделив его зажатием левой кнопки мыши.";
            i++;
            AddList(text, i);
            text = "Теперь вы можете увидеть сведения о выбранном юните в нижней части экрана. Кроме того в правом нижнем углу вы можете увидеть команды для управления юнитом.";
            i++;
            AddList(text, i);
            text = "Давайте построим здание. нажмите левой кнопкой мыши на кнопку строительства (кнопка с буквой B) в правом нижнем углу, либо нажмите горячую клавишу (B). Буквы на кнопках обозначают горячие клавиши этих кнопок.";
            i++;
            AddList(text, i);
            text = "Теперь в правом нижнем углу появилась новые кнопки нажав на которые можно построить здания, на кнопке видна цена строительства, если она зеленая, то значит нам хватает ресурсов для   строительства.";
            i++;
            AddList(text, i);
            text = "Нажмите на кнопку базы (кнопка с буквой B), после чего у нас появится мираж здания, если он зеленый, то место для строительства свободно, если же красное, то строительству что-то мешает, и нужно найти другое место для строительства. ";
            i++;
            AddList(text, i);
            text = "Выберите подходящее место для строительства и нажмите левую кнопку мыши. После чего строитель пойдет строить здание, до окончания строительства, здание не функционирует(не строит юнитов и не добывает ресурсы).";
            i++;
            AddList(text, i);
            text = "После окончания строительства базы, выделев здание можно построить юнитов, нажав на кнопку юнита, либо нажатием на горячую клавишу. Также нажатием правой кнопки мыши можно выбрать точку, куда будут направлятся юниты после строительства.";
            i++;
            AddList(text, i);
            text = "Давайте построим ещё одного билдера. Выделите базу и постройте билдера.";
            i++;
            AddList(text, i);
            text = "Теперь у нас есть доплнительный билдер. Давайте увеличем добычу  ресурсов. Для этого выделите билдера и постройте на синем кристале ещё один майнер для добычи ресурсов.";
            i++;
            AddList(text, i);
            text = "Теперь у нас достаточный прирост ресурсов для строителства большой армии. Давайте построим 5 дронов.";
            i++;
            AddList(text, i);
            text = "Для удобства выделения вы можете зажимать shift, чтобы добовлять юнитов в вы деление, или ctrl, чтобы удолять из выделения.";
            i++;
            AddList(text, i);
            text = "Для удобства управления вы можете использовать кнопки над миникартой с горячими клавишами (F1 и F2). При нажатие на кнопку F1 выделяются все билдеры. При нажатеи на кнопку F2 выделяются все боевые юниты.";
            i++;
            AddList(text, i);
            text = "Также для удобства вы можете забиндить свох юнитов или здания на цифры от 0 до 9. После чего, нажатием кнопок от 0 до 9 будет выделять юнитов, которых вы забиндили на эти кнопки.";
            i++;
            AddList(text, i);
            text = "Чтобы забиндить юнитов нужно нажать ctrl + [0-9], для того чтобы добавить юнитов в бинд нужно нажать shift + [0-9].";
            i++;
            AddList(text, i);
            text = "Теперь вы знаете все инструменты для выполнения поставленных задач. Выполните задачи указааные в левом верхнем углу.";
            i++;
            AddList(text, i);
        }

        public void AddList(string text, int index)
        {
            var getText = "";
            EducationLists.Add(new List<string> { });
            foreach (var a in text)
            {
                if (getText.Length >= 25 && a == ' ')
                {
                    EducationLists[index].Add(getText);
                    getText = "";
                }
                getText += a;
            }
            if(getText.Length != 0)
                EducationLists[index].Add(getText);
        }

        public void Update()
        {
            UpdateDrawButton();
            UpdateLists();
        }

        public void UpdateLists()
        {
            var mousePosition = Globals.Control.MouseState.Position;
            if ((mousePosition.X > NextButton.Position.X - 50 && mousePosition.X < NextButton.Position.X + 50 &&
                mousePosition.Y > 387 && mousePosition.Y < 413) && 
                Globals.Control.CheckOneLeftClick() && EducationNumber + 1 < EducationLists.Count)
            {
                EducationNumber++;
                Globals.Audio.PlayAudio();
            }
            if ((mousePosition.X > BackButton.Position.X - 50 && mousePosition.X < BackButton.Position.X + 50 &&
                mousePosition.Y > 387 && mousePosition.Y < 413) && Globals.Control.CheckOneLeftClick() && EducationNumber > 0)
            {
                EducationNumber--;
                Globals.Audio.PlayAudio();
            }
        }

        public void UpdateDrawButton() 
        {
            var mousePosition = Globals.Control.MouseState.Position;
            if (mousePosition.X > NextButton.Position.X - 50 && mousePosition.X < NextButton.Position.X + 50 && 
                mousePosition.Y > 387 && mousePosition.Y < 413)
                NextButton.Path = "2d\\ActiveNext";
            else
                NextButton.Path = "2d\\Next";
            if (mousePosition.X > BackButton.Position.X - 50 && mousePosition.X < BackButton.Position.X + 50 && 
                mousePosition.Y > 387 && mousePosition.Y < 413)
                BackButton.Path = "2d\\ActiveBack";
            else
                BackButton.Path = "2d\\Back";
        }

        public void Draw()
        {
            EducationBackground.Draw();
            for (int i = 0; i <  EducationLists[EducationNumber].Count; i++)
            {
                Globals.SpriteBatch.DrawString(Font, EducationLists[EducationNumber][i],
                    new Vector2(1530, 40 + 30 * i), Color.White);
            }
            NextButton.Draw();
            BackButton.Draw();
            Globals.SpriteBatch.DrawString(Font,
                (EducationNumber + 1).ToString() + "/" + EducationLists.Count.ToString(),
                new Vector2(1705, 390), Color.White);
        }
    }
}
