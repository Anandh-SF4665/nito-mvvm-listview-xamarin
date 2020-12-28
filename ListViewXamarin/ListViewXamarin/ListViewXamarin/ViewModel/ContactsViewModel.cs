using Newtonsoft.Json;
using Nito.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListViewXamarin
{
    public class ContactsViewModel : INotifyPropertyChanged
    {
        #region Fields
        
        private bool isLoading = false;
        private dynamic userInfo;
        #endregion
        
        #region Properties

        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        
        public IAsyncCommand LoadDataCommand { get; set; }

        public dynamic UserInfo 
        {
            get { return userInfo; }
            set { userInfo = value; RaisedOnPropertyChanged(nameof(UserInfo)); }

        public bool IsLoading 
        {
            get { return isLoading; }
            set { isLoading = value; RaisedOnPropertyChanged(nameof(IsLoading)); }
        }
        #endregion

        #region Constructor

        public ContactsViewModel()
        {
            client = new HttpClient();
            LoadDataCommand = new AsyncCommand(ExecuteLoadData);
        }
        #endregion

        #region Methods
        public async Task ExecuteLoadData()
        {
            var notifyTask = NotifyTask.Create(GetDataAsync);
            IsLoading = true;
            await notifyTask.TaskCompleted;
            if (notifyTask.IsCompleted)
            {
                UserInfo = notifyTask.Result as dynamic;
            }
            IsLoading = false;
        }

        public async Task<dynamic> GetDataAsync()
        {
            try
            {
                //Sends request to retrieve data from the web service for the specified Uri
                var response = await client.GetAsync("https://jsonplaceholder.typicode.com/users");
                if (response.IsSuccessStatusCode)
                {
                    await Task.Delay(1500);
                    var content = await response.Content.ReadAsStringAsync(); //Returns the response as JSON string
                    var details = JsonConvert.DeserializeObject<dynamic>(content); //Converts JSON string to dynamic
                    return details;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
            return null;
        }

        #endregion

        #region Interface Member

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
