using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace YetAnotherEvernote
{
    public sealed partial class NoteContent : Page
    {
        private String notepad;
        private Button item;
        private Button item1;
        private NotepadRepository notepadRepository;
        private List<Note> ourNotes;
        private Note CurrNote;
        public NoteContent(object sender , object sender1)
        {
            this.InitializeComponent();
            item = (Button)sender;
            item1 = (Button) sender1;
            notepadRepository = new NotepadRepository();
            ourNotes = notepadRepository.GetNotes(item.Content.ToString().ToLower());
            notepad = ourNotes[0].notepad;
            
            foreach (Note note in ourNotes)
            {
                if (note.title == item1.Content.ToString().ToLower())
                    CurrNote = note;
            }
            if (CurrNote != null)
            {
                if (CurrNote.title == "+")
                    CurrNote.title = "New Title";
                NoteAct.Text = CurrNote.content;
                //NoteAct.Document.SetText(new Windows.UI.Text.TextSetOptions(), CurrNote.content);
                NoteName.Text = CurrNote.title;
            }
            AppTitle.Text = notepad;
                foreach (Note note in ourNotes)
            {
                notes.Items.Add(createNoteButton(note.title,note.content));   
            }
            



        }
        void buttonPointerLeave(object sender, PointerRoutedEventArgs e)
        {
            Button item = (Button)sender;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.LightGray;
            item.Background = brush;

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
            //item1 = (Button)LastSender;
            bool NoteChoosen = false;
            foreach (Note note in ourNotes)
            {
                if ((note.title.Equals(item.Content.ToString().ToLower())) && NoteChoosen == false)
                {
                    if (CurrNote != null)
                    {
                        if (CurrNote.id != note.id)
                        {
                            NoteChoosen = true;
                            CurrNote = note;
                            NoteName.Text = note.title;
                            NoteAct.Text = note.content;
                            item.Content = CurrNote.title.ToUpper();
                            //NoteAct.Document.SetText(new Windows.UI.Text.TextSetOptions(), note.content);
                        }
                    }
                    else
                    {
                        NoteChoosen = true;
                        CurrNote = note;
                        NoteName.Text = note.title;
                        NoteAct.Text = note.content;
                        item.Content = CurrNote.title.ToUpper();
                    }
                }
            }
            
            //NoteContent noteContent = new NoteContent(item1);
            //Window.Current.Content = new NoteContent(item1);

        }
        private Button createNoteButton(String title, String content)
        {
            Button button = new Button();
            button.Width = 390;
            button.Height = 100;
            button.Content = title.ToUpper();
            button.FontSize = 16;
            
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.Black;
            button.Foreground = brush;

            brush = new SolidColorBrush();
            brush.Color = Windows.UI.Colors.LightGray;
            button.Background = brush;
            button.PointerExited += new PointerEventHandler(buttonPointerLeave);
            button.Click += new RoutedEventHandler(buttonClick);
            return button;
        }

        private void NoteAct_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            if (CurrNote != null)
            {
                CurrNote.content = NoteAct.Text;
                //String str;
                //NoteAct.Document.GetText(new Windows.UI.Text.TextGetOptions(), out str);
                //CurrNote.content = str;
                foreach (Note note in ourNotes)
                {
                    if (CurrNote.id == note.id)
                    {
                        //note.title = CurrNote.title;
                        note.content = CurrNote.content;
                    }
                    
                }
            }
            SaveAnnouncer.Visibility = Visibility.Visible;
            SaveAnnouncer.Text = "Changes Saved";
            notepadRepository.Save();
        }

        private void NoteAddButton_Click(object sender, RoutedEventArgs e)
        {
            if ((NoteName.Text != null) && (NoteName.Text != "")) 
            {
                if (NoteAct.Text != "" && NoteAct.Text != null)
                    notepadRepository.addNote(new Note(notepad, NoteName.Text.ToString(), NoteAct.Text, DateTime.Today));
                else
                notepadRepository.addNote(new Note(notepad,NoteName.Text.ToString(),"",DateTime.Today));
                notepadRepository.Save();
                notes.Items.Clear();
                ourNotes = notepadRepository.GetNotes(item.Content.ToString().ToLower());
                foreach (Note note in ourNotes)
                {
                    notes.Items.Add(createNoteButton(note.title, note.content));
                }
            }
            NoteName.Text = "";
        }

        private void NoteName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CurrNote != null)
            {
                CurrNote.title = NoteName.Text;
                foreach (Note note in ourNotes)
                {
                    if (CurrNote.id == note.id)
                    {
                        note.title = CurrNote.title;
                    }

                }
                if (NoteName.Text == "")
                    NoteName.Text = "New Note";
                notepadRepository.Save();
                notes.Items.Clear();
                foreach (Note note in ourNotes)
                {
                    notes.Items.Add(createNoteButton(note.title, note.content));
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrNote != null)
            {
                notepadRepository.deleteNote(CurrNote);
                notepadRepository.Save();
                notes.Items.Clear();
                ourNotes = notepadRepository.GetNotes(item.Content.ToString().ToLower());
                foreach (Note note in ourNotes)
                {
                    notes.Items.Add(createNoteButton(note.title, note.content));
                }
            }

        }


        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = new MainPage();
            
        }

        
    }
}
