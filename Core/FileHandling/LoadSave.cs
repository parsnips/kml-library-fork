namespace Core.FileHandling
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    public class LoadSave
    {
        private readonly List<FileFilter> filterList = new List<FileFilter>();

// ---
        public string Data;
        public string DefaultFilename;
        public string FilePath;
        public string InitialDirectory;
        private string fileType;
        private string filter;
        private bool m_AllowAllFiles;

// ---
        /// <summary>
        ///   A LoadSave object provides easy access to Load and Save dialogues and keeps track of the filepath of the file currently in use
        /// </summary>
        /// <param name = "file_type">The extension of file to be handled eg. "xml" or "txt"</param>
        public LoadSave()
            : this("*")
        {
        }

        public LoadSave(string filterName, params string[] exts)
        {
            Init(new FileFilter(filterName, exts));
        }

        public LoadSave(string ext)
        {
            Init(new FileFilter(ext));
        }

        public bool AllowAllFiles
        {
            get { return m_AllowAllFiles; }
            set
            {
                m_AllowAllFiles = value;
                RegenFilter();
            }
        }

        public bool HasSavePath
        {
            get { return FilePath != null; }
        }

        private void Init(FileFilter dFilter)
        {
            filterList.Add(dFilter);
            fileType = dFilter.Extension;
            RegenFilter();
            FilePath = null;
            InitialDirectory = null;
            DefaultFilename = null;
        }

        public void AddFilter(string ext)
        {
            filterList.Add(new FileFilter(ext));
            RegenFilter();
        }

        public void AddFilter(string name, params string[] exts)
        {
            filterList.Add(new FileFilter(name, exts));
            RegenFilter();
        }

        private void RegenFilter()
        {
            // Sql files (*.sql)|*.sql|All files (*.*)|*.*
            // Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF
            filter = string.Empty;
            foreach (var f in filterList)
            {
                if (filter != string.Empty)
                {
                    filter += "|";
                }

                filter += f.Name + " (" + f.ExtensionString + ")|" + f.ExtensionString + string.Empty;
            }

            if (m_AllowAllFiles)
            {
                filter += "|All files (*.*)|*.*";
            }
        }

        public bool FileExists()
        {
            return File.Exists(FilePath);
        }

        public void SaveByDialog(string title)
        {
            if (Data == null)
            {
                throw new Exception("No data to save");
            }

            var fname = (DefaultFilename == null) ? string.Empty : DefaultFilename;
            SaveByDialog(title, fname, Data);
        }

        public void SaveByDialog(string title, string data)
        {
            var fname = (DefaultFilename == null) ? string.Empty : DefaultFilename;
            SaveByDialog(title, fname, data);
        }

        public void SaveByDialog(string title, string suggestedName, string data)
        {
            var fpath = GetSavePath(title, suggestedName);

            if (fpath == null)
            {
                return;
            }

            FilePath = fpath;
            Data = data;

            Save();
        }

        public void Save()
        {
            if (Data == null)
            {
                throw new Exception("No data to save");
            }

            if (HasSavePath)
            {
                SaveData();
            }
            else
            {
                SaveByDialog("Please specify a file path");
            }
        }

        public void SaveAs(string filePath)
        {
            FilePath = filePath;
            Save();
        }

        public void SaveAs(string data, string filePath)
        {
            FilePath = filePath;
            Data = data;
            Save();
        }

        public void Save(string data)
        {
            Data = data;
            Save();
        }

        private void SaveData()
        {
            try
            {
                StreamWriter save_writer = null;
                if (FilePath != null)
                {
                    save_writer = new StreamWriter(FilePath);
                }

                if (save_writer != null)
                {
                    save_writer.Write(Data);
                    save_writer.Close();
                }
            }
            catch (Exception e)
            {
                var usr = MessageBox.Show("Could not save data to: " + FilePath + "\n" + e.Message, 
                                          "Problem saving file", MessageBoxButtons.RetryCancel);
                if (usr == DialogResult.Cancel)
                {
                    return;
                }

                SaveData();
            }
        }

        public bool LoadByDialog()
        {
            return LoadByDialog("Choose file...");
        }

        public bool LoadByDialog(string title)
        {
            var f_path = GetLoadPath(title);
            if (f_path != null)
            {
                Data = GetFileContentsFromPath(f_path);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Load(string path)
        {
            Data = GetFileContentsFromPath(path);
            if (Data != null)
            {
                FilePath = path;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///   Unsets the FilePath (will prompt a SaveAs on Save).
        /// </summary>
        /// <param name = "data">The new data set</param>
        public void NewFile(string data)
        {
            Data = data;
            FilePath = null;
        }

        /// <summary>
        ///   Unsets the FilePath (will prompt a SaveAs on Save). Sets Data to null.
        /// </summary>
        public void NewFile()
        {
            Data = null;
            FilePath = null;
        }

        public string GetFileContentsFromPath(string f_path)
        {
            if (f_path != null)
            {
                FilePath = f_path;
                var s_reader = new StreamReader(f_path);
                var f_data = s_reader.ReadToEnd();
                s_reader.Close();
                return f_data;
            }
            else
            {
                return null;
            }
        }

        public string GetSavePath()
        {
            return GetSavePath("Show me where to save the file");
        }

        public string GetSavePath(string title)
        {
            return GetSavePath(title, DefaultFilename);
        }

        public string GetSavePath(string title, string suggestedName)
        {
            var save_dialogue = new SaveFileDialog();
            if (InitialDirectory != null)
            {
                save_dialogue.InitialDirectory = InitialDirectory;
            }

            if (suggestedName != null)
            {
                save_dialogue.FileName = suggestedName;
            }

            save_dialogue.Filter = filter;
            save_dialogue.FilterIndex = 1;
            save_dialogue.RestoreDirectory = true;
            save_dialogue.Title = title;
            if (save_dialogue.ShowDialog() == DialogResult.OK)
            {
                FilePath = save_dialogue.FileName;
                return FilePath;
            }

            return null;
        }

        public string GetLoadPath()
        {
            return GetLoadPath("Please select a file");
        }

        public string GetLoadPath(string title)
        {
            var open_dialogue = new OpenFileDialog();
            if (InitialDirectory != null)
            {
                open_dialogue.InitialDirectory = InitialDirectory;
            }

            open_dialogue.Filter = filter;
            open_dialogue.FilterIndex = 1;
            open_dialogue.RestoreDirectory = true;
            open_dialogue.Title = title;
            if (open_dialogue.ShowDialog() == DialogResult.OK)
            {
                FilePath = open_dialogue.FileName;
                return FilePath;
            }

            return null;
        }

        public string[] GetLoadPathMultiple(string title)
        {
            var open_dialogue = new OpenFileDialog();
            if (InitialDirectory != null)
            {
                open_dialogue.InitialDirectory = InitialDirectory;
            }

            open_dialogue.Multiselect = true;
            open_dialogue.Filter = filter;
            open_dialogue.FilterIndex = 1;
            open_dialogue.RestoreDirectory = true;
            open_dialogue.Title = title;
            if (open_dialogue.ShowDialog() == DialogResult.OK)
            {
                return open_dialogue.FileNames;
            }

            return null;
        }

        public static bool BrowseLoadForTextBox(TextBox txtBox, string ext)
        {
            var ls = new LoadSave(ext);
            if (String.IsNullOrEmpty(txtBox.Text))
            {
                ls.InitialDirectory = txtBox.Text;
            }

            var path = ls.GetLoadPath();
            if (path != null)
            {
                txtBox.Text = path;
                return true;
            }

            return false;
        }

        public static bool BrowseSaveForTextBox(TextBox txtBox, string ext)
        {
            var ls = new LoadSave(ext);
            if (String.IsNullOrEmpty(txtBox.Text))
            {
                ls.InitialDirectory = txtBox.Text;
            }

            var path = ls.GetSavePath();
            if (path != null)
            {
                txtBox.Text = path;
                return true;
            }

            return false;
        }

        #region Nested type: FileFilter

        internal class FileFilter
        {
            internal string Extension;
            internal List<string> Extensions;
            internal string Name;

            internal FileFilter(string name, params string[] exts)
            {
                Extensions = new List<string>(exts);
                for (var i = 0; i < Extensions.Count; i++)
                {
                    var ext = Extensions[i];
                    if (ext.StartsWith("."))
                    {
                        Extensions[i] = ext.Substring(1);
                    }
                }

                Name = name;
                Extension = exts[0];
            }

            internal FileFilter(string name, string ext)
            {
                if (ext.StartsWith("."))
                {
                    ext = ext.Substring(1);
                }

                Name = name;
                Extension = ext;
            }

            internal FileFilter(string ext)
            {
                if (ext.StartsWith("."))
                {
                    ext = ext.Substring(1);
                }

                Extension = ext;
                Name = ext.ToUpper();
            }

            public string ExtensionString
            {
                get
                {
                    if (Extensions == null || Extensions.Count < 2)
                    {
                        return "*." + Extension;
                    }

// *.BMP;*.JPG;*.GIF
                    var ans = string.Empty;
                    foreach (var ext in Extensions)
                    {
                        ans += "*." + ext + ";";
                    }

                    return ans.Substring(0, ans.Length - 1);
                }
            }
        }

        #endregion
    }
}