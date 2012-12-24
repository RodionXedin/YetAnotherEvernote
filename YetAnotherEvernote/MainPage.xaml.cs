using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace YetAnotherEvernote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private String Notepad;
        private object LastSender;
        NotepadRepository notepadRepository = new NotepadRepository();
        public MainPage()
        {
            this.InitializeComponent();
            //notepadRepository.addNote(new Note("testnotepad", "testTitle", "testContent", DateTime.Today));
            notepadRepository.Save();
            foreach (string notepad in notepadRepository.GetNotepadesNames())
            {
                notepades.Items.Add(createNotepadeButton(notepad));
                
            }
            
        }

        void buttonPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Button item = (Button)sender;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.Green;
            item.Background = brush;
            if (!sender.Equals(LastSender))
                if (notes.Items.Count == 0)
                {
                    LastSender = sender;
                    if(notepadRepository.GetNotes(item.Content.ToString().ToLower()).Count >1)
                        notes.Items.Add(createNoteButton("+", ""));
                    else if ((notepadRepository.GetNotes(item.Content.ToString().ToLower()).Count >= 1) &&
                             (notepadRepository.GetNotes(item.Content.ToString().ToLower())[0].title != "+") &&
                             (notepadRepository.GetNotes(item.Content.ToString().ToLower())[0].content != ""))
                    {
                        notes.Items.Add(createNoteButton("+", ""));
                    }

                    foreach (Note note in notepadRepository.GetNotes(item.Content.ToString().ToLower()))
                    {
                        notes.Items.Add((createNoteButton(note.title, note.content)));
                    }
                   
                }
                else
                {
                    notes.Items.Add(createNoteButton("+", ""));
                    if (!sender.Equals(LastSender))
                    notes.Items.Clear();
                    
                }

        }

        void buttonClick(object sender, RoutedEventArgs e)
        {
            Button item = (Button)sender;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.LightGreen;
            item.Background = brush;

            brush.Color = Windows.UI.Colors.Black;
            item.Foreground = brush;
            Button item1 = new Button();
            item1 = (Button) LastSender;
            foreach (String notepad in notepadRepository.GetNotepadesNames())
            {
                if (notepad.Equals(item1.ToString().ToLower()))
                Notepad = notepad;
            }
            //NoteContent noteContent = new NoteContent(item1);
            Window.Current.Content = new NoteContent(item1,item);
            

        }

        void buttonPointerLeave(object sender, PointerRoutedEventArgs e)
        {
            Button item = (Button)sender;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.LightGreen;
            item.Background = brush;
            
        }

        private Button createNotepadeButton(String title)
        {
            Button button = new Button();
            button.Width = 340;
            button.Height = 100;
            button.Content = title.ToUpper();
            button.FontSize = 20;

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.Black;
            button.Foreground = brush;

            brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.LightGreen;
            button.Background = brush;

            button.PointerMoved += new PointerEventHandler(buttonPointerEntered);
            button.PointerExited += new PointerEventHandler(buttonPointerLeave);
            //button.Click += new RoutedEventHandler(buttonClick);
            //button.DragLeave += new DragEventHandler(buttonClick);

            return button;
        }

        void buttonPointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }
        private Button createNoteButton(String title, String content)
        {
            Button button = new Button();
            button.Width = 200;
            button.Height = 200;
            button.Content = title.ToUpper();
            button.FontSize = 16;

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.Black;
            button.Foreground = brush;

            brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.LightGray;
            button.Background = brush;
            button.PointerPressed += new PointerEventHandler(buttonClick);
            button.Click += new RoutedEventHandler(buttonClick);
            return button;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //List<Button> notepadesButtons = new List<Button>();
            /*List<Note> notesToDel = notepadRepository.GetNotes("testnotepad");
            //List<Note> notesToDel1 = notepadRepository.GetNotes("testnotepad2");
            for (int i = 0; i < 1; i++ )
            {
                notepadRepository.deleteNote(notesToDel[i]);
            }
            /*for (int i = 0; i < notesToDel1.Count; i++)
            {

                notepadRepository.deleteNote(notesToDel1[i]);
            }*/
            //notepadRepository.addNote(new Note("testnotepad", "testTitle", "testContent", DateTime.Today));
            //notepadRepository.addNote(new Note("testnotepad1", "testTitle1", "testContent1", DateTime.Today));
            //notepadRepository.addNote(new Note("testnotepad1", "testTitle2", "testContent2", DateTime.Today));
            //notepadRepository.addNote(new Note("testnotepad2", "testTitle3", "testContent3", DateTime.Today));
            notepadRepository.Save();
            foreach (string notepad in notepadRepository.GetNotepadesNames())
            {
                notepades.Items.Add(createNotepadeButton(notepad));
                
            }
            
            
            /*
            notepades.Items.Add(createNotepadeButton("button1"));
            notepades.Items.Add(createNotepadeButton("button2"));
            notepades.Items.Add(createNotepadeButton("button3"));
            notepades.Items.Add(createNotepadeButton("button4"));
            notepades.Items.Add(createNotepadeButton("button5"));
            notepades.Items.Add(createNotepadeButton("button3"));
            notepades.Items.Add(createNotepadeButton("button4"));
            notepades.Items.Add(createNotepadeButton("button5"));
            notepades.Items.Add(createNotepadeButton("button3"));
            notepades.Items.Add(createNotepadeButton("button4"));
            notepades.Items.Add(createNotepadeButton("button5"));

            notes.Items.Add(createNoteButton("button3", "content"));
            notes.Items.Add(createNoteButton("button3", "content"));
            notes.Items.Add(createNoteButton("button3", "content"));
             * */
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if ((NotePadAdd.Text != null) && (NotePadAdd.Text != ""))
            {
                notepadRepository.addNote(new Note(NotePadAdd.Text, "+", "", DateTime.Today));
                notepades.Items.Clear();
                foreach (string notepad in notepadRepository.GetNotepadesNames())
                {
                    notepades.Items.Add(createNotepadeButton(notepad));
                }
                notepadRepository.Save();
                
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
           // notepadRepository.deleteNote();
        }
    }
}
