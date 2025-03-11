using LagoVista.Core.Interfaces;
using LagoVista.Core.IOC;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace LagoVista.Core.WPF.PlatformSupport
{
    public class PopupsService : IPopupServices
    {
        public Task<bool> ConfirmAsync(string title, string prompt)
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();

            Task.Run(() =>
            {
                var result = MessageBox.Show(prompt, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                tcs.SetResult(result);
            });

            return tcs.Task;
        }

        public Task<double?> PromptForDoubleAsync(string label, double? defaultvalue = 0, string help = "", bool isRequried = false)
        {
            var promptWindow = new UI.PromptDialog<decimal>();
            promptWindow.Title = label;
            promptWindow.Help = help;
            promptWindow.isRequired = isRequried;
            promptWindow.DoubleValue = defaultvalue;
            var result = promptWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return Task.FromResult(promptWindow.DoubleValue);
            }
            else
            {
                return Task.FromResult(defaultvalue);
            }
        }

        public Task<int?> PromptForIntAsync(string label, int? defaultvalue = 0, string help = "", bool isRequried = false)
        {
            var promptWindow = new UI.PromptDialog<int>();
            promptWindow.Title = label;
            promptWindow.Help = help;
            promptWindow.isRequired = isRequried;
            promptWindow.IntValue = defaultvalue;

            var result = promptWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return Task.FromResult(promptWindow.IntValue);
            }
            else
            {
                return Task.FromResult(defaultvalue);
            }
        }

        public Task<string> PromptForStringAsync(string label, string defaultvalue = "", string help = "", bool isRequried = false)
        {
            var tcs = new TaskCompletionSource<string>();
            var promptWindow = new UI.PromptDialog<string>();
            promptWindow.Title = label;
            promptWindow.Help = help;
            promptWindow.isRequired = isRequried;
            promptWindow.StringValue = defaultvalue;
            var result = promptWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return Task.FromResult(promptWindow.StringValue);
            }
            else
            {
                return Task.FromResult(defaultvalue);
            }
        }

        public Task ShowAsync(string message)
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<object>();

            Task.Run(() =>
            {
                MessageBox.Show(message);
                tcs.SetResult(null);
            });

            return tcs.Task;
        }

        public Task ShowAsync(string title, string message)
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<object>();

            var promptWindow = new UI.PromptDialog<string>();

            promptWindow.Title = title;
            promptWindow.Help = message;
            promptWindow.TextInputVisible = false;
            promptWindow.CancelButtonVisible = false;
           
            promptWindow.ShowDialog();
            tcs.SetResult(null);

            return tcs.Task;
        }

        private string _lastOpenDirectory;
        private string _lastSaveDirectory;

        public async Task<string> ShowOpenFileAsync(string fileMask = "")
        {
            if (String.IsNullOrEmpty(_lastOpenDirectory))
            {
                Object obj;
                if (SLWIOC.TryResolve(typeof(IStorageService), out obj))
                {
                    var storage = obj as IStorageService;
                    _lastOpenDirectory = await storage.GetKVPAsync<string>("LAST_BROWSED_OPEN_DIRECTORY");
                }
            }

            var openFileDialog = new OpenFileDialog();
            if (!String.IsNullOrEmpty(fileMask))
            {
                openFileDialog.Filter = fileMask;
            }
            else
            {
                openFileDialog.Filter = "All Files|*.*";
            }

            if (!String.IsNullOrEmpty(_lastOpenDirectory))
            {
                openFileDialog.InitialDirectory = _lastOpenDirectory;
            }
            else
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            }

            var result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var fn = openFileDialog.FileName;
                var fileInfo = new FileInfo(openFileDialog.FileName);
                _lastOpenDirectory = fileInfo.DirectoryName;
                Object obj;
                if (SLWIOC.TryResolve(typeof(IStorageService), out obj))
                {
                    var storage = obj as IStorageService;
                    await storage.StoreKVP("LAST_BROWSED_OPEN_DIRECTORY", _lastOpenDirectory);
                }

                return fn;
            }
            else
            {
                return null;
            }

        }

        public async Task<string> ShowSaveFileAsync(string defaultFileName = "", string fileMask = "")
        {
            if (String.IsNullOrEmpty(_lastSaveDirectory))
            {
                Object obj;
                if (SLWIOC.TryResolve(typeof(IStorageService), out obj))
                {
                    var storage = obj as IStorageService;
                    _lastSaveDirectory = await storage.GetKVPAsync<string>("LAST_BROWSED_SAVE_DIRECTORY");
                }
            }

            var saveFileDialog = new SaveFileDialog();
            if (!String.IsNullOrEmpty(fileMask))
            {
                saveFileDialog.Filter = fileMask;
            }
            else
            {
                saveFileDialog.Filter = "All Files|*.*";
            }

            if (!String.IsNullOrEmpty(_lastSaveDirectory))
            {
                saveFileDialog.InitialDirectory = _lastSaveDirectory;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            }

            var result = saveFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var fn = saveFileDialog.FileName;
                var fileInfo = new FileInfo(saveFileDialog.FileName);
                _lastSaveDirectory = fileInfo.DirectoryName;
                Object obj;
                if (SLWIOC.TryResolve(typeof(IStorageService), out obj))
                {
                    var storage = obj as IStorageService;
                    await storage.StoreKVP("LAST_BROWSED_SAVE_DIRECTORY", _lastSaveDirectory);
                }

                return fn;
            }
            else
            {
                return null;
            }
        }

        public Task<bool> ShowModalAsync<TModalWindow, TViewModel>(TViewModel viewModal)
            where TModalWindow : IModalWindow
            where TViewModel : IViewModel
        {
            throw new NotImplementedException();
        }
    }
}
