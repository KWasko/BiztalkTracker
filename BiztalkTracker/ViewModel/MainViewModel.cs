using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiztalkDbHelper;
using BiztalkDbHelper.Model;
using BiztalkTracker.Model;
using AutoMapper;
using System.Windows.Input;
using BiztalkDbHelper.Utils;
using PropertyChanged;
using System.IO;
using System.Windows;

namespace BiztalkTracker.ViewModel
{
    [ImplementPropertyChanged]
    public class MainViewModel
    {
      
        public List<string> ConnectionStringsList { get; set; }
        public string SelectedConnectionString { get; set;}

        public MsgSearchQuery SearchQuery { get; set; }
        public ObservableCollection<SelectableMessage> Messages { get; set; }

        public SelectableMessage SelectedMessage { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand ClearSearchDataCommand { get; set; }
        public ICommand SaveMessagesCommand { get; set; }
        public MainViewModel()
        {
            SearchQuery = new MsgSearchQuery()
            {
                MsgBodyAndContextDependedSearchQuery= new BodyAndContextDependedSearchQuery()
            };
            Messages = new ObservableCollection<SelectableMessage>();
            ConnectionStringsList = new List<string> ();
            ConnectionStringsList.AddRange(Properties.Settings.Default.ConnectionStrings.Cast<string>().ToList());
            SelectedConnectionString = ConnectionStringsList.FirstOrDefault();

            Mapper.Initialize(cfg => cfg.CreateMap<Message, SelectableMessage>());

            SearchCommand = new DelegateCommand(GetMessages);
            ClearSearchDataCommand = new DelegateCommand(ClearSearchData);
            SaveMessagesCommand = new DelegateCommand(SaveMessages);

        }
        public void GetMessages()
        {
            using (var con = new SqlConnection(SelectedConnectionString))
            {
                ITrackedMsgsFinder trMsgFinder = new TrackedMsgsFinder();
                //ITrackedMsgsFinder trMsgFinder = new FakeTrackedMsgsFinder();
                List<Message> messages = null;
                try
                {
                     messages = trMsgFinder.GetTrackedMessages(SearchQuery, con);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fetching messages", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
           
                Messages.Clear();
                foreach(var msg in messages)
                {
                    var selectableMessage = Mapper.Instance.Map<SelectableMessage>(msg);
                    Messages.Add(selectableMessage);
                }
            }
        }

        public void SaveMessages()
        {
            var messages = Messages.Where(m => m.IsSelected).ToList();
            if (messages.Count == 0) return;
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog() { Description ="Choose directory where messages will be saved"})
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        foreach (var msg in messages)
                        {
                            string msgFilePath = Path.Combine(dialog.SelectedPath, msg.Id + ".txt");
                            File.WriteAllText(msgFilePath, msg.Body, Encoding.UTF8);
                        }
                        MessageBox.Show("Operation completed.", "Saving messages", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Saving messages", MessageBoxButton.OK, MessageBoxImage.Error);
                    }            
                }
            }
        }

        public void ClearSearchData()
        {
          var dlgResult=  MessageBox.Show("Are you sure do you want to clear search definitions?", "Clearing search data", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (dlgResult == MessageBoxResult.Yes)
                SearchQuery = new MsgSearchQuery();
        }
    }
}
