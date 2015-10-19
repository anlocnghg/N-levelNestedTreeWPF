using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryManager.Models 
{
    public class DirModel : INotifyPropertyChanged
    {
        #region Fields

        DirModel parent;
        int dirId;
        string dirName;
        bool? isSelected = false;
        ObservableCollection<DirModel> subDirs;

        static Random random = new Random();

        #endregion / Fields

        #region Set parent for sub-dirs

        public void SetParent() {
            foreach (DirModel dir in this.subDirs) {
                dir.parent = this;
                dir.SetParent();
            }
        }

        #endregion /Set parent

        #region Properties
        public string DirDescription
        {
            get { return this.dirName + " #" + this.dirId; }
        }

        public DirModel Parent
        {
            get { return this.parent; }
        }
        public ObservableCollection<DirModel> SubDirs
        {
            get { return this.subDirs; }
            set
            {
                this.subDirs = value;
                OnPropertyChanged("SubDirs");
            }
        }

        public bool? IsSelected
        {
            get { return this.isSelected; }
            set
            {
                SetIsSelected(value, true, true);
            }
        }

        private void SetIsSelected(bool? value, bool isUpdateChildren, bool isUpdateParent)
        {
            if (value == this.isSelected) return;
            this.isSelected = value;

            if (isUpdateChildren && this.isSelected.HasValue)
            {
                foreach (DirModel dir in this.subDirs)
                {
                    dir.SetIsSelected(this.isSelected, true, false);
                }
            }

            if (isUpdateParent && this.parent != null)
            {
                this.parent.VerifyCheckedState();
            }

            OnPropertyChanged("IsSelected");
        }

        private void VerifyCheckedState()
        {
            bool? state = null;

            for (int i = 0; i < this.subDirs.Count; i++)
            {
                bool? current = this.subDirs.ElementAt(i).isSelected;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }

            SetIsSelected(state, false, true);
        }


        #endregion / Properties

        #region Constructor and Get Instance
        private DirModel(string name, int id)
        {
            this.dirId = id;
            this.dirName = name;
            this.subDirs = new ObservableCollection<DirModel>();
        }
        public static DirModel GetDir(string name, int upLimitDirRandomId)
        {
            return new DirModel(name, random.Next(1, upLimitDirRandomId));
        }

        #endregion / Constructor and Get Instance

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion / INotifyPropertyChanged
    }
}
