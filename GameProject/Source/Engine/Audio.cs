using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DenWild.Source.Engine;
using GameProject.Source.Engine;
using DenWild.World;
using GameProject.Source.GamePlay.world.Entities;
using GameProject.Source.GamePlay.world.Entities.FriendlyEntities;
using GameProject.Source.GamePlay.world.Entities.EnemyEntities;
using GameProject.Source.GamePlay.world.Entities.NeutralEntities;
using GameProject.Source.GamePlay.world.Entities.FriendlyBuilders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;
using System.Diagnostics;

namespace GameProject.Source.Engine
{
    public record Audio
    {
        private Song song1, song2, song3, song4;
        SoundEffect ClickSound;
        public Audio()
        {
            song1 = Globals.Content.Load<Song>("Audio\\Lyudvig");
            song2 = Globals.Content.Load<Song>("Audio\\Svyatoslav");
            song3 = Globals.Content.Load<Song>("Audio\\Violin");
            song4 = Globals.Content.Load<Song>("Audio\\Ringtone");
            ClickSound = Globals.Content.Load<SoundEffect>("Audio\\ClickSound");
            SoundEffect.MasterVolume = 1.0f;
            MediaPlayer.Play(song2);
            MediaPlayer.Play(song3);
            MediaPlayer.Play(song4);
            MediaPlayer.Play(song1);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.4f;

        }

        public void PlayAudio()
        {
            ClickSound.Play();
        }
    }
}
