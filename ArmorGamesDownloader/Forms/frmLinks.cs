using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArmorGamesDownloader
{
    public partial class frmLinks : Form
    {
        Dictionary<String, String> Links;
        public frmLinks()
        {
            InitializeComponent();
            Int32 BorderHeight = this.Height - this.ClientRectangle.Height;
            Links = new Dictionary<string, string>() 
            { 
                { "ArmorGames", "http://armorgames.com" },
                { "Kongregate", "http://www.kongregate.com" },
                { "NewGrounds", "http://www.newgrounds.com" },
                { "Two Towers Games", "http://twotowersgames.com" },
                { "thePodge", "http://www.thepodge.co.uk" },
                { "StickPage", "http://www.stickpage.com" },
                { "Likwid Games", "http://www.likwidgames.com" },
                { "Swartag", "http://swartag.com" },
                { "JayIsGames", "http://jayisgames.com" },
                { "Barbarian Games", "http://barbarian-games.com" },
                { "Not Doppler", "http://www.notdoppler.com" },
                { "Gamesfree.ca", "http://www.gamesfree.ca/" },                
                { "Crazy Monkey Games", "http://www.crazymonkeygames.com" },
                { "Max Games", "http://www.maxgames.com" },
                { "ArcadeBomb", "http://www.arcadebomb.com/" },
                { "ArcadeTown", "http://www.arcadetown.com/" },
                { "flashgames247", "http://www.flashgames247.com" },
                { "Belugerin Studios", "http://www.belugerinstudios.com/" },
                { "Box10", "http://www.box10.com/" },
                { "GamesFree.com", "http://www.gamesfree.com/" },
                { "Funny Games", "http://www.funnygames.ru/" }
            };

            Int32 count = Links.Count;
            Button temp;
            for (int i = count - 1; i >= 0; i--)
            {
                temp = new Button()
                {
                    Text = Links.Keys.ElementAt(i),
                    Width = 120,
                    Height = 24,
                    Dock = DockStyle.Top,
                    TabIndex = i
                };
                temp.Click += btnLink_Click;
                this.Controls.Add(temp);
            }
            this.Height = 24 * count + BorderHeight;
        }

        void btnLink_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Links[(sender as Button).Text]);
        }
        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start((sender as LinkLabel).Tag.ToString());
        }
    }
}
