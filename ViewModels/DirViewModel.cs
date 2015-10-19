using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DirectoryManager.Helpers;
using DirectoryManager.Models;

namespace DirectoryManager.ViewModels
{
    public class DirViewModel
    {
        #region Fields

        DirModel myDir;
        ObservableCollection<DirModel> dirs = new ObservableCollection<DirModel>();
        ObservableCollection<DirModel> processDirs = new ObservableCollection<DirModel>();

        #endregion / Fields

        #region Constructor and Generate random directories and nested sub-directories

        public DirViewModel()
        {
            GenerateRandomNestedDirs();

            #region Set parent

            foreach (DirModel dir in this.dirs)
            {
                dir.SetParent();
            }

            #endregion / Set parent
        }

        private void GenerateRandomNestedDirs()
        {
            #region 1st level
            for (int i = 0; i < 6; i++)
            {
                this.myDir = DirModel.GetDir("Dir", 1000);
                this.dirs.Add(this.myDir);
            }
            #endregion

            #region 2nd level
            foreach (DirModel dir in this.dirs)
            {
                for (int i = 0; i < 2; i++)
                {
                    this.myDir = DirModel.GetDir("Sub-Dir", 1000);
                    dir.SubDirs.Add(this.myDir);
                }
            }
            #endregion

            #region 3rd level
            foreach (DirModel dir in this.dirs)
            {
                foreach (DirModel subDir in dir.SubDirs)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        this.myDir = DirModel.GetDir("Sub-sub-Dir", 1000);
                        subDir.SubDirs.Add(this.myDir);
                    }
                }
            }
            #endregion

            #region 4th level
            foreach (DirModel dir in this.dirs)
            {
                foreach (DirModel subDir in dir.SubDirs)
                {
                    foreach (DirModel subSubDir in subDir.SubDirs)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            this.myDir = DirModel.GetDir("Sub-sub-sub-Dir", 1000);
                            subSubDir.SubDirs.Add(this.myDir);
                        }
                    }
                }
            }
            #endregion
        }

        #endregion / Constructor and Generate random

        #region Properties 

        public ObservableCollection<DirModel> Dirs
        {
            get { return this.dirs; }
            set { this.dirs = value; }
        }

        public ObservableCollection<DirModel> ProcessDirs
        {
            get { return this.processDirs; }
            set { this.processDirs = value; }
        }
        #endregion / Properties

        #region Commands

        #region TestCmd *outside of ItemTemplate*

        ICommand testCmd;
        public ICommand TestCmd
        {
            get
            {
                return this.testCmd ??
                    (this.testCmd = new RelayCommand(this.TestExe));
            }
        }
        void TestExe(object obj)
        {
            MessageBox.Show(obj.ToString());
        }

        #endregion / TestCmd *outside of ItemTemplate*

        #region MoveDirCmd *inside of ItemTemplate*

        ICommand moveDirCmd;
        public ICommand MoveDirCmd
        {
            get
            {
                return this.moveDirCmd ??
                    (this.moveDirCmd = new RelayCommand(this.MoveDirExe));
            }
        }
        private void MoveDirExe(object obj)
        {
            Button clickedBtn = obj as Button;
            DirModel dir = clickedBtn.Tag as DirModel;

            MoveThisDir(dir);
        }
        private void MoveThisDir(DirModel d)
        {
            if (d.IsSelected == true)
            {
                if (d.Parent == null)
                {
                    this.dirs.Remove(d);
                    this.processDirs.Add(d);
                }
                else
                {
                    DirModel parent = d.Parent;
                    parent.SubDirs.Remove(d);
                    this.processDirs.Add(d);
                }
            }
            else if (d.IsSelected == null)
            {
                MessageBox.Show(d.DirDescription + " has sub-Dir not selected");
            }
            else
            {
                MessageBox.Show("Please explicitly select " + d.DirDescription);
            }
        }
        
        #endregion / MoveDirCmd *inside of ItemTemplate*

        #region ClearCmd

        ICommand clearCmd;
        public ICommand ClearCmd
        {
            get
            {
                return this.clearCmd ??
                    (this.clearCmd = new RelayCommand(this.ClearExe));
            }
        }
        void ClearExe(object obj)
        {
            this.processDirs.Clear();
        }

        #endregion / ClearCmd

        #endregion / Commands
    }
}
