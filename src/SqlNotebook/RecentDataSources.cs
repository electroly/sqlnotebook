using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SqlNotebook {
    public static class RecentDataSources {
        const string FILENAME = "RecentDataSources.xml";
        const int MAX_RECENTS = 10;

        public sealed class StorageItem {
            public string ImportSessionTypeName { get; set; }
            public string DisplayName { get; set; }
            public string ConnectionString { get; set; }
        }

        public sealed class StorageFile {
            public List<StorageItem> Items { get; set; } = new List<StorageItem>();

            public void RemoveFirst(Type interfaceType) {
                for (int i = 0; i < Items.Count; i++) {
                    var t = SessionNameToType(Items[i].ImportSessionTypeName);
                    if (t.GetInterfaces().Contains(interfaceType)) {
                        Items.RemoveAt(i);
                        return;
                    }
                }
                throw new InvalidOperationException($"There are no items of type {interfaceType.Name} in the list.");
            }

            public void Count(out int fileCount, out int serverCount) {
                fileCount = 0;
                serverCount = 0;
                foreach (var item in Items) {
                    var t = SessionNameToType(item.ImportSessionTypeName);
                    if (t.GetInterfaces().Contains(typeof(IFileImportSession))) {
                        fileCount++;
                    } else {
                        serverCount++;
                    }
                }
            }
        }

        public static IEnumerable<RecentDataSource> List {
            get {
                var file = LoadFile();
                return file.Items.Select(x => new RecentDataSource {
                    ImportSessionType = SessionNameToType(x.ImportSessionTypeName),
                    DisplayName = x.DisplayName,
                    ConnectionString = x.ConnectionString
                });
            }
        }

        public static IEnumerable<RecentDataSource> FileList {
            get {
                return List.Where(x => x.ImportSessionType.GetInterfaces().Contains(typeof(IFileImportSession)));
            }
        }

        public static IEnumerable<RecentDataSource> ServerList {
            get {
                return List.Where(x => x.ImportSessionType.GetInterfaces().Contains(typeof(IDatabaseImportSession)));
            }
        }

        private static Type SessionNameToType(string name) {
            var type = Type.GetType(name);
            if (type.GetInterfaces().Any(x => x == typeof(IImportSession))) {
                return type;
            } else {
                throw new Exception($"Unexpected type \"{name}\".");
            }
        }

        public static void Add(RecentDataSource source) {
            var file = LoadFile();

            foreach (var oldItem in file.Items.Where(x => x.DisplayName.ToLower() == source.DisplayName.ToLower()).ToList()) {
                file.Items.Remove(oldItem);
            }

            file.Items.Add(new StorageItem {
                ImportSessionTypeName = source.ImportSessionType.FullName,
                DisplayName = source.DisplayName,
                ConnectionString = source.ConnectionString
            });

            int fileCount, serverCount;
            file.Count(out fileCount, out serverCount);
            for (; fileCount > MAX_RECENTS; fileCount--) {
                file.RemoveFirst(typeof(IFileImportSession));
            }
            for (; serverCount > MAX_RECENTS; serverCount--) {
                file.RemoveFirst(typeof(IDatabaseImportSession));
            }

            SaveFile(file);
        }

        private static string GetPath() {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SQL Notebook");
        }

        private static StorageFile LoadFile() {
            try {
                var filePath = Path.Combine(GetPath(), FILENAME);
                var serializer = new XmlSerializer(typeof(StorageFile));
                using (var stream = File.OpenRead(filePath)) {
                    var file = (StorageFile)serializer.Deserialize(stream);
                    return file;
                }
            } catch {
                return new StorageFile();
            }
        }

        private static void SaveFile(StorageFile file) {
            try {
                var path = GetPath();
                Directory.CreateDirectory(path);
                var filePath = Path.Combine(GetPath(), FILENAME);
                var serializer = new XmlSerializer(typeof(StorageFile));
                using (var stream = File.CreateText(filePath)) {
                    serializer.Serialize(stream, file);
                }
            } catch { }
        }
    }
}
