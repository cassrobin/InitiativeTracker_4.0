using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace InitiativeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\InitiativeTracker";
        string partypath = (path + @"\party.txt");
        string dmnotespath = (path + @"\dmnotes.txt");
        string savedinipath = (path + @"\savedinis.txt");
        string next = Environment.NewLine;

               

        public void savePartyDeets(List<Player> newPlayers)
        {
            try
            {
                var existingPlayers = new List<Player>();
                var allPlayers = new List<Player>();
                var adv = newPlayers[0].ADV;
                bool stop = false;

                if (File.Exists(partypath))
                {
                    bool replace = false;
                    existingPlayers = loadPartyDeets();

                    foreach (var advs in existingPlayers)
                    {
                        if (advs.ADV == adv)
                        {
                            MessageBoxResult result = MessageBox.Show("This party name already exists. Do you want to replace it with this one?", "Replace old party?", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                            if (result == MessageBoxResult.Cancel)
                                stop = true;

                            if (result == MessageBoxResult.OK)
                                replace = true;

                            break;
                        }
                    }

                    if (replace == true)
                    {
                        foreach (var oldadv in existingPlayers)
                        {
                            if (oldadv.ADV != adv)
                            {
                                allPlayers.Add(oldadv);
                            }
                        }
                    }
                    else
                        allPlayers = existingPlayers;

                    if (stop == false)
                    {
                        foreach (var player in newPlayers)
                            allPlayers.Add(player);
                    }
                }
                else
                { allPlayers = newPlayers; }

                var strOutb = JsonConvert.SerializeObject(allPlayers, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
                File.WriteAllText(partypath, strOutb);

                if (stop == false)
                {
                    var advNames = loadAdvNames();
                    var newAdv = advNames[advNames.Count - 1];
                    campName.Text = newAdv;
                    updateLists(newAdv);

                    newPartyBorder.Visibility = Visibility.Hidden;
                    partyBorder.Visibility = Visibility.Hidden;
                    editPartyBorder.Visibility = Visibility.Hidden;
                    campBorder.Visibility = Visibility.Visible;
                    battleSetupUIBorder.Visibility = Visibility.Visible;
                }
            }

            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }

        public void saveDMnotes(List<DMNotes> notes)
        {
            try
            {
                var strOutb = JsonConvert.SerializeObject(notes, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects }) + next;
                File.WriteAllText(dmnotespath, strOutb);
            }

            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }

        public void saveInitiatives(List<Initiatives> inis)
        {
            try
            {
                var existingInitiatives = new List<Initiatives>();
                var allInitiatives = new List<Initiatives>();
                var adv = inis[0].ADV;
                bool stop = false;

                if (File.Exists(savedinipath))
                {
                    bool replace = false;
                    existingInitiatives = loadAllInitiatives();

                    if (battleSetupBorder.Visibility == Visibility.Visible)
                    {
                        foreach (var advs in existingInitiatives)
                        {
                            if (advs.ADV == adv)
                            {
                                MessageBoxResult result = MessageBox.Show("This party name already exists. Do you want to replace it with this one?", "Replace old party?", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                                if (result == MessageBoxResult.Cancel)
                                {
                                    stop = true;
                                    break;
                                }

                                if (result == MessageBoxResult.OK)
                                {
                                    replace = true;
                                    break;
                                }
                            }
                        }
                    }
                    //replace = true;


                    if (replace == true)
                    {
                        foreach (var oldadv in existingInitiatives)
                        {
                            if (oldadv.ADV != adv)
                            {
                                allInitiatives.Add(oldadv);
                            }
                        }
                    }
                    else
                        allInitiatives = existingInitiatives;

                    if (stop == false)
                    {
                        foreach (var fighter in inis)
                            allInitiatives.Add(fighter);
                    }
                }

                else
                { allInitiatives = inis; }

                var strOutb = JsonConvert.SerializeObject(inis, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects }) + next;
                File.WriteAllText(savedinipath, strOutb);
            }

            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
        }

        public List<Player> loadPartyDeets()
        {
            try
            {
                var allParties = File.ReadAllText(partypath);
                var obj = JsonConvert.DeserializeObject<List<Player>>(allParties,
                   new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
                return obj ?? new List<Player>();
            }

            catch (FileNotFoundException f)
            {
                System.Console.WriteLine(f.ToString());
                return new List<Player>();
            }
            catch (DirectoryNotFoundException d)
            {
                System.Console.WriteLine(d.ToString());
                return new List<Player>();
            }
        }

        public List<DMNotes> loadDMnotes()
        {
            try
            {
                var allNotes = File.ReadAllText(dmnotespath);
                var obj = JsonConvert.DeserializeObject<List<DMNotes>>(allNotes,
                   new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
                return obj ?? new List<DMNotes>();
            }

            catch (FileNotFoundException f)
            {
                System.Console.WriteLine(f.ToString());
                return new List<DMNotes>();
            }
            catch (DirectoryNotFoundException d)
            {
                System.Console.WriteLine(d.ToString());
                return new List<DMNotes>();
            }
        }
        
        public List<Initiatives> loadAllInitiatives()
        {
            try
            {
                var allInitiatives = File.ReadAllText(savedinipath);
                var obj = JsonConvert.DeserializeObject<List<Initiatives>>(allInitiatives,
                   new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
                return obj ?? new List<Initiatives>();
            }

            catch (FileNotFoundException f)
            {
                System.Console.WriteLine(f.ToString());
                return new List<Initiatives>();
            }
            catch (DirectoryNotFoundException d)
            {
                System.Console.WriteLine(d.ToString());
                return new List<Initiatives>();
            }
        }

        public static List<String> loadConditions()
        {
            var cons = new List<string>();
            return cons = Properties.Resources.Conditions.Split(',').ToList<string>();
        }

        public List<string> loadAdvNames()
        {
            var camps = new List<string>();
            var parties = loadPartyDeets();

            foreach (var adv in parties)
            {
                if (camps.Contains(adv.ADV))
                    continue;
                else
                    camps.Add(adv.ADV);
            }
            //camps.Add("New Party");

            return camps;
        }

        public List<string> loadPlayerDeets(string adv, string deet)
        {
            var ply = new List<string>();
            var rc = new List<string>();
            var cls = new List<string>();
            var bg = new List<string>();
            var ac = new List<string>();
            var pp = new List<string>();
            var dmn = new List<string>();
            var whoops = new List<string>();
            string[] pars = { "ply", "rc", "cls", "bg", "ac", "pp", "dmn" };


            var parties = loadPartyDeets();
            foreach (var advs in parties)
            {
                if (advs.ADV == adv)
                {
                    ply.Add(advs.IRL + " AKA " + advs.RPG);
                    rc.Add(advs.RPG + " the " + advs.Race);
                    cls.Add(advs.RPG + " the " + advs.Cass);
                    bg.Add(advs.RPG + " the " + advs.BG);
                    ac.Add(advs.RPG + " - " + advs.AC.ToString());
                    pp.Add(advs.RPG + " - " + advs.PP.ToString());
                }
                //else return message about needing stuff
            }

            if (File.Exists(dmnotespath))
            {
                var notes = loadDMnotes();
                foreach (var note in notes)
                {
                    if (note.ADV == adv)
                        dmn.Add(note.NOTES);
                    else dmn.Add(adv);
                }
            }
            else dmn.Add(adv);

            if (deet == pars[0]) return ply;
            else if (deet == pars[1]) return rc;
            else if (deet == pars[2]) return cls;
            else if (deet == pars[3]) return bg;
            else if (deet == pars[4]) return ac;
            else if (deet == pars[5]) return pp;
            else if (deet == pars[6]) return dmn;
            else if (!pars.Contains(deet))
                whoops.Add("Check loadPartyDeets method call - a parameter is incorrectly set.");

            return whoops;
        }

        public List<Initiatives> loadInitiatives(string adv)
        {
            var inis = new List<Initiatives>();

            if (File.Exists(savedinipath))
            {
                var allfights = loadAllInitiatives();

                foreach (var advs in allfights)
                {
                    if (advs.ADV == adv)
                    {
                        inis.Add(new Initiatives()
                        {
                            ADV = adv,
                            ROLL = advs.ROLL,
                            FOE = advs.FOE,
                            NAME = advs.NAME,
                            DMG = advs.DMG,
                            STATUS = advs.STATUS,
                            NOTE = advs.NOTE,
                            CONLIST = loadConditions()
                        });
                    }
                }
            }
            return inis;
        }



        public string hello
        {
            get
            {
                return "Greetings and Welcome!" +
                    next + next + "On the next page, you'll be asked to enter your players' information to be stored for future sessions." +
                    next + next + "If you're in a rush - don't fret!  Just enter what you need and you can add the rest later.";
            }
        }

        public class Player : INotifyPropertyChanged
        {
            private int ini;
            private int pp;
            private int ac;

            protected void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, e);
            }

            protected void OnPropertyChanged(string propertyName)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }

            public string ADV { get; set; }
            public string IRL { get; set; }
            public string RPG { get; set; }
            public string Race { get; set; }
            public string Cass { get; set; }
            public string BG { get; set; }
            public int PP
            {
                get { return pp; }
                set
                {
                    if (value != pp)
                    {
                        pp = value;
                        OnPropertyChanged("PP");
                    }
                }
            }

            public int AC
            {
                get { return ac; }
                set
                {
                    if (value != ac)
                    {
                        ac = value;
                        OnPropertyChanged("AC");
                    }
                }
            }

            public int INI
            {
                get { return ini; }
                set
                {
                    if (value != ini)
                    {
                        ini = value;
                        OnPropertyChanged("INI");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public class Baddies : INotifyPropertyChanged
        {
            private int ini;
            private int hp;
            protected void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, e);
            }

            protected void OnPropertyChanged(string propertyName)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }

            public string ADV { get; set; }
            public string BAD { get; set; }
            public int HP 
            {
                get { return hp; }
                set
                {
                    if (value != hp)
                    {
                        hp = value;
                        OnPropertyChanged("HP");
                    }
                }
            }
            public int INI
            {
                get { return ini; }
                set
                {
                    if (value != ini)
                    {
                        ini = value;
                        OnPropertyChanged("INI");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public class DMNotes
        {
            public string ADV { get; set; }
            public string NOTES { get; set; }
        }

        public class Initiatives : INotifyPropertyChanged
        {
            private List<string> status;
            protected void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, e);
            }

            protected void OnPropertyChanged(string propertyName)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }

            public string ADV { get; set; }
            public bool FOE { get; set; }
            public int ROLL { get; set; }
            public string NAME { get; set; }
            public int DMG { get; set; }
            public string NOTE { get; set; }
            public List<string> CONLIST { get; set; }

            public List<string> STATUS
            {
                get { return status; }
                set
                {
                    if (value != status)
                    {
                        status = value;
                        OnPropertyChanged("STATUS");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public MessageBoxResult msgbox(string msg)
        {
            var box = MessageBox.Show(msg);
            return box;
        }

        public void CloseAllPopups()
        {
            playersPopup.IsOpen = false;
            racesPopup.IsOpen = false;
            classesPopup.IsOpen = false;
            bgsPopup.IsOpen = false;
            acPopup.IsOpen = false;
            ppPopup.IsOpen = false;
        }

        public string currentPick
        {
            get
            {
                string adv;

                if (File.Exists(partypath))
                {
                    var listBox = campList;
                    var listBoxPick = listBox.SelectedItem;

                    if (listBoxPick != null)
                        adv = listBoxPick.ToString();

                    else
                    {
                        var advList = loadAdvNames();
                        adv = advList[0];
                    }
                }

                else
                    adv = partyName.Text;

                return adv;
            }
        }

        public string FirstAdv
        {
            get
            {
                string firstAdv;

                if (File.Exists(partypath))
                {
                    var advList = loadAdvNames();
                    firstAdv = advList[0];
                }

                else
                { firstAdv = "Create Your Party!"; }

                return firstAdv;
            }
        }

        public List<string> AdvList
        {
            get
            {
                List<string> advList = new List<string>();

                if (File.Exists(partypath))
                {
                    advList = loadAdvNames();
                    return advList;
                }

                else
                {
                    advList.Add("No Parties Listed");
                    return advList;
                }
            }
        }

        public List<String> PlayList
        {
            get
            {
                List<String> playList = new List<String>();

                if (File.Exists(partypath))
                {
                    var partyList = loadPartyDeets();
                    var advList = loadAdvNames();
                    var firstAdv = advList[0];

                    foreach (var player in partyList)
                    {
                        if (player.ADV == firstAdv)
                            playList.Add(player.IRL + " AKA " + player.RPG);
                    }
                    return playList;
                }

                else
                {
                    playList.Add("Players missing.");
                    return playList;
                }
            }
        }

        public List<String> RaceList
        {
            get
            {
                List<String> raceList = new List<String>();

                if (File.Exists(partypath))
                {
                    var partyList = loadPartyDeets();
                    var advList = loadAdvNames();
                    var firstAdv = advList[0];

                    foreach (var player in partyList)
                    {
                        if (player.ADV == firstAdv)
                            raceList.Add(player.RPG + " the " + player.Race);
                    }
                    return raceList;
                }

                else
                {
                    raceList.Add("Player details missing.");
                    return raceList;
                }
            }
        }

        public List<String> ClassList
        {
            get
            {
                List<String> classList = new List<String>();

                if (File.Exists(partypath))
                {
                    var partyList = loadPartyDeets();
                    var advList = loadAdvNames();
                    var firstAdv = advList[0];

                    foreach (var player in partyList)
                    {
                        if (player.ADV == firstAdv)
                            classList.Add(player.RPG + " the " + player.Cass);
                    }
                    return classList;
                }

                else
                {
                    classList.Add("Player details missing.");
                    return classList;
                }
            }
        }

        public List<String> BGList
        {
            get
            {
                List<String> bgList = new List<String>();

                if (File.Exists(partypath))
                {
                    var partyList = loadPartyDeets();
                    var advList = loadAdvNames();
                    var firstAdv = advList[0];

                    foreach (var player in partyList)
                    {
                        if (player.ADV == firstAdv)
                            bgList.Add(player.RPG + " the " + player.BG);
                    }
                    return bgList;
                }

                else
                {
                    bgList.Add("Player details missing.");
                    return bgList;
                }
            }
        }

        public List<String> ACList
        {
            get
            {
                List<String> acList = new List<String>();

                if (File.Exists(partypath))
                {
                    var partyList = loadPartyDeets();
                    var advList = loadAdvNames();
                    var firstAdv = advList[0];

                    foreach (var player in partyList)
                    {
                        if (player.ADV == firstAdv)
                            acList.Add(player.RPG + " - " + player.AC);
                    }
                    return acList;
                }

                else
                {
                    acList.Add("Player details missing.");
                    return acList;
                }
            }
        }

        public List<String> PPList
        {
            get
            {
                List<String> ppList = new List<String>();

                if (File.Exists(partypath))
                {
                    var partyList = loadPartyDeets();
                    var advList = loadAdvNames();
                    var firstAdv = advList[0];

                    foreach (var player in partyList)
                    {
                        if (player.ADV == firstAdv)
                            ppList.Add(player.RPG + " - " + player.PP);
                    }
                    return ppList;
                }

                else
                {
                    ppList.Add("Player details missing.");
                    return ppList;
                }
            }
        }

        public string Notes
        {
            get
            {
                string dmnotes = "guten tag";

                if (File.Exists(dmnotespath))
                {
                    var dmnotesList = loadDMnotes();
                    var advList = loadAdvNames();
                    var firstAdv = advList[0];

                    foreach (var note in dmnotesList)
                    {
                        if (note.ADV == firstAdv)
                            dmnotes = note.NOTES;
                    }
                }
                else
                    dmnotes = "Enter notes here.";

                return dmnotes;
            }
        }

        public List<String> ConditionList
        {
            get
            {
                List<String> cons = new List<String>();
                cons = loadConditions();

                return cons;
            }
        }

        public Initiatives NewFighter()
        {
            var fighter = new Initiatives
            {
                ADV = currentPick,
                FOE = false,
                NAME = "Name",
                ROLL = 14,
                DMG = 42,
                STATUS = null,
                NOTE = null,
                CONLIST = loadConditions()
            };

            return fighter;
        }            


        public Visibility updateLists(string adv)
        {
            var playList = loadPlayerDeets(adv, "ply");
            var raceList = loadPlayerDeets(adv, "rc");
            var classList = loadPlayerDeets(adv, "cls");
            var bgList = loadPlayerDeets(adv, "bg");
            var acList = loadPlayerDeets(adv, "ac");
            var ppList = loadPlayerDeets(adv, "pp");
            var dmNotes = loadPlayerDeets(adv, "dmn");

            playersList.ItemsSource = playList;
            racesList.ItemsSource = raceList;
            classesList.ItemsSource = classList;
            bgsList.ItemsSource = bgList;
            acsList.ItemsSource = acList;
            ppsList.ItemsSource = ppList;

            return navButtons.Visibility = Visibility.Visible;
        }


        public Player GetDefaultPlayerDeets()
        {
            var defaultDeets = new Player
            {
                IRL = "player name",
                RPG = "character name",
                Race = "gnome",
                Cass = "druid/wizard",
                BG = "charlatan",
                PP = 20,
                AC = 20,
                INI = 0
            };
            return defaultDeets;
        }

        public void AutoSizeColumns(GridViewColumn hdr)
        {
            if (double.IsNaN(hdr.Width))
                hdr.Width = hdr.ActualWidth;

            hdr.Width = double.NaN;
        }

        //public void rotateScroller(ScrollBar sb)
        //{           
        //    sb.Value = 0;
        //    sb.Minimum = 0;
        //    sb.Maximum = 999;
        //    sb.SmallChange = 1;
        //    sb.Orientation = Orientation.Vertical;

        //    var rotate = new RotateTransform();
        //    var group = new TransformGroup();

        //    rotate.Angle = 180;
        //    group.Children.Add(rotate);

        //    sb.RenderTransform = group;
        //    sb.RenderTransformOrigin = new Point(0.5, 0.5);
        //}






        //start
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

         

            //if (advs.ADV == adv)
            //{
            //    MessageBoxResult result = MessageBox.Show("This party name already exists. Do you want to replace it with this one?", "Replace old party?", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            //    if (result == MessageBoxResult.Cancel)
            //        stop = true;

            //    if (result == MessageBoxResult.OK)
            //        replace = true;

            //    break;
            //}


            if (File.Exists(partypath))
            {
                welcomeBorder.Visibility = Visibility.Hidden;
                newPartyBorder.Visibility = Visibility.Hidden;
                partyBorder.Visibility = Visibility.Hidden;

                var advNames = loadAdvNames();
                var adv = advNames[0];
                campName.Text = adv;

                campBorder.Visibility = Visibility.Visible;
                battleSetupUIBorder.Visibility = Visibility.Visible;
                updateLists(adv);

                partyName.Focus();

                if (File.Exists(savedinipath))
                {
                    var fighters = loadInitiatives(adv);

                    if (fighters.Count > 0)
                        loadBattle.Visibility = Visibility.Visible;
                }
            }

            else
                welcomeOK.Focus();
        }


        private void welcomeOK_Click(object sender, RoutedEventArgs e)
        {
            welcomeBorder.Visibility = Visibility.Hidden;
            newPartyBorder.Visibility = Visibility.Visible;
            goBack.Visibility = Visibility.Hidden;
            partyBorder.Visibility = Visibility.Hidden;

            partyName.Focus();
        }

        private void partyGo_Click(object sender, RoutedEventArgs e)
        {
            var adv = partyName.Text;
            var pnt = partyNum.Text;

            if (String.IsNullOrEmpty(adv))
                msgbox("You need to enter a party name.");

            else if (String.IsNullOrEmpty(pnt) || (Convert.ToInt32(pnt) <= 0))
                msgbox("You need to enter the number of players.");

            else
            {
                var play = new List<Player>();
                int pn = (Convert.ToInt32(partyNum.Text));

                for (var p = 0; p < pn; p++)
                    play.Add(GetDefaultPlayerDeets());

                newPartyList.ItemsSource = play;
                partyBorder.Visibility = Visibility.Visible;
            }
        }

        private void partyNum_KeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            var oldNum = Convert.ToInt32(tb.Text);
            var newNum = oldNum;

            if (e.IsDown && e.Key == Key.Down)
            {
                e.Handled = true;

                if (oldNum > 0)
                {
                    newNum = (oldNum - 1);
                    tb.Text = newNum.ToString();
                }
            }

            if (e.IsDown && e.Key == Key.Up)
            {
                e.Handled = true;

                if (oldNum < 999)
                {
                    newNum = (oldNum + 1);
                    tb.Text = newNum.ToString();
                }
            }

            if (e.Key == Key.Return)
                partyGo_Click(null, null);
        }

        private void partyName_Enter(object sender, KeyboardFocusChangedEventArgs e)
        { partyName.SelectAll(); }

        private void editPartyName_Enter(object sender, KeyboardFocusChangedEventArgs e)
        { editPartyName.SelectAll(); }

        private void partyNum_Enter(object sender, KeyboardFocusChangedEventArgs e)
        { partyNum.SelectAll(); }

        private void button_OnFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBlock)(sender as Button).Content;
            tb.Foreground = new SolidColorBrush(Colors.Black);
            tb.Background = new SolidColorBrush(Colors.Ivory);
        }

        private void button_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBlock)(sender as Button).Content;
            tb.Foreground = new SolidColorBrush(Colors.Ivory);
            tb.Background = new SolidColorBrush(Colors.Black);                        
        }

        private void block_MouseEnter(object sender, RoutedEventArgs e)
        {
            var box = sender as TextBlock;
            box.Foreground = new SolidColorBrush(Colors.Black);
            box.Background = new SolidColorBrush(Colors.Ivory);
        }

        private void block_MouseLeave(object sender, RoutedEventArgs e)
        {
            var box = sender as TextBlock;
            box.Foreground = new SolidColorBrush(Colors.Ivory);
            box.Background = new SolidColorBrush(Colors.Black);
        }

        private void numbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            var oldNum = Convert.ToInt32(tb.Text);
            var newNum = oldNum;

            if (e.IsDown && e.Key == Key.Down)
            {
                e.Handled = true;

                if (oldNum > 0)
                {
                    newNum = (oldNum - 1);
                    tb.Text = newNum.ToString();
                }               
            }

            if (e.IsDown && e.Key == Key.Up)
            {
                e.Handled = true;

                if (oldNum < 999)
                {
                    newNum = (oldNum + 1);
                    tb.Text = newNum.ToString();
                }
            }
        }

        

        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            newPartyBorder.Visibility = Visibility.Hidden;
            editPartyBorder.Visibility = Visibility.Hidden;
            partyBorder.Visibility = Visibility.Hidden;

            var advNames = loadAdvNames();
            var adv = advNames[0];
            campName.Text = adv;

            campBorder.Visibility = Visibility.Visible;
            battleSetupUIBorder.Visibility = Visibility.Visible;
            updateLists(adv);

        }

        private void goBack2_Click(object sender, RoutedEventArgs e)
        {
            editPartyBorder.Visibility = Visibility.Hidden;
            partyBorder.Visibility = Visibility.Hidden;
            newPartyBorder.Visibility = Visibility.Hidden;

            var advNames = loadAdvNames();
            var adv = advNames[0];
            campName.Text = adv;

            campBorder.Visibility = Visibility.Visible;
            battleSetupUIBorder.Visibility = Visibility.Visible;
            updateLists(adv);
        }

        private void addPlayer_Click(object sender, RoutedEventArgs e)
        {
            var adv = partyName.Text;
            var play = new List<Player>();

            foreach (var item in newPartyList.Items.OfType<Player>())
            {
                play.Add(new Player()
                {
                    ADV = adv,
                    IRL = item.IRL,
                    RPG = item.RPG,
                    Race = item.Race,
                    Cass = item.Cass,
                    BG = item.BG,
                    PP = item.PP,
                    AC = item.AC
                });
            }

            play.Add(GetDefaultPlayerDeets());
            int pn = play.Count();
            partyNum.Text = pn.ToString();
            newPartyList.ItemsSource = play;
        }

        private void delPlayer_Click(object sender, RoutedEventArgs e)
        {
            var db = sender as Button;
            var dbContext = (sender as Button).DataContext;
            int dbIndex = newPartyList.Items.IndexOf(dbContext);

            var adv = partyName.Text;
            var play = new List<Player>();

            foreach (var item in newPartyList.Items.OfType<Player>())
            {
                if (newPartyList.Items.IndexOf(item) == dbIndex)
                    continue;

                play.Add(new Player()
                {
                    ADV = adv,
                    IRL = item.IRL,
                    RPG = item.RPG,
                    Race = item.Race,
                    Cass = item.Cass,
                    BG = item.BG,
                    PP = item.PP,
                    AC = item.AC
                });
            }

            int pn = play.Count();
            partyNum.Text = pn.ToString();
            newPartyList.ItemsSource = play;
        }

        
        private void saveParty_Click(object sender, RoutedEventArgs e)
        {
            var adv = partyName.Text;
            int pn = (Convert.ToInt32(partyNum.Text));
            var play = new List<Player>();
            foreach (var item in newPartyList.Items.OfType<Player>())
            {
                play.Add(new Player()
                {
                    ADV = adv,
                    IRL = item.IRL,
                    RPG = item.RPG,
                    Race = item.Race,
                    Cass = item.Cass,
                    BG = item.BG,
                    PP = item.PP,
                    AC = item.AC
                });
            }

            savePartyDeets(play);
        }

        private void TextBox_Enter(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.SelectAll();
        }

        private void campButton_Click(object sender, RoutedEventArgs e)
        {
            campList.ItemsSource = loadAdvNames();
            campListPopup.IsOpen = true;
        }

        private void campList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox != null)
            {
                campListPopup.IsOpen = false;

                var mainWin = Application.Current.Windows;
                foreach (Window wnd in mainWin)
                {
                    if (wnd is DMNotesWindow)
                        wnd.Close();
                }
            }
        }

        private void campListPopup_Closed(object sender, EventArgs e)
        {
            var listBox = campList;
            var listBoxPick = listBox.SelectedItem;
            string newPick = currentPick;

            if (listBoxPick != null)
            {
                newPick = listBoxPick.ToString();
                campName.Text = newPick;
                updateLists(newPick);
            }

            if (newPartyBorder.Visibility == Visibility.Visible)
                navButtons.Visibility = Visibility.Hidden;

            if (battleSetupBorder.Visibility == Visibility.Visible)
                battleSetupBorder.Visibility = Visibility.Hidden;


            if (File.Exists(savedinipath))
            {
                var fighters = loadInitiatives(newPick);

                if (fighters.Count > 0)
                    loadBattle.Visibility = Visibility.Visible;
                else
                    loadBattle.Visibility = Visibility.Hidden;
            }

            else
                loadBattle.Visibility = Visibility.Hidden;
        }


        private void playersButton_Click(object sender, RoutedEventArgs e)
        {
            playersPopup.IsOpen = true;
        }

        private void playersPopup_Closed(object sender, EventArgs e)
        {
            playersPopup.IsOpen = false;
        }

        private void racesButton_Click(object sender, RoutedEventArgs e)
        {
            racesPopup.IsOpen = true;
        }

        private void racesPopup_Closed(object sender, EventArgs e)
        {
            racesPopup.IsOpen = false;
        }

        private void classesButton_Click(object sender, RoutedEventArgs e)
        {
            classesPopup.IsOpen = true;
        }

        private void classesPopup_Closed(object sender, EventArgs e)
        {
            classesPopup.IsOpen = false;
        }

        private void bgsButton_Click(object sender, RoutedEventArgs e)
        {
            bgsPopup.IsOpen = true;
        }

        private void bgsPopup_Closed(object sender, EventArgs e)
        {
            bgsPopup.IsOpen = false;
        }

        private void acButton_Click(object sender, RoutedEventArgs e)
        {
            acPopup.IsOpen = true;
        }

        private void acPopup_Closed(object sender, EventArgs e)
        {
            acPopup.IsOpen = false;
        }

        private void ppButton_Click(object sender, RoutedEventArgs e)
        {
            ppPopup.IsOpen = true;
        }

        private void ppPopup_Closed(object sender, EventArgs e)
        {
            ppPopup.IsOpen = false;
        }

        private void dmnotesButton_Click(object sender, RoutedEventArgs e)
        {
            var butt = sender as Button;

            Point buttPoint = butt.TransformToAncestor(this).Transform(new Point(0, 0));

            var dmnWindow = new DMNotesWindow();
            dmnWindow.Top = buttPoint.Y + 50;
            dmnWindow.Left = buttPoint.X - 40;
            dmnWindow.Show();            
        }

        private void InitiativeTracker_Closing(object sender, CancelEventArgs e)
        {
            var mainWin = Application.Current.Windows;
            foreach (Window wnd in mainWin)
            {
                if (wnd is DMNotesWindow)
                    wnd.Close();
            }
        }

        private void newPartyButton_Click(object sender, RoutedEventArgs e)
        {
            campListPopup.IsOpen = false;
            campBorder.Visibility = Visibility.Hidden;
            navButtons.Visibility = Visibility.Hidden;
            battleSetupUIBorder.Visibility = Visibility.Hidden;
            battleSetupBorder.Visibility = Visibility.Hidden;

            var mainWin = Application.Current.Windows;
            foreach (Window wnd in mainWin)
            {
                if (wnd is DMNotesWindow)
                    wnd.Close();
            }

            newPartyBorder.Visibility = Visibility.Visible;
            partyName.Focus();
        }

        private void editPartyButton_Click(object sender, RoutedEventArgs e)
        {
            var eb = sender as Button;
            if (battleSetupBorder.Visibility == Visibility.Visible)
                eb.IsEnabled = false;

            else
            {
                var adv = currentPick;

                CloseAllPopups();

                partyName.Text = adv;
                editPartyName.Text = adv;
                var play = new List<Player>();
                var parties = loadPartyDeets();

                foreach (var advs in parties)
                {
                    if (advs.ADV == adv)
                        play.Add(advs);
                }
                int pn = play.Count();
                newPartyList.ItemsSource = play;

                campListPopup.IsOpen = false;
                campBorder.Visibility = Visibility.Hidden;
                navButtons.Visibility = Visibility.Hidden;
                battleSetupUIBorder.Visibility = Visibility.Hidden;
                battleSetupBorder.Visibility = Visibility.Hidden;


                var mainWin = Application.Current.Windows;
                foreach (Window wnd in mainWin)
                {
                    if (wnd is DMNotesWindow)
                        wnd.Close();
                }

                editPartyBorder.Visibility = Visibility.Visible;
                partyBorder.Visibility = Visibility.Visible;
            }

        }

        private void guestsBox_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null) return;

            guestsQ.Visibility = Visibility.Visible;
            guestsNum.Visibility = Visibility.Visible;
            guestsNumScroller.Visibility = Visibility.Visible;

        }

        private void guestsBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null) return;

            guestsQ.Visibility = Visibility.Hidden;
            guestsNum.Visibility = Visibility.Hidden;
            guestsNumScroller.Visibility = Visibility.Hidden;

        }

        private void lairBox_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null) return;
        }

        private void lairBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null) return;

        }

        private void getreadyButton_Click(object sender, RoutedEventArgs e)
        {
            battleSetupBorder.Visibility = Visibility.Visible;

            var adv = currentPick;
            var play = new List<Player>();
            var parties = loadPartyDeets();

            foreach (var advs in parties)
            {
                if (advs.ADV == adv)
                    play.Add(advs);
            }

            playersBattleList.ItemsSource = play;

            if (guestsBox.IsChecked == true)
            {
                var guestNum = guestsNum.Text;
                int gn = Convert.ToInt32(guestNum);

                if (gn > 0)
                {
                    var guestList = new List<Player>();

                    for (var star = 0; star < gn; star++)
                        guestList.Add(GetDefaultPlayerDeets());

                    guestsBattleList.ItemsSource = guestList;
                    guestsBattlePanel.Visibility = Visibility.Visible;
                }                
            }
            else
                guestsBattlePanel.Visibility = Visibility.Collapsed;

            var baddieNum = baddiesNum.Text;
            var bn = Convert.ToInt32(baddieNum);

            if (bn > 0)
            {
                var baddieList = new List<Baddies>();

                for (var bad = 0; bad < bn; bad++)
                {
                    baddieList.Add(new Baddies()
                    {
                        ADV = adv,
                        BAD = "monster" + (bad + 1),
                        HP = 10,
                        INI = 0
                    });
                }

                baddiesBattleList.ItemsSource = baddieList;
                baddiesBattlePanel.Visibility = Visibility.Visible;
            }
            else
                baddiesBattlePanel.Visibility = Visibility.Collapsed;
            
        }

        private void rollD20_Click(object sender, RoutedEventArgs e)
        {
            var d20 = new Random((int)DateTime.Now.Ticks);
            var ini = d20.Next(1, 21);

            var rb = sender as Button;
            var rollDC = rb.DataContext;

            var listView = new ListView().Items;

            //make these a method(type, listview, object)
            var hero = rollDC as Player;
            if (hero != null)
            {
                if (playersBattleList.Items.Contains(hero))
                {
                    listView = playersBattleList.Items;
                    var row = listView.IndexOf(hero);

                    foreach (var item in listView.OfType<Player>())
                    {
                        if (listView.IndexOf(item) == row)
                        {
                            item.INI = ini;
                        }
                    }
                }

                if (guestsBattleList.Items.Contains(hero))
                {
                    listView = guestsBattleList.Items;
                    var row = listView.IndexOf(hero);

                    foreach (var item in listView.OfType<Player>())
                    {
                        if (listView.IndexOf(item) == row)
                        {
                            item.INI = ini;
                        }
                    }
                }
            }

            var villain = rollDC as Baddies;
            if (villain != null)
            {
                if (baddiesBattleList.Items.Contains(villain))
                {
                    listView = baddiesBattleList.Items;
                    var row = listView.IndexOf(villain);

                    foreach (var item in listView.OfType<Baddies>())
                    {
                        if (listView.IndexOf(item) == row)
                        {
                            item.INI = ini;
                        }
                    }
                }
            }


        }

        private void deliniGuest_Click(object sender, RoutedEventArgs e)
        {
            var db = sender as Button;
            var dbContext = (sender as Button).DataContext;
            int dbIndex = guestsBattleList.Items.IndexOf(dbContext);

            var adv = currentPick;
            var play = new List<Player>();

            foreach (var item in guestsBattleList.Items.OfType<Player>())
            {
                if (guestsBattleList.Items.IndexOf(item) == dbIndex)
                    continue;

                play.Add(new Player()
                {
                    ADV = adv,
                    IRL = item.IRL,
                    RPG = item.RPG,
                    Race = item.Race,
                    Cass = item.Cass,
                    BG = item.BG,
                    PP = item.PP,
                    AC = item.AC
                });
            }

            int pn = play.Count();
            guestsBattleList.ItemsSource = play;
        }

        private void deliniBaddie_Click(object sender, RoutedEventArgs e)
        {
            var db = sender as Button;
            var dbContext = (sender as Button).DataContext;
            int dbIndex = baddiesBattleList.Items.IndexOf(dbContext);

            var adv = currentPick;
            var bad = new List<Baddies>();

            foreach (var item in baddiesBattleList.Items.OfType<Baddies>())
            {
                if (baddiesBattleList.Items.IndexOf(item) == dbIndex)
                    continue;

                bad.Add(new Baddies()
                {
                    ADV = adv,
                    BAD = item.BAD,
                    HP = item.HP,
                    INI = item.INI
                });
            }

            int pn = bad.Count();
            baddiesBattleList.ItemsSource = bad;
        }

        private void clearInis_Click(object sender, RoutedEventArgs e)
        {
            battleSetupBorder.Visibility = Visibility.Hidden;
            guestsBox.IsChecked = false;
            guestsNum.Text = "2";
            lairBox.IsChecked = false;
            baddiesNum.Text = "2";
        }




        private void addGuest_Click(object sender, RoutedEventArgs e)
        {
            var adv = currentPick;
            var guests = new List<Player>();

            foreach (var item in guestsBattleList.Items.OfType<Player>())
            {
                guests.Add(new Player()
                {
                    ADV = adv,
                    RPG = item.RPG,
                    INI = item.INI
                });
            }

            guests.Add(GetDefaultPlayerDeets());
            int gn = guests.Count();
            guestsNum.Text = gn.ToString();
            guestsBattleList.ItemsSource = guests;
        }

        private void addBaddie_Click(object sender, RoutedEventArgs e)
        {
            var adv = currentPick;
            var baddies = new List<Baddies>();

            foreach (var item in baddiesBattleList.Items.OfType<Baddies>())
                baddies.Add(new Baddies()
                {
                    ADV = adv,
                    BAD = item.BAD,
                    HP = item.HP,
                    INI = item.INI
                });

            baddies.Add(new Baddies()
            {
                ADV = adv,
                BAD = "monster" + (baddiesBattleList.Items.Count + 1),
                HP = 10,
                INI = 0
            });

            int bn = baddies.Count();
            baddiesNum.Text = bn.ToString();
            baddiesBattleList.ItemsSource = baddies;
        }

        private void delGuest_Click(object sender, RoutedEventArgs e)
        {
            var db = sender as Button;
            var dbContext = (sender as Button).DataContext;
            int dbIndex = guestsBattleList.Items.IndexOf(dbContext);

            var adv = partyName.Text;
            var guest = new List<Player>();

            foreach (var item in guestsBattleList.Items.OfType<Player>())
            {
                if (guestsBattleList.Items.IndexOf(item) == dbIndex)
                    continue;

                guest.Add(new Player()
                {
                    ADV = adv,
                    RPG = item.RPG,
                    INI = 0
                });
            }

            int gn = guest.Count();
            guestsNum.Text = gn.ToString();
            guestsBattleList.ItemsSource = guest;
        }

        private void delBaddie_Click(object sender, RoutedEventArgs e)
        {
            var db = sender as Button;
            var dbContext = (sender as Button).DataContext;
            int dbIndex = baddiesBattleList.Items.IndexOf(dbContext);

            var adv = partyName.Text;
            var baddie = new List<Baddies>();

            foreach (var item in baddiesBattleList.Items.OfType<Baddies>())
            {
                if (baddiesBattleList.Items.IndexOf(item) == dbIndex)
                    continue;

                baddie.Add(new Baddies()
                {
                    ADV = adv,
                    BAD = item.BAD,
                    HP = item.HP,
                    INI = item.INI
                });
            }

            int bn = baddie.Count();
            baddiesNum.Text = bn.ToString();
            baddiesBattleList.ItemsSource = baddie;
        }

        //findme
        private void loadBattle_Click(object sender, RoutedEventArgs e)
        {
            var adv = currentPick;

            bool load = false;
            var allInis = loadAllInitiatives();
            var theseInis = loadInitiatives(adv);

            foreach (var ini in allInis)
            {
                if (ini.ADV == adv)
                {
                    MessageBoxResult result = MessageBox.Show("This party has saved initiatives. Do you want to replace load them now?", "Load Battle?", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.OK)
                        load = true;

                    break;
                }
            }

            if (load == true)
            {                
                var fighters = new List<Initiatives>();

                foreach (var fighter in theseInis)
                {
                    fighters.Add(new Initiatives()
                    {
                        ADV = adv,
                        ROLL = fighter.ROLL,
                        FOE = fighter.FOE,
                        NAME = fighter.NAME,
                        DMG = fighter.DMG,
                        STATUS = fighter.STATUS,
                        NOTE = fighter.NOTE,
                        CONLIST = fighter.CONLIST
                    });
                }
                
                fighters.Sort((p, q) => p.ROLL.CompareTo(q.ROLL));
                fighters.Reverse();

                //initiativesList.Items.Clear();
                initiativesList.ItemsSource = fighters;

                battleSetupUIBorder.Visibility = Visibility.Hidden;
                battleSetupBorder.Visibility = Visibility.Hidden;
                initiativesBorder.Visibility = Visibility.Visible;
                campButton.IsEnabled = false;
            }
        }

        private void sortButton_Click(object sender, RoutedEventArgs e)
        {
            var adv = currentPick;
            var fighters = new List<Initiatives>();

            foreach (var item in playersBattleList.Items.OfType<Player>())
            {
                fighters.Add(new Initiatives()
                {
                    ADV = adv,
                    ROLL = item.INI,
                    FOE = false,                    
                    NAME = item.RPG,
                    DMG = 0,
                    STATUS = null,
                    NOTE = null,
                    CONLIST = loadConditions()
                });
            }

            foreach (var item in guestsBattleList.Items.OfType<Player>())
            {
                fighters.Add(new Initiatives()
                {                    
                    ADV = adv,
                    ROLL = item.INI,
                    FOE = false,
                    NAME = item.RPG,
                    DMG = 0,
                    STATUS = null,
                    NOTE = null,
                    CONLIST = loadConditions()
                });
            }

            foreach (var item in baddiesBattleList.Items.OfType<Baddies>())
            {
                fighters.Add(new Initiatives()
                {
                    ADV = adv,
                    ROLL = item.INI,
                    FOE = true,                    
                    NAME = item.BAD,
                    DMG = item.HP,
                    STATUS = null,
                    NOTE = null,
                    CONLIST = loadConditions()
                });
            }

            fighters.Sort((p, q) => p.ROLL.CompareTo(q.ROLL));
            fighters.Reverse();
            saveInitiatives(fighters);
            

            initiativesList.ItemsSource = fighters;

            battleSetupUIBorder.Visibility = Visibility.Hidden;
            battleSetupBorder.Visibility = Visibility.Hidden;
            initiativesBorder.Visibility = Visibility.Visible;
            campButton.IsEnabled = false;
        }

        int row = 0;

        private void statusListButton_Click(object sender, EventArgs e)
        {
            var butt = sender as Button;
            var buttDB = butt.DataContext;
            row = initiativesList.Items.IndexOf(buttDB);

            statusList.ItemsSource = loadConditions();
            statusListPopup.IsOpen = true;
        }

        private void statusListPopup_Open(object sender, EventArgs e)
        {
            var cons = new List<string>();

            foreach (var item in initiativesList.Items.OfType<Initiatives>())
            {
                if (initiativesList.Items.OfType<Initiatives>().ToList().IndexOf(item) == row)
                {
                    cons = item.STATUS;

                    if (cons != null)
                    {
                        foreach (var stat in statusList.Items)
                        {
                            if (cons.Contains(stat))
                                statusList.SelectedItems.Add(stat);
                        }
                    }
                }
            }
        }

        private void statusListPopup_Closed(object sender, EventArgs e)
        {
            var cons = new List<string>();

            foreach (var item in statusList.SelectedItems)
                cons.Add(item.ToString());

            foreach (var item in initiativesList.Items.OfType<Initiatives>())
            {
                if (initiativesList.Items.OfType<Initiatives>().ToList().IndexOf(item) == row)
                    item.STATUS = cons;
            }

            statusList.SelectedItems.Clear();
        }

        private void statusCheckBox_Checked(object sender, EventArgs e)
        {
            var lbSelItems = statusList.SelectedItems;
            int cnt = lbSelItems.Count;

            if (cnt >= 10)
                lbSelItems.RemoveAt(9);
        }

        private void statusCheckBox_Unchecked(object sender, EventArgs e)
        {

        }

        private void saveInis_Click(object sender, EventArgs e)
        {
            var adv = currentPick;
            var fighters = new List<Initiatives>();

            foreach (var item in initiativesList.Items.OfType<Initiatives>())
            {
                fighters.Add(new Initiatives()
                {
                    ADV = adv,
                    ROLL = item.ROLL,
                    FOE = item.FOE,
                    NAME = item.NAME,
                    DMG = item.DMG,
                    STATUS = item.STATUS,
                    NOTE = item.NOTE,
                    CONLIST = item.CONLIST
                });
            }

            fighters.Sort((p, q) => p.ROLL.CompareTo(q.ROLL));
            fighters.Reverse();
            saveInitiatives(fighters);

            initiativesList.ItemsSource = fighters;
        }

        private void closeInis_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save first?", "Save before quitting?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                saveInis_Click(sender, e);

                battleSetupUIBorder.Visibility = Visibility.Visible;
                initiativesBorder.Visibility = Visibility.Hidden;
                campButton.IsEnabled = true;
            }

            if (result == MessageBoxResult.No)
            {
                battleSetupUIBorder.Visibility = Visibility.Visible;
                initiativesBorder.Visibility = Visibility.Hidden;
                campButton.IsEnabled = true;
            }

            if (result == MessageBoxResult.Cancel)
            { }

        }

        private void addFighter_Click(object sender, EventArgs e)
        {
            newFighterPopup.IsOpen = true;
            dmgHdr.Width = 100;

            var fighter = new List<Initiatives>();
            fighter.Add(NewFighter());

            newFighterLV.ItemsSource = fighter;            
        }

        private void quitNewFighter_Click(object sender, EventArgs e)
        {
            newFighterPopup.IsOpen = false;
        }

        private void addNewFighter_Click(object sender, EventArgs e)
        {
            var adv = currentPick;
            var fighters = new List<Initiatives>();

            foreach (var item in initiativesList.Items.OfType<Initiatives>())
            {
                fighters.Add(new Initiatives()
                {
                    ADV = adv,
                    ROLL = item.ROLL,
                    FOE = item.FOE,
                    NAME = item.NAME,
                    DMG = item.DMG,
                    STATUS = item.STATUS,
                    NOTE = item.NOTE,
                    CONLIST = item.CONLIST
                });
            }
                        
            foreach (var item in newFighterLV.Items.OfType<Initiatives>())
            {
                int dmg;
                if (item.FOE)
                    dmg = 0;
                else
                    dmg = item.DMG;                    

                fighters.Add(new Initiatives()
                {
                    ADV = adv,
                    ROLL = item.ROLL,
                    FOE = item.FOE,
                    NAME = item.NAME,
                    DMG = dmg,
                    STATUS = item.STATUS,
                    NOTE = item.NOTE,
                    CONLIST = item.CONLIST
                });

            }

            fighters.Sort((p, q) => p.ROLL.CompareTo(q.ROLL));
            fighters.Reverse();

            initiativesList.ItemsSource = fighters;
            newFighterPopup.IsOpen = false;
        }

        private void foeChk_Checked(object sender, EventArgs e)
        {
            dmgHdr.Width = 0;
        }

        private void foeChk_Unchecked(object sender, EventArgs e)
        {
            dmgHdr.Width = 100;
        }

        void partynumScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {                     
            var sb = sender as ScrollBar;            
            partyNum.Text = sb.Value.ToString();
        }

        void guestsnumScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            guestsNum.Text = sb.Value.ToString();
        }

        void baddiesnumScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            baddiesNum.Text = sb.Value.ToString();            
        }

        void ppScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            var sbContext = sb.DataContext;
            int sbIndex = newPartyList.Items.IndexOf(sbContext);

            foreach (var item in newPartyList.Items.OfType<Player>())
            {                
                if (newPartyList.Items.IndexOf(item) == sbIndex)                
                    item.PP = Convert.ToInt32(sb.Value);                    
            }            
        }

        void acScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            var sbContext = sb.DataContext;
            int sbIndex = newPartyList.Items.IndexOf(sbContext);

            foreach (var item in newPartyList.Items.OfType<Player>())
            {
                if (newPartyList.Items.IndexOf(item) == sbIndex)
                    item.AC = Convert.ToInt32(sb.Value);
            }
        }

        void playIniScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            var sbContext = sb.DataContext;
            int sbIndex = playersBattleList.Items.IndexOf(sbContext);

            foreach (var item in playersBattleList.Items.OfType<Player>())
            {
                if (playersBattleList.Items.IndexOf(item) == sbIndex)
                    item.INI = Convert.ToInt32(sb.Value);
            }
        }

        void guestIniScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            var sbContext = sb.DataContext;
            int sbIndex = guestsBattleList.Items.IndexOf(sbContext);

            foreach (var item in guestsBattleList.Items.OfType<Player>())
            {
                if (guestsBattleList.Items.IndexOf(item) == sbIndex)
                    item.INI = Convert.ToInt32(sb.Value);
            }
        }

        void baddieHPScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            var sbContext = sb.DataContext;
            int sbIndex = baddiesBattleList.Items.IndexOf(sbContext);

            foreach (var item in baddiesBattleList.Items.OfType<Baddies>())
            {
                if (baddiesBattleList.Items.IndexOf(item) == sbIndex)
                    item.HP = Convert.ToInt32(sb.Value);
            }
        }

        void baddieIniScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            var sbContext = sb.DataContext;
            int sbIndex = baddiesBattleList.Items.IndexOf(sbContext);

            foreach (var item in baddiesBattleList.Items.OfType<Baddies>())
            {
                if (baddiesBattleList.Items.IndexOf(item) == sbIndex)
                    item.INI = Convert.ToInt32(sb.Value);
            }
        }

        void iniboxScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            var sbContext = sb.DataContext;
            int sbIndex = initiativesList.Items.IndexOf(sbContext);

            foreach (var item in initiativesList.Items.OfType<Initiatives>())
            {
                if (initiativesList.Items.IndexOf(item) == sbIndex)
                    item.ROLL = Convert.ToInt32(sb.Value);
            }
        }

        void dmgboxScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            var sbContext = sb.DataContext;
            int sbIndex = initiativesList.Items.IndexOf(sbContext);

            foreach (var item in initiativesList.Items.OfType<Initiatives>())
            {
                if (initiativesList.Items.IndexOf(item) == sbIndex)
                    item.DMG = Convert.ToInt32(sb.Value);
            }
        }

        void newRollBoxScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            var sbContext = sb.DataContext;
            int sbIndex = newFighterLV.Items.IndexOf(sbContext);

            foreach (var item in newFighterLV.Items.OfType<Initiatives>())
            {
                if (newFighterLV.Items.IndexOf(item) == sbIndex)
                    item.ROLL = Convert.ToInt32(sb.Value);
            }
        }

        void newDmgBoxScrollbar_ValueChanged(object sender, ScrollEventArgs e)
        {
            var sb = sender as ScrollBar;
            var sbContext = sb.DataContext;
            int sbIndex = newFighterLV.Items.IndexOf(sbContext);

            foreach (var item in newFighterLV.Items.OfType<Initiatives>())
            {
                if (newFighterLV.Items.IndexOf(item) == sbIndex)
                    item.DMG = Convert.ToInt32(sb.Value);
            }
        }


        //display in ListView: INI  NAME  "Damage Done: " or "Health Remaining: "

        //add save and continue button
        //add go back button (prompt to save)
        //add load existing button and get rid of message box, disable button when none existing
        //add a fighter button - autosorts again but keeps all other deets
        //disable campaign button
        //disable edit party buttons

        /*
        private void fullButt_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var mainWindow = new MainWindow();

            if (this.WindowState == WindowState.Maximized)
            {
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.AllowsTransparency = false;
                mainWindow.WindowStyle = WindowStyle.ThreeDBorderWindow;
            }

            if (this.WindowState == WindowState.Normal)
            {
                mainWindow.WindowState = WindowState.Maximized;
                mainWindow.WindowStyle = WindowStyle.None;
                mainWindow.AllowsTransparency = true;
            }

            mainWindow.Show();
            this.Close();
        }
        */
    }
}
