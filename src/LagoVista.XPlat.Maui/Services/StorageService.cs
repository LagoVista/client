using LagoVista.Core.PlatformSupport;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LagoVista.XPlat.Maui.Services
{
    internal class StorageService : IStorageService
    {
        Dictionary<String, Object> _kvpStorage;

        public Task ClearAllAsync()
        {
            throw new NotImplementedException();
        }

        private async Task PersistDictionary()
        {
            await StoreAsync(_kvpStorage, "KVPSTORAGE.DAT");
        }

        private async Task<Dictionary<string, object>> GetDictionary()
        {
            if (_kvpStorage != null)
                return _kvpStorage;

            _kvpStorage = await GetAsync<Dictionary<string, object>>("KVPSTORAGE.DAT");
            if (_kvpStorage == null)
                _kvpStorage = new Dictionary<string, object>();

            return _kvpStorage;
        }

        public async Task ClearKVP(string key)
        {
            (await GetDictionary()).Clear();
            await PersistDictionary();
        }

        public Task<Stream> Get(Uri uri)
        {
            var client = new HttpClient();
            return client.GetStreamAsync(uri);
        }

        private String GetAppDataDirectory(Locations location = Locations.Default)
        {
            var name = Process.GetCurrentProcess().ProcessName;

            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (location == Locations.Temp)
                dir = System.IO.Path.GetTempPath();
            else if (location == Locations.Local)
                dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            dir = Path.Combine(dir, name);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }
        private String GetAppRelativeFileNameIfNecessary(String fileName, Locations location = Locations.Default)
        {
            if (!fileName.Contains("\\"))
            {
                return Path.Combine(GetAppDataDirectory(), fileName);
            }
            else
            {
                return fileName;
            }
        }


        public Task<Stream> Get(string fileName, Locations location = Locations.Default, string folder = "")
        {
            if (!String.IsNullOrEmpty(folder))
            {
                fileName = Path.Combine(folder, fileName);
                if (!System.IO.Directory.Exists(folder))
                {
                    System.IO.Directory.CreateDirectory(folder);
                }
            }
            else
            {
                fileName = GetAppRelativeFileNameIfNecessary(fileName, location);
            }

            if (System.IO.File.Exists(fileName))
            {
                var file = System.IO.File.OpenRead(fileName);
                return Task.FromResult(file as Stream);
            }
            else
            {
                return Task.FromResult(default(Stream));
            }
        }

        public Task<TObject> GetAsync<TObject>(string fileName) where TObject : class
        {
            fileName = GetAppRelativeFileNameIfNecessary(fileName);
            if (System.IO.File.Exists(fileName))
            {
                var json = System.IO.File.ReadAllText(fileName);
                var instance = JsonConvert.DeserializeObject<TObject>(json);

                return Task.FromResult(instance);
            }
            else
            {
                return Task.FromResult(default(TObject));
            }
        }

        public async Task<T> GetKVPAsync<T>(string key, T defaultValue = null) where T : class
        {
            var dictionary = await GetDictionary();
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key] as T;
            }
            else
            {
                return defaultValue;
            }
        }

        public async Task<bool> HasKVPAsync(string key)
        {
            var dictionary = await GetDictionary();
            return (dictionary.ContainsKey(key));
        }

        public Task<byte[]> ReadAllBytesAsync(string fileName)
        {
            fileName = GetAppRelativeFileNameIfNecessary(fileName);
            if (System.IO.File.Exists(fileName))
            {
                return Task.FromResult(System.IO.File.ReadAllBytes(fileName));
            }
            else
            {
                return Task.FromResult(default(byte[]));
            }
        }

        public Task<List<string>> ReadAllLinesAsync(string fileName)
        {
            fileName = GetAppRelativeFileNameIfNecessary(fileName);
            var allLines = System.IO.File.ReadAllLines(fileName);
            if (allLines != null)
            {
                return Task.FromResult(allLines.ToList());
            }
            else
            {
                return Task.FromResult(default(List<string>));
            }
        }

        public Task<string> ReadAllTextAsync(string fileName)
        {
            fileName = GetAppRelativeFileNameIfNecessary(fileName);
            return Task.FromResult(System.IO.File.ReadAllText(fileName));
        }

        public Task<string> StoreAsync(Stream stream, string fileName, Locations location = Locations.Default, string folder = "")
        {
            if (!String.IsNullOrEmpty(folder))
            {
                fileName = Path.Combine(folder, fileName);
                if (!System.IO.Directory.Exists(folder))
                {
                    System.IO.Directory.CreateDirectory(folder);
                }
            }
            else
            {
                fileName = GetAppRelativeFileNameIfNecessary(fileName, location);
            }

            if (System.IO.File.Exists(fileName))
                System.IO.File.Delete(fileName);

            using (var file = System.IO.File.Create(fileName))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(file);
            }

            return Task.FromResult(fileName);
        }

        public Task<string> StoreAsync<TObject>(TObject instance, string fileName) where TObject : class
        {
            fileName = GetAppRelativeFileNameIfNecessary(fileName);

            var json = JsonConvert.SerializeObject(instance);
            System.IO.File.WriteAllText(fileName, json);

            return Task.FromResult(fileName);
        }

        public async Task StoreKVP<T>(string key, T value) where T : class
        {
            var dictionary = await GetDictionary();
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }

            dictionary.Add(key, value);

            await PersistDictionary();
        }

        public Task<string> WriteAllBytesAsync(string fileName, byte[] buffer)
        {
            fileName = GetAppRelativeFileNameIfNecessary(fileName);
            System.IO.File.WriteAllBytes(fileName, buffer);
            return Task.FromResult(fileName);
        }

        public Task<string> WriteAllLinesAsync(string fileName, List<string> text)
        {
            fileName = GetAppRelativeFileNameIfNecessary(fileName);
            System.IO.File.WriteAllLines(fileName, text);
            return Task.FromResult(fileName);

        }

        public Task<string> WriteAllTextAsync(string fileName, string text)
        {
            fileName = GetAppRelativeFileNameIfNecessary(fileName);
            System.IO.File.WriteAllText(fileName, text);
            return Task.FromResult(fileName);
        }
    }
}
