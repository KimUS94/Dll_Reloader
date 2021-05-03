using System;
using System.Reflection;

namespace FX.Framework.Assembly
{
    public class AssemblyLoader : MarshalByRefObject
    {
        private System.Reflection.Assembly m_Assembly;
        private Type m_Type;
        private object m_instance;

        public AssemblyLoader()
        {
            m_Assembly = null;
            m_Type = null;
            m_instance = null;
        }

        ~AssemblyLoader()
        {
            m_Assembly = null;
            m_Type = null;
            m_instance = null;
        }

        public void Load(string path)
        {
            this.m_Assembly = System.Reflection.Assembly.Load(AssemblyName.GetAssemblyName(path));
        }

        public object ExecuteMethod(string strModule, string methodName, params object[] parameters)
        {
            foreach (System.Type type in this.m_Assembly.GetTypes())
            {
                if (String.Compare(type.Name, strModule, true) == 0)
                {
                    this.m_Type = type;
                    this.m_instance = m_Assembly.CreateInstance(type.FullName);
                    break;
                }
            }

            //MethodInfo MyMethod = MyType.GetMethod(methodName, new Type[] { typeof(int), typeof(string), typeof(string), typeof(string) });
            //MyMethod.Invoke(inst, BindingFlags.InvokeMethod, null, parameters, null);

            MethodInfo MyMethod = this.m_Type.GetMethod(methodName);
            object obj = MyMethod.Invoke(this.m_instance, BindingFlags.InvokeMethod, null, parameters, null);

            return obj;
        }
    }
}
