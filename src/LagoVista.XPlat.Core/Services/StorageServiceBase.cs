using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using LagoVista.Core.PlatformSupport;

namespace LagoVista.XPlat.Core.Services
{
    public abstract class StorageServiceBase : IStorageService
    {
        private const string SETTINGS_FILE_NAME = "AppConfigSettings.json";


        protected abstract Task<Stream> GetStreamAsync(String file);

        protected abstract Task PutStreamAsync(String fileName, Stream stream);
        protected abstract Task<String> ReadAllTextAsync(String file);

        protected abstract Task PutAllTextAsync(String fileName, String stream);

        private async Task<Dictionary<string, object>> GetSettingsAsync()
        {
            var settingsJSON = await ReadAllTextAsync(SETTINGS_FILE_NAME);
            if (String.IsNullOrEmpty(settingsJSON))
            {
                return new Dictionary<string, object>();
            }
            else
            {
                return JsonConvert.DeserializeObject<Dictionary<String, object>>(settingsJSON);
            }
        }

        private async Task SaveSettingsAsync(Dictionary<String, object> appSettings)
        {
            var settingsJSON = Newtonsoft.Json.JsonConvert.SerializeObject(appSettings);
            await PutAllTextAsync(SETTINGS_FILE_NAME, settingsJSON);
        }

        public async Task<T> GetKVPAsync<T>(string key, T defaultValue = null) where T : class
        {
            var appSettings = await GetSettingsAsync();
            if (appSettings.ContainsKey(key))
            {
                return (appSettings[key] as T);
            }
            else
            {
                return (defaultValue);
            }
        }

        public Task<Uri> StoreAsync(Stream stream, Locations location, string fileName, string folder = "")
        {
            throw new NotImplementedException();
        }

        public Task<Stream> Get(Uri rui)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> Get(Locations location, string fileName, string folder = "")
        {
            throw new NotImplementedException();
        }

        public async Task StoreKVP<T>(string key, T value) where T : class
        {
            var appSettings = await GetSettingsAsync();
            if(appSettings.Keys.Contains(key))
            {
                appSettings.Remove(key);
            }

            appSettings.Add(key, value);
            await SaveSettingsAsync(appSettings);
        }

        public async Task<bool> HasKVPAsync(string key)
        {
            var appSettings = await GetSettingsAsync();
            return (appSettings.Keys.Contains(key));
          }

        public async Task ClearKVP(string key)
        {
            var appSettings = await GetSettingsAsync();
            if (appSettings.Keys.Contains(key))
            {
                appSettings.Remove(key);
                await SaveSettingsAsync(appSettings);
            }
        }

        public async Task<string> StoreAsync<TObject>(TObject instance, string fileName) where TObject : class
        {
            var json = JsonConvert.SerializeObject(instance);
            await PutAllTextAsync(fileName, json);

            return fileName;
        }

        public async Task<TObject> GetAsync<TObject>(string fileName) where TObject : class
        {
            var json = await ReadAllTextAsync(fileName);
            if(String.IsNullOrEmpty(json))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<TObject>(json);
            }
        }

        public Task<string> StoreAsync(Stream stream, string fileName, Locations location = Locations.Default, string folder = "")
        {
            return StoreAsync(stream, fileName);
        }

        public Task<Stream> Get(string fileName, Locations location = Locations.Default, string folder = "")
        {
            return null;
        }
        
        

        public Task<string> WriteAllTextAsync(string fileName, string text)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ReadAllLinesAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> WriteAllLinesAsync(string fileName, List<string> text)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ReadAllBytesAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> WriteAllBytesAsync(string fileName, byte[] buffer)
        {
            throw new NotImplementedException();
        }

        Task<string> IStorageService.ReadAllTextAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task ClearAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
