using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitiativeTracker
{
    /// <summary>
    /// Interaction logic for DMNotesWindow.xaml
    /// </summary>
    public partial class DMNotesWindow : Window
    {        
        public void updateDMnotes(string adv)
        {
            var mainWin = ((MainWindow)Application.Current.MainWindow);

            bool exists = false;
            var notes = new List<MainWindow.DMNotes>();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\GT_InitiativeTracker";
            string dmnotespath = (path + @"\dmnotes.txt");

            if (File.Exists(dmnotespath))
            {
                notes = mainWin.loadDMnotes();

                foreach (var note in notes)
                {
                    if (note.ADV == adv)
                    {
                        note.NOTES = dmnoteBox.Text;
                        exists = true;
                    }
                }
            }

            if (exists == false)
            {
                notes.Add(new MainWindow.DMNotes()
                {
                    ADV = adv,
                    NOTES = dmnoteBox.Text
                });
            }

            mainWin.saveDMnotes(notes);
        }

        private string adv;

        public DMNotesWindow()
        {
            InitializeComponent();

            var mainWin = ((MainWindow)Application.Current.MainWindow);            
            
            mainWin.dmnotesButton.IsEnabled = false;

            adv = mainWin.currentPick;

            var loadNotes = mainWin.loadPlayerDeets(adv, "dmn");

            //updateDMnotes(adv);

            dmnoteBox.Text = loadNotes[0];
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void saveNotesButton_Click(object sender, RoutedEventArgs e)
        {
            updateDMnotes(adv);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        private void DungeonMasterNotes_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var mainWin = ((MainWindow)Application.Current.MainWindow);
            updateDMnotes(adv);
            mainWin.dmnotesButton.IsEnabled = true;
        }
    }
}
