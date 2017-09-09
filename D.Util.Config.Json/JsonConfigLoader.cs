using D.Util.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Util.Config
{
    /// <summary>
    /// json 配置
    /// </summary>
    public class JsonConfigLoader : IConfigLoader
    {
        #region const 变量

        /// <summary>
        /// path 的分隔字符串
        /// </summary>
        const char _splitChar = '.';
        #endregion

        #region 字段
        /// <summary>
        /// json 文件中读取的 root json 对象
        /// </summary>
        JObject _root;

        /// <summary>
        /// 所有从 root 中取出来的 config 
        /// save 需要做的事情就是把 items 中的内容全部保存到 fileContent 然后保存到文件
        /// </summary>
        Dictionary<string, IConfig> _items;

        /// <summary>
        /// json 文件的路径
        /// </summary>
        string _filePath;
        #endregion

        public JsonConfigLoader(string filePath)
        {
            _filePath = filePath;

            _items = new Dictionary<string, IConfig>();
        }

        #region IConfigLoader
        public T Load<T>(string instanceName)
            where T : class, IConfig, new()
        {
            if (_root == null)
            {
                lock (this)
                {
                    LoadFile();
                }
            }

            var config = new T();

            var path = string.IsNullOrEmpty(instanceName) ? config.Path : config.Path + _splitChar + instanceName;

            if (_items.ContainsKey(path))
            {
                config = _items[path] as T;
            }
            else
            {
                var jsonItem = JsonValue(path);

                if (jsonItem != null)
                {
                    try
                    {
                        config = jsonItem.ToObject<T>();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"从配置文件 { _filePath } 读取配置项 { path } 失败", ex);
                    }
                }

                _items.Add(path, config);
            }

            return config;
        }

        public void Save()
        {
            lock (this)
            {
                foreach (var path in _items.Keys)
                {
                    JsonValue(path, _items[path]);
                }

                SaveJsonToFile();
            }
        }
        #endregion

        /// <summary>
        /// 根据文件路径加载 json 配置文件的所有内容到 root 对象中
        /// </summary>
        private void LoadFile()
        {
            //如果配置文件不存在
            if (!File.Exists(_filePath))
            {
                _root = new JObject();
                return;
            }

            using (StreamReader sr = new StreamReader(_filePath))
            {
                try
                {
                    JsonReader jr = new JsonTextReader(sr);

                    _root = JObject.Load(jr);
                }
                catch (Exception ex)
                {
                    throw new Exception($"读取配置文件 { _filePath } 失败：{ ex.ToString() }");
                }
            }
        }

        /// <summary>
        /// 获取 file content 中值
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private JToken JsonValue(string path)
        {
            JToken value = _root;

            var nameArray = path.Split(_splitChar);

            foreach (var name in nameArray)
            {
                value = value[name];

                if (value == null)
                {
                    return value;
                }
            }

            return value;
        }

        /// <summary>
        /// 向 file content 中添加或者保存配置
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        private void JsonValue(string path, object value)
        {
            if (value == null)
            {
                return;
            }

            JObject content = _root;

            var nameArray = path.Split(_splitChar);

            var name = nameArray[0];

            for (var i = 0; i < nameArray.Length - 1; i++)
            {
                if (content.Property(name) == null)
                {
                    content.Add(name, new JObject());
                }

                content = content[name] as JObject;

                name = nameArray[i + 1];
            }

            if (value.GetType() == typeof(string))
            {
                content.Remove(name);
                content.Add(name, value as string);
            }
            else
            {
                var js = new JsonSerializer();
                js.ContractResolver = new WritablePropertiesOnlyResolver();

                content[name] = JObject.FromObject(value, js);
            }
        }

        /// <summary>
        /// 将 file content 保存到文件中
        /// </summary>
        private void SaveJsonToFile()
        {
            using (StreamWriter wr = new StreamWriter(_filePath))
            {
                try
                {
                    JsonWriter jr = new JsonTextWriter(wr);
                    jr.Formatting = Formatting.Indented;

                    _root.WriteTo(jr);

                    jr.Close();
                    wr.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("保存配置到 json 文件失败：" + ex.ToString());
                }
            }
        }
    }
}
