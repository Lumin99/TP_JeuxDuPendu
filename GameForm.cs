using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JeuxDuPendu.MyControls;


namespace JeuxDuPendu
{
    public partial class GameForm : Form
    {
        Random alea = new Random();
        List<string> ListeMot = new List<string>() /*{ "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" }*/;
        int counter,x;
        //string filepath = "‪C:\JeuxDuPendu\ListeDeMot.txt";
        StreamReader reader = new StreamReader(@"..\..\Resources\ListeDeMot.txt");
        string ligne;

        String mot;

        List<char> Lettre = new List<char>();
        int nTab;
        int Verif;

        // Initialisation de l'instance de la classe d'affichage du pendu.
        HangmanViewer _HangmanViewer = new HangmanViewer();

        /// <summary>
        /// Constructeur du formulaire de jeux
        /// </summary>
        public GameForm()
        {
            InitializeComponent();
            InitializeMyComponent();
            StartNewGame();
        }

        /// <summary>
        /// Initialisations des composant specifique a l'application
        /// </summary>
        private void InitializeMyComponent()
        {
            // On positionne le controle d'affichage du pendu dans panel1 : 
            panel1.Controls.Add(_HangmanViewer);
			
			// à la position 0,0
            _HangmanViewer.Location = new Point(0, 0);
			
			// et de la même taille que panel1
            _HangmanViewer.Size = panel1.Size;
        }

        /// <summary>
        /// Initialise une nouvelle partie
        /// </summary>
        public void StartNewGame()
        {
            
            _HangmanViewer.Reset();
                        
            Lettre.Clear();
            nTab = 0;
            Verif = 0;
            int i;

            ligne = reader.ReadLine();
            for (i = 0; ligne != null; i++)  // Pour chaque ligne
            {
                ListeMot.Add(ligne);
                ligne = reader.ReadLine(); // On passe à la ligne suivante
            }

            counter = 0;
            foreach (string m in ListeMot)
            {
                counter++;
            }
            x = alea.Next(counter);
            mot = ListeMot[x].ToString().ToUpper();
            //String mot = "ESIEE";

            // Methode de reinitialisation classe d'affichage du pendu.
            _HangmanViewer.Init(mot.Length);
            //Affichage du mot à trouver dans le label.
            lCrypedWord.Text = "";
            
            for (i = 0; i < mot.Length; i++)
            {
                lCrypedWord.Text += '_';
            }

            label1.Text = "";
        }


        /// <summary>
        /// Methode appelé lors de l'appui d'un touche du clavier, lorsque le focus est sur le bouton "Nouvelle partie"
        /// </summary>
        private void bReset_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressed(e.KeyChar);
        }

        /// <summary>
        /// Methode appelé lors de l'appui d'un touche du clavier, lorsque le focus est sur le forulaire
        /// </summary>
        private void GameForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressed(e.KeyChar);
        }

        /// <summary>
        /// Methode appelé lors de l'appui sur le bouton "Nouvelle partie"
        /// </summary>
        private void bReset_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private bool LetterNotInTab(char l)
        {
            if(nTab != 0)
            {
                foreach (char caractere in Lettre)
                {
                    if (l == caractere)
                    {
                        return false;
                    }
                }
            }
            
            Lettre.Add(l);
            nTab++;
            return true;
        }

        private void KeyPressed(char letter)
        {
            //HEEEEEERRRRRRRREEEEEEE
            letter = Char.ToUpper(letter);
            if (LetterNotInTab(letter)) {
                if (mot.Contains(letter))
                {
                    int i;
                    for (i = 0; i < mot.Length; i++)
                    {
                        if (letter == mot[i])
                        {
                            String before = lCrypedWord.Text.Remove(i) + letter;
                            String after = lCrypedWord.Text.Remove(0, i + 1);
                            lCrypedWord.Text = before + after;
                            _HangmanViewer.Incrémenter();
                        }
                    }
                }
                else
                {
                    // On avance le pendu d'une etape
                    _HangmanViewer.MoveNextStep();
                }
            }
            else
            {
                MessageBox.Show("Veuillez taper une lettre non présente dans le tableau");
            }

            if (Verif+1 == nTab) {
                label1.Text += Char.ToString(Lettre[nTab - 1]) + ' ';
                Verif = nTab;
            }
                        

            // Si le pendu est complet, le joueur à perdu.
            if (_HangmanViewer.IsGameOver)
            {
                MessageBox.Show("Vous avez perdu !");
                StartNewGame();
            }
            else
            {   
                if (_HangmanViewer.IsGameSuccesful)
                {
                    MessageBox.Show("Vous avez gagné !");
                    StartNewGame();
                }
            }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lCrypedWord_Click(object sender, EventArgs e)
        {

        }
    }

}
