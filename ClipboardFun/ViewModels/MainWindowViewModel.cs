using CsvHelper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using Savaged.ClipboardFun.Extensions;
using Savaged.ClipboardFun.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;

namespace Savaged.ClipboardFun.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string raw;
        private string csv;
        private string json;

        public MainWindowViewModel()
        {
            Index = new ObservableCollection<Model>();
            ClearCmd = new RelayCommand(OnClear);
            PasteCmd = new RelayCommand(OnPaste);
        }

        public ObservableCollection<Model> Index { get; }

        public string Raw
        {
            get => raw; 
            set => Set(ref raw, value);
        }

        public string CSV
        {
            get => csv;
            set => Set(ref csv, value);
        }

        public string JSON
        {
            get => json;
            set => Set(ref json, value);
        }

        public RelayCommand ClearCmd { get; }

        public RelayCommand PasteCmd { get; }

        private void OnClear()
        {
            Clipboard.Clear();
            Raw = JSON = string.Empty;
            Index.Clear();
        }

        private void OnPaste()
        {
            Raw = Clipboard.GetText();
            CSV = Clipboard.GetText(TextDataFormat.CommaSeparatedValue);
            JSON = GetJsonFromCsv(CSV);
            UpdateIndex(CSV);
        }

        private void UpdateIndex(string csv)
        {
            var dt = GetDataTableFromCsv(csv);
            foreach (DataRow dr in dt.Rows)
            {
                Index.Add(dr.ToModel<Model>());
            }
        }

        private static DataTable GetDataTableFromCsv(string csv)
        {
            var value = new DataTable();
            if (string.IsNullOrEmpty(csv))
            {
                return value;
            }
            using (var textReader = new StringReader(csv))
            {
                using (var csvReader = new CsvReader(textReader))
                {
                    using (var csvDataReader = new CsvDataReader(csvReader))
                    {
                        value.Load(csvDataReader);
                    }
                }
            }
            return value;
        }

        private static string GetJsonFromCsv(string csv)
        {
            var value = string.Empty;
            if (string.IsNullOrEmpty(csv))
            {
                return value;
            }
            var dt = GetDataTableFromCsv(csv);
            value = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return value;
        }
    }
}
