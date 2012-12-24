using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace YetAnotherEvernote
{
     internal class NotepadRepository
    {
        private String fileName;
        private string uri;
        private string SJson;
        private int lastId;
        private JObject o;
        private Dictionary<String, List<Note>> foundNotes;
        Windows.Storage.ApplicationDataContainer ourTxtFile = Windows.Storage.ApplicationData.Current.LocalSettings; 

        /*private void CreateEmptyJson()
        {
         
        }*/

        public void addNote(Note note)
        {
            if (!foundNotes.ContainsKey(note.notepad))
                foundNotes.Add(note.notepad, new List<Note>());
            note.id = lastId++;
            foundNotes[note.notepad].Add(note);
        }

        public void deleteNote(Note note)
        {
            if (foundNotes.ContainsKey(note.notepad))
            {
                foundNotes[note.notepad].Remove(note);
            }
        }

        public NotepadRepository()
        {
            //this.uri = uri;
            //this.fileName = fileName;
            foundNotes = new Dictionary<String, List<Note>>();

            if (!GetJson())
            {
                //CreateEmptyJson();
                lastId = 0;
                
            }
        }

        private bool GetJson()
        {
            StreamReader sr = null;
            try
            {


                String line = ourTxtFile.Values["Json"].ToString();
                Dictionary<String, Note> list = JsonConvert.DeserializeObject<Dictionary<String, Note>>(line);
                foreach (String key in list.Keys)
                {
                    Note note = list[key];
                    addNote(note);
                }
                // pring json file content
                //Console.WriteLine(line);
                

                return true;
            }

            catch (Exception e)
            {
               
                return false;
            }
            finally
            {
                //if(sr != null)
                    //sr.Close();
            }
        }

        public void Save()
        {
            // write lastID to json file
            JObject container = new JObject();

           // StreamWriter jsonWriter =
           //     new StreamWriter(fileName); 

            foreach(String key in foundNotes.Keys)
            {
                List<Note> notes = foundNotes[key];
                foreach (Note note in notes)
                {
                    // write to json file   
                    JObject jobj = new JObject();
                    jobj.Add("notepad", key);
                    jobj.Add("title", note.title);
                    jobj.Add("content", note.content);
                    jobj.Add("id", note.id);
                    jobj.Add("date", note.date);
                    container.Add(note.id.ToString(), jobj);
                }
            }
            ourTxtFile.Values["Json"] = container.ToString();
            /*jsonWriter.Write(container.ToString());
            jsonWriter.Flush();
            jsonWriter.Dispose();*/
        }
        
        public ICollection<String> GetNotepadesNames()
        {
            return foundNotes.Keys;
        }

        public List<Note> GetNotes (string  npName)
        {
            if (foundNotes[npName]!=null)
            return foundNotes[npName];
            return new List<Note>();
        }
    }
}
class Note
{
    public int id { get; set; }
    public String notepad { get; set; }
    public String title { get; set; }
    public String content { get; set; }
    public DateTime date { get; set; }
    public Note(string notepad, String title, String Content, DateTime date)
    {
        this.notepad = notepad.ToLower();
        this.title = title.ToLower();
        this.content = Content;
        this.date = date;
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (this == obj)
            return true;
        if (!(obj is Note))
            return false;
        Note note = (Note)obj;
        return id == note.id;
    }
    public override int GetHashCode()
    {
        return id;
    }

}
