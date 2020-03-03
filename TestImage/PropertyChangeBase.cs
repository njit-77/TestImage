using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestImage
{
    [Serializable]
    public class PropertyChangeBase : INotifyPropertyChanged
    {
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyname)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public T Clone<T>() where T : PropertyChangeBase
        {
            T result;
            using (Stream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Seek(0L, SeekOrigin.Begin);
                result = formatter.Deserialize(stream) as T;
            }
            return result;
        }
    }

    static class PropertyChangedBaseEx
    {
        public static void OnPropertyChanged<T, TProperty>(this T PropertyChangeBase, Expression<Func<T, TProperty>> propertyname) where T : PropertyChangeBase
        {
            var PropertyName = propertyname.Body as MemberExpression;
            if (null != PropertyName)
            {
                PropertyChangeBase.OnPropertyChanged(PropertyName.Member.Name);
            }
        }
    }
}
