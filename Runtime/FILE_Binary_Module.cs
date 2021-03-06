#region Access
//using System;
//using System.Threading.Tasks;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endregion
namespace Architecture.FILE_Binary
{
    public static partial class Key
    {
        public static string Path   = "FILE_Binary_Path";
        public static string Load   = "FILE_Binary_Load";
        public static string Save   = "FILE_Binary_Save";
        public static string Exist  = "FILE_Binary_Exist";
        public static string Delete = "FILE_Binary_Delete";
    }
    public sealed partial class FILE_Binary_Module : Module
    {
        #region Reactions ( On___ )
        // Contenedor de toda las reacciones del BASE_FILE_Binary
        protected override void OnSubscription(bool condition)
        {
            //PATH
            Middleware<string, string>.Subscribe_Publish(condition, Key.Path, Path);

            //LOADS
            Middleware<string, (bool isLoaded, object value)>.Subscribe_Publish(condition, Key.Load, Load);

            //SAVES
            Middleware<(string path, object value)>.Subscribe_Publish(condition, Key.Save, Save);

            //EXIST
            Middleware<string, bool>.Subscribe_Publish(condition, Key.Exist, Exist);

            //DELETE
            Middleware<string>.Subscribe_Publish(condition, Key.Delete, Delete);
        }
        #endregion
        #region Methods
        // Contenedor de toda la logica del BASE_FILE_Binary

        private static string Path(string path) => Application.persistentDataPath + path; //TODO Llamar al modulo Application

        private static bool Exist(string path) => File.Exists(Path(path)); //TODO Llamar al modulo FIle ?

        private static void Delete(string path) => File.Delete(Path(path)); //TODO Llamar al modulo FIle ?

        private void Save((string path, object value) value)
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(Path(value.path), FileMode.Create);
            _formatter.Serialize(_stream, value.value);
            _stream.Close();
        }

        private (bool isLoaded, object value) Load(string path)
        {
            //Debug.Log("Loading...");

            if (Exist(path))
            {
                BinaryFormatter _formatter = new BinaryFormatter();
                FileStream _stream = new FileStream(Path(path), FileMode.Open);
                object storedData = default;
                storedData = _formatter.Deserialize(_stream);
                _stream.Close();
                #if UNITY_EDITOR
                    UnityEditor.AssetDatabase.Refresh();
                #endif
                return (true, storedData);
            }
            else
            {
                Debug.Log($"No hay archivo para {path}");
                return (false, null);
            }
        }
        #endregion
        #region Request ( Coroutines )
        // Contenedor de toda la Esperas de corutinas del BASE_FILE_Binary
        #endregion
        #region Task ( async )
        // Contenedor de toda la Esperas asincronas del BASE_FILE_Binary
        #endregion
    }
}


/*

TODO:

1. Crear un diccionario de string y object, donde dicho object sea el cache del dato que hemos cargado de los archivos
1.1 Si alguien carga un archivo, revisa si hay guardado en cach?? y lo usa, sino carga y asigna en cach??
1.2 Si alguien coloca nuevos datos, estos tambien actualizar??n los datos en cach??

2. Crear un array de strings que representan los path de cada archivo creado por el modulo binario
2.1 cada vez que alguien cree un nuevo archivo se a??ade aqu??
2.2 cada vez que alguien elimine un archivo de los del path se actualiza aqu??
2.3 dar segunda opci??n de busqueda que cuando alguien quiere hacer busqueda de los archivos se revisa el string[] y no usa File.Exist

3. Tener una opci??n que permita limpiarlo todo

- Se crear?? un segundo archivo que contendr?? un 


*/
